using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeView : MonoBehaviour
{

   // private GameObject selectedPart;
    public GameObject test;
    public GameObject test2;
    private GameObject camera;
    private GameObject plane_left;
    private GameObject plane_right;
    private Camera eye_left;
    private Camera eye_right;
    private RenderTexture texture_left;
    private RenderTexture texture_right;
    private bool isVisible;
        private GameObject lastHitObject;  


    void Start(){
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        refresh(test);
        isVisible=true;
        toggleView(true);
        
    }

    void Update(){   


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit)){
            GameObject hitObject = hit.transform.gameObject;

            if(hitObject.layer == 7){
                if (lastHitObject != hitObject){
                    lastHitObject = hitObject;
                    refresh(hitObject);
                }
            }
        }

        toggleView();
    }

    public void refresh(GameObject obj){


       // Debug.Log("X1");
        GameObject root = traverseToRoot(obj);

        
       // Debug.Log("X2");

        //camera = 
        plane_left = GameObject.Find("Projection_Left_Eye");
        plane_right = GameObject.Find("Projection_Right_Eye");
        
       // plane_left = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0).gameObject;
       // plane_right = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(1).gameObject;

        //plane_left = root.transform.parent.gameObject.FindGameObjectWithTag("MainCamera").GetChild(0).gameObject;
       // plane_right = root.transform.parent.gameObject.FindGameObjectWithTag("MainCamera").GetChild(1).gameObject;

        //plane_left = camera.transform.GetChild(0).gameObject;
        //plane_right = camera.transform.GetChild(1).gameObject;


//Debug.Log("\nselected: " + obj.name + " --- \nroot: " + root.name);

        //Debug.Log( "\nthing: " + root.transform.Find("Spine/Base_BONE/Torso_BONE/inter1/Neck_BONE/head_21_visual/Eye_View_Left").gameObject.name);


       
        //eye_left = root.transform.Find("Spine/Base_BONE/Torso_BONE/inter1/Neck_BONE/head_21_visual/Eye_View_Left").gameObject.GetComponent<Camera>();
        //eye_right = root.transform.Find("Spine/Base_BONE/Torso_BONE/inter1/Neck_BONE/head_21_visual/Eye_View_Right").gameObject.GetComponent<Camera>();


// Debug.Log( "\nthing: " + root.transform.Find("Eye_View_Left").gameObject.name);

        eye_left = root.transform.Find("Eye_View_Left").gameObject.GetComponent<Camera>();
        eye_right = root.transform.Find("Eye_View_Right").gameObject.GetComponent<Camera>();


        attachTexture(eye_left, plane_left);
        attachTexture(eye_right, plane_right);       
    }



    public GameObject traverseToRoot(GameObject obj){

        Transform part = obj.transform;

        while (!part.name.Contains("NICO") && part.parent != null){
            part = part.parent;
        }
        return part.gameObject;
    }

    public void attachTexture(Camera cam, GameObject plane){
        RenderTexture rt = new RenderTexture(1024, 1024, 24);
        cam.targetTexture = rt;
        Renderer planeRenderer = plane.GetComponent<Renderer>();
        planeRenderer.material.mainTexture = rt;
        
    }

    public void toggleView(bool init = false){
        plane_left = GameObject.Find("Projection_Left_Eye");
        plane_right = GameObject.Find("Projection_Right_Eye");

        if (Input.GetKeyDown(KeyCode.T) || init){
            toggle(plane_left);
            toggle(plane_right);
            isVisible = !isVisible;
        }
        
    }

    public void toggle(GameObject plane){
        Renderer renderer = plane.GetComponent<Renderer>();
        if(isVisible){
            renderer.enabled = false;
            //plane.transform.position.y= -30;
        }else{
            renderer.enabled = true;
            //plane.transform.position
        }
    }

}
