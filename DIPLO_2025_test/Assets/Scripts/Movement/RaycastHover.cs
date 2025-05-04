using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RaycastHover : MonoBehaviour
{
    
    public Slider slider;
    public float minValue = -5f; 
    public float maxValue = 5f; 



    private Color originalColor;
    private Color previousColor;
    private Renderer highlightedRenderer;  
    private GameObject lastHitObject;  
    private GameObject clickedObject;

    private GameObject camera;


    public TMP_Text m_TextComponent;

    Arm_Test script;
    //EyeView viewscript;

    void Start()
    {   
        script = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Arm_Test>();
        script.extractBones();

        camera = GameObject.FindGameObjectWithTag("MainCamera");
        //viewscript = camera.GetComponent<EyeView>();

        slider.minValue = minValue;
        slider.maxValue = maxValue;
        UpdateCubePosition(slider.value);
        slider.onValueChanged.AddListener(UpdateCubePosition);
    }

    void Update()
    {   
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        

        if (Physics.Raycast(ray, out hit)){
            GameObject hitObject = hit.transform.gameObject;

            if(hitObject.layer == 7){

                if (Input.GetMouseButtonDown(0)){
                    if (clickedObject != hitObject){

                        
                        OnObjectClick(hitObject, hit);
                    }
                }

                if (lastHitObject != hitObject){
                    ResetHighlight();
                    lastHitObject = hitObject;
                    HighlightObject(hitObject);


                    //viewscript.refresh(hitObject.GetComponent<GameObject>());
                }
            }
        }else{
            ResetHighlight();
        }
    }


    void effectorGenerate(RaycastHit hit){
        GameObject effector = GameObject.CreatePrimitive(PrimitiveType.Cube);
        effector.AddComponent<MoveBox>();
        effector.name = "move_me";
        effector.layer = LayerMask.NameToLayer("Move_Box");
        Color brown = new Color(139f/255f, 69f/255f, 19f/255f, 1f);
        effector.GetComponent<Renderer>().material.color = brown;
        Instantiate(effector, hit.point, Quaternion.identity);  
    }

    void UpdateCubePosition(float value)
    {
        GameObject cube = GameObject.Find("move_me");
        if (cube != null){
           cube.transform.position = new Vector3(value, cube.transform.position.y, cube.transform.position.z);
        }
    }

    void effectorDestroy(){
        
    }




    void HighlightObject(GameObject hitObject){
        
        Renderer renderer = hitObject.GetComponent<Renderer>(); 
        if (renderer != null){
            originalColor = renderer.material.color;
            renderer.material.color = Color.green;
            highlightedRenderer = renderer;

            //Debug.Log(" name ... " + hitObject.GetComponent<Collider>().name);

            //viewscript.refresh(hitObject.transform.parent.gameObject);
            //viewscript.refresh(this.lastHitObject,camera);
             
        }
    }

    void ResetHighlight(){
        if (highlightedRenderer != null && highlightedRenderer.material.color != Color.red){
            highlightedRenderer.material.color = originalColor;
            highlightedRenderer = null;
        }
    }

    void OnObjectClick(GameObject clickedObject, RaycastHit hit){


        effectorGenerate(hit);
        Renderer renderer = clickedObject.GetComponent<Renderer>();

        if (this.clickedObject != null){
            ResetObjectColor(this.clickedObject);
        }
        if (renderer != null){
            renderer.material.color = Color.red;
            m_TextComponent.text = clickedObject.transform.name;
        }


        GameObject joint = clickedObject.transform.parent.gameObject.transform.parent.gameObject;


        this.clickedObject = clickedObject;
        //Debug.Log("Object clicked: " + clickedObject.name+ " --- " + joint.name);
        previousColor = originalColor;
    }

    void ResetObjectColor(GameObject obj){
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null){
            renderer.material.color = previousColor;
        }
    }
}
