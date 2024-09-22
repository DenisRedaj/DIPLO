using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class MoveArm_Test : MonoBehaviour {
	public TextMeshProUGUI button;
    public GameObject bone;

/*  public struct NICO_bone {

        public NICO_bone(GameObject x, GameObject y, GameObject z){

        } 
    }


    public struct NICO_body {
        NICO_bone base,
        NICO_bone torso,
        NICO_Bone neck
    }*/

	void Start () {

	}

	public void clickTest(){
        Debug.Log ("button clicked" + bone.name + "transform: "+ bone.transform.rotation);

        bone.transform.rotation = Quaternion.Euler(0, 30, 0);
	}


    public void slowRotate(){

        double degrees = 30;
        double total_time = 5;
        double refresh_rate = 0.01;

        double step = degrees / (total_time/refresh_rate);

        int start = 0;
        int counter = 0;

        while (start != degrees && step != 0) {
            start += (int)step;
            bone.transform.rotation = Quaternion.Euler(0, start, 0);
            if(counter == 1500) break;
            counter++;
        }

        Debug.Log ("slow rotate! " + bone.name + "transform: "+ bone.transform.rotation);

        
	}


    private float waitTime = 2.0f;
    private float frameRate = 120;
    //private float start_rotation = 0.933f;
    private float desired_rotation = 0.933f;
    private float timer = 0;

    int counter = 0;


    public void waitRotate(){



        var time = 5.0f;
        var dps = bone.transform.eulerAngles.y / time;


        timer += Time.deltaTime;
        var interval = frameRate / ((float)desired_rotation/waitTime);
        var counter1 = waitTime/interval;


        if (timer > interval && counter1 != 0)
        {   
            counter--;
            timer = timer - interval;
            bone.transform.Rotate(new Vector3(0, dps * Time.deltaTime, 0));
            Debug.Log ("wait rotate! " + bone.name + "transform: "+ bone.transform.rotation);
            //newRotate();
        }

        if(counter != 1500) {waitRotate();}
        
        counter++;

    }


    public void timeRotate(){

        

        while(bone.transform.eulerAngles.y > 0.93 ){
            newRotate();
            if(counter == 1500) break;
            counter++;
        }
        
    }

    
    private void newRotate(float time){

        var dps = bone.transform.eulerAngles.y / time;

        bone.transform.Rotate(new Vector3(0, dps * Time.deltaTime, 0));

        Debug.Log ("new rotate! " + bone.name + "transform: "+ bone.transform.rotation);

	}


    private void newRotate(){

        var time = 5.0f;
        var dps = bone.transform.eulerAngles.y / time;

        bone.transform.Rotate(new Vector3(0, dps * Time.deltaTime, 0));

        Debug.Log ("new rotate! " + bone.name + "transform: "+ bone.transform.rotation);

	}

}