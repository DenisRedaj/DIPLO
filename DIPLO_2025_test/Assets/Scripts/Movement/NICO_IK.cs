using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

using TMPro;
using System.Linq;

using System.Reflection;
using System.IO;



public class NICO_IK : MonoBehaviour
{

    [Serializable]
    public struct IK_Bone {
        public GameObject bone;
        public Transform model;
        public Restrict restrict;
    }

    [Serializable]
    public enum Restrict{
        Y_Only,    
        X_Only,       
        Z_Only,
        XZ_Only,
        YZ_Only,
        XY_Only
    };

    public class UserData : MonoBehaviour
    {
        public string name;
        public string parent_name;
        public int group;

        public void setParentName(string parentname){
            parent_name = parentname;
        }

         public void setName(string partname){
            name = partname;
        }

        public void setGroup(int groupname){
            group = groupname;
        }
    }

    public IK_Bone[] bones;

    public Transform EE_Target;
    public int iterations;
    private Canvas canvas;
    private bool check;
    public Transform[] allBones;

    public TMP_InputField input_load;
    public TMP_InputField input_save;

    private List<GameObject> BACKUP_A = new List<GameObject>();
    private List<GameObject> BACKUP_B = new List<GameObject>();
    private List<GameObject> BACKUP_C = new List<GameObject>();

    //private List<float> IK_angles = new List<float>();
    private List<List<float>> IK_angles = new  List<List<float>>();
    private List<string> IK_angles_headers = new  List<string>();

    public List<List<GameObject>> boneList = new List<List<GameObject>>();
    public List<List<UserData>> userData = new List<List<UserData>>();

    public JointConstraints script;
    public Dictionary<string, Vector3> axis_list;



    public Vector3 lastPos;
    
    void Start(){
        canvas = GetComponent<Canvas>();
        
        extractBones();
        
        script = canvas.GetComponent<JointConstraints>();
        axis_list = script.getJointConstraints();
        lastPos = EE_Target.transform.position;

        input_load.text = "test.csv";
        input_save.text = "test.csv";
        check = true;
    }


    void Update()
    {
        if (EE_Target.transform.position != lastPos){
            solve();
            check = false;
            lastPos = EE_Target.transform.position;
        }
    }

    void solve(){

        generate_HelperBones();

        for (int i = 0; i<iterations; i++) {
           
            EE_to_Root();
        }
        //IK_match();
    }



    public void extractBones(){
        List<GameObject> NICO_group = new List<GameObject>();
        
        allBones = GetComponentsInChildren<Transform>(true);
        foreach (var bone in allBones.Where(bone => (bone != transform))){
            if(bone.name.StartsWith("NICO")){
                NICO_group.Add(bone.gameObject);
            }   
        }

        for (int i=0; i<NICO_group.Count; i++){
            List<GameObject> parts = new List<GameObject>();
            List<UserData> udata = new List<UserData>();
            iterate(NICO_group[i], parts, udata);
        }

        for(int i=0; i<boneList.Count; i++){
            for(int j=0; j<boneList[i].Count; j++){
                reparent(boneList[i][j], canvas.transform);
            }
        }

        Debug.Log("CHECKING EXTRACT BONES\n bonelist count ... " + boneList[0][0].name + "\n userdata count ... " + userData[0][0].parent_name + "\n NICO-group count ... " + NICO_group.Count);
    }

    public void iterate(GameObject obj, List<GameObject> parts, List<UserData> udata){

        Debug.Log( " ITERATE 1 : " + obj.name + " ... "+ obj.transform.childCount);  
        for (int i=0; i<obj.transform.childCount; i++){

            GameObject child = obj.transform.GetChild(i).gameObject;

            Debug.Log( " FOREACH NAME : " + child.name + " ... "  + i + " : " + obj.transform.childCount );  

            UserData ud = new UserData();
            
            if(child.name.Contains("_BONE")){
                ud.setName(child.name); 
                ud.setParentName(findParentBone(child));  
                ud.setGroup(i); 
                
                udata.Add(ud);
                parts.Add(child.gameObject);

                userData.Add(udata);
                boneList.Add(parts);
            }

            iterate(child, parts, udata);
        
        }  
    }


     public void IK_match(){
        for (int i=0; i<bones.Length-2; i++){

            Debug.Log("IK_MATCH ... " +  i + " ///  " + bones[i].bone.name + " /// ");
            bones[i].bone.transform.forward = BACKUP_B[i].transform.right;
            bones[i].bone.transform.position = BACKUP_C[i].transform.position;
        }
    }

    // MARK BUTTON
    public void extractAngles(int group=0){

        List<float> list = new List<float>();

        for (int i=0; i<boneList[group].Count; i++){
            Debug.Log( " EXTRACT ANGLES .... "  + userData[group][i].parent_name );
            int index = boneList[group].FindIndex(obj => (obj.name == userData[group][i].parent_name));

            if(index != -1){
                float angle = calcAngle(
                    boneList[group][i].transform,
                    boneList[group][index].transform
                );
                
                IK_angles_headers.Add(boneList[group][i].name);
                list.Add(angle);
            }
        }
        IK_angles.Add(list);
    }

    
    public float calcAngle(Transform child, Transform parent){
        float angle = Vector3.Angle(parent.position-child.position, child.forward);
        Debug.Log(" angle ... " + angle + " ... parent: " + parent.gameObject.name + " .... child: "+ child.gameObject.name);
        return angle;
    }

    public string findParentBone(GameObject root){

        string root_OG = root.name;
        root = root.transform.parent.gameObject;
        do {
           if(!root.name.Contains("_BONE")){root = root.transform.parent.gameObject;}
        }while(!root.name.Contains("_BONE") && !root.name.Contains("Canvas"));

        Debug.Log(" findParentBone ... " + root_OG + "'s parent is ... " + root.name);

        return root.name;
    }

    public void clickTest(){

        //action_IK();

       // StartCoroutine(action_IK(0, 0, 45, 2));
        
        StartCoroutine(rotateJoint(bones[5].bone, 0, 45, 2));
        StartCoroutine(rotateJoint(bones[1].bone, 0, 45, 2,3));
	}

    public IEnumerator rotateJoint(GameObject bone, int group, float angle, float time, float delay=0){

        float axis = (angle >= 0) ? 1 : -1;
        angle = (angle >= 0) ? angle : -angle;

        yield return new WaitForSeconds(delay);
        Coroutine routine = StartCoroutine(action(getChildren(bone,group),bone,group, angle,axis,time));
        yield return new WaitForSeconds(delay+time);
        StopCoroutine(routine);
        yield return null;
    }

    public List<GameObject> getChildren(GameObject bone, int nx=0){

        List<UserData> data = new List<UserData>();
        List<GameObject> stack = new List<GameObject>();
        List<GameObject> children = new List<GameObject>();

        stack.Add(bone);
        
        int tally = 0;
        // iterate
        do {   
            printList(stack, tally);
            data = userData[nx].Where(x=> x.parent_name == stack[0].name).ToList();  

            if(data != null){
                for(int i=0;i<data.Count;i++){
                    int inx = userData[nx].FindIndex(x => ReferenceEquals(x, data[i]));
                    stack.Add(boneList[nx][inx]);
                }
                children.Add(stack[0]);
                stack.RemoveAt(0);
            }
            
            
            tally++;    
        }while ((stack.Count > 0) && (tally < 50));

        return children;
    }
    public void printList(List<GameObject> stack, int tally){
        string stack_check = "";
        for (int j = 0; j < stack.Count; j++){
            stack_check += "\n #" + stack[j].name + "; ";
        }
    }
   


    public IEnumerator action(List<GameObject> children, GameObject bone, int group, float angle, float axis, float time){

        children = getChildren(bone,group);

        float speed = angle/time;
        float deltaAngle = 0;

        do{
            deltaAngle += speed * Time.deltaTime;
            deltaAngle = Mathf.Min(deltaAngle, angle);
            
            Vector3 new_axis = Quaternion.Inverse(bone.transform.rotation) * axis_list[bone.name];  

            for (int i = 0; i<children.Count; i++) {
                children[i].transform.RotateAround(bone.transform.position, new_axis, speed * Time.deltaTime);
            }
            yield return null;
        } while(deltaAngle < angle);

        yield return new WaitForSeconds(.1f);
    }


    public void action_IK(int group=0){

        for (int i=bones.Length-2; i>=0; i--){
            if (i!=0) {
                float angle = calcAngle(
                    bones[i].bone.transform,
                    bones[i+1].bone.transform
                );
               StartCoroutine(rotateJoint(bones[i].bone, group, angle, 2));
            }
        }
    }

    // SAVE CSV BUTTON
    public void save_CSV(){
        string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(desktopPath, input_save.text);

        using (StreamWriter writer = new StreamWriter(filePath)){

            writer.WriteLine(string.Join(",",IK_angles_headers));

            foreach (var row in IK_angles){
                string line = string.Join(",", row);
                writer.WriteLine(line);
            }
        }
        Debug.Log("CSV file saved to: " + filePath);
    }

    // LOAD CSV BUTTON
    public void load_CSV(int group=0){

        string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(desktopPath, input_load.text);

        if (File.Exists(filePath)){
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 1; i < lines.Length; i++) {
                string[] angles = lines[i].Split(',');

                for(int j=0; j < angles.Length; j++){
                    
                    string[] headers = lines[0].Split(',');

                    int index = boneList[group].FindIndex(obj => (obj.name == headers[j]));

                    if(index != -1){
                        GameObject gx = boneList[group][index];
                        StartCoroutine(rotateJoint(gx, group, float.Parse(angles[j]) , 2));
                    }
                    
                }
            }
        }else {
            Debug.LogError("CSV " + input_load.text + " not found");
        }
    }




    public void generate_HelperBones(){

        if(!BACKUP_A.Any()){
            for (int i=0; i<bones.Length-1; i++){

                GameObject ax = GameObject.CreatePrimitive(PrimitiveType.Cube);
                ax.name = "a"+i;
                ax.transform.position = bones[i].bone.transform.position; 
                ax.transform.rotation = bones[i].bone.transform.rotation;
                ax.transform.up = bones[i].bone.transform.up;
                ax.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                GameObject bx = GameObject.CreatePrimitive(PrimitiveType.Cube);
                bx.name = "b"+i;
                bx.transform.position = bones[i+1].bone.transform.position;
                bx.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                GameObject cx = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cx.name = "c"+i;
                cx.transform.position = bones[i+1].bone.transform.position;
                cx.transform.LookAt(ax.transform);
                
                cx.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            
                Color color = new Color(((float)i*85f)/255f, ((float)i*170f)/255f, ((float)i*255f)/255f, 1f);
                
                ax.GetComponent<Renderer>().material.color = color;
                bx.GetComponent<Renderer>().material.color = color;
                cx.GetComponent<Renderer>().material.color = color;

                cx.GetComponent<Renderer>().enabled = false;
                bx.GetComponent<Renderer>().enabled = false;


                cx.transform.parent = canvas.transform;
                bx.transform.parent = canvas.transform;
                ax.transform.parent = canvas.transform;

                rotate_towards(bx.transform, axis_list[bones[i+1].bone.name]);

                cx.transform.parent = canvas.transform;
                ax.transform.parent = cx.transform; 
                bx.transform.parent = cx.transform; 

                BACKUP_A.Add(ax);
                BACKUP_B.Add(bx);
                BACKUP_C.Add(cx);


            }
        }
    }

    public void rotate_towards(Transform obj, Vector3 targetDirection){
            targetDirection.Normalize();
            Vector3 currentDirection = obj.forward;
            Quaternion rotationDelta = Quaternion.FromToRotation(currentDirection, targetDirection);

            obj.rotation = rotationDelta * obj.rotation;
    }

    public void direct_rotation(GameObject ax, Transform end){
        ax.transform.LookAt(end);
    }

    public void reparent(GameObject ax, Transform bx){
        bx.transform.SetParent(canvas.transform);
        ax.transform.SetParent(bx.transform);
    }


    public void reparent_config_A(GameObject ax, GameObject bx, GameObject cx){
        cx.transform.parent = canvas.transform;
        bx.transform.parent = canvas.transform;
        ax.transform.parent = canvas.transform;

        cx.transform.parent = ax.transform; 
    }

    public void reparent_config_C(GameObject ax, GameObject bx, GameObject cx){
        cx.transform.parent = canvas.transform;
        bx.transform.parent = canvas.transform;
        ax.transform.parent = canvas.transform;

        ax.transform.parent = cx.transform;  
    }

    public void reparent_config_X(int i){
        if(i != BACKUP_A.Count-1)  BACKUP_B[i].transform.parent = BACKUP_C[i+1].transform.parent;
    }

    public void reparent_reset(){
        for (int i = 0; i<BACKUP_C.Count; i++){
            reparent_config_C(BACKUP_A[i],BACKUP_B[i],BACKUP_C[i]);
        }
    }


    public void EE_to_Root(){


        // from EE to root
        for (int i = 0; i<BACKUP_A.Count; i++){

            reparent_config_X(i);
            if(i == 0){
                direct_rotation(BACKUP_C[i], EE_Target);
                reparent_config_A(BACKUP_A[i],BACKUP_B[i],BACKUP_C[i]);
                
            
                snap(BACKUP_A[i], EE_Target);
            }else{
                direct_rotation(BACKUP_C[i], BACKUP_C[i-1].transform);
                reparent_config_A(BACKUP_A[i],BACKUP_B[i],BACKUP_C[i]);
                snap(BACKUP_A[i], BACKUP_C[i-1].transform);
            }
            reparent_config_C(BACKUP_A[i],BACKUP_B[i],BACKUP_C[i]);
        }

        // from root to EE
        for (int i = BACKUP_A.Count-1; i>=0; i--){

            if(i == BACKUP_A.Count-1){
                snap(BACKUP_C[i], bones[BACKUP_A.Count].bone.transform);
            }else{
                snap(BACKUP_C[i], BACKUP_A[i+1].transform);
            }
            align_along_axis(BACKUP_C[i], bones[i+1].restrict, BACKUP_B[i]);
        }
    }




    public void align_along_axis(GameObject cx, Restrict restrict, GameObject bx){

        Quaternion c_rotation = cx.transform.rotation;
        Quaternion b_rotation = bx.transform.rotation;

        float new_x = 0;
        float new_y = 0;
        float new_z = 0;

        if (restrict == Restrict.X_Only) {
            new_x = cx.transform.rotation.eulerAngles.x;
            new_y = bx.transform.rotation.eulerAngles.y;
            new_z = bx.transform.rotation.eulerAngles.z;
        }
        if (restrict == Restrict.Z_Only) {
            new_x = bx.transform.rotation.eulerAngles.x;
            new_y = cx.transform.rotation.eulerAngles.y;
            new_z = cx.transform.rotation.eulerAngles.z;
        }

        if (restrict == Restrict.Y_Only) {
            new_x = bx.transform.rotation.eulerAngles.x;
            new_y = cx.transform.rotation.eulerAngles.y;
            new_z = bx.transform.rotation.eulerAngles.z;
        }

        if (restrict == Restrict.XZ_Only) {
            new_x = cx.transform.rotation.eulerAngles.x;
            new_y = bx.transform.rotation.eulerAngles.y;
            new_z = cx.transform.rotation.eulerAngles.z;
        }

        if (restrict == Restrict.XY_Only) {
            new_x = bx.transform.rotation.eulerAngles.x;
            new_y = bx.transform.rotation.eulerAngles.y;
            new_z = cx.transform.rotation.eulerAngles.z;
        }

        if (restrict == Restrict.YZ_Only) {
            new_x = cx.transform.rotation.eulerAngles.x;
            new_y = cx.transform.rotation.eulerAngles.y;
            new_z = bx.transform.rotation.eulerAngles.z;
        }

        cx.transform.rotation = Quaternion.Euler(new_x,new_y,new_z);
        bx.transform.rotation = Quaternion.Euler(new_x,new_y,new_z);
    }

    public void snap(GameObject bone, Transform end){
        bone.transform.position = end.transform.position;
    }

}