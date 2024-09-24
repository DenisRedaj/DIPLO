using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class Arm_Test : MonoBehaviour
{
    public TextMeshProUGUI button;
    public GameObject bone;
    public GameObject bone2;
     public GameObject bone3;
    void Start(){}

    IEnumerator rotateJoint(GameObject bone, float angle, Vector3 axis, float time, float delay=0){

        yield return new WaitForSeconds(delay);
        Coroutine routine = StartCoroutine(action(bone,angle,axis,time,delay));
        yield return new WaitForSeconds(delay+time);
        StopCoroutine(routine);
        yield return null;
    }

    IEnumerator action(GameObject bone, float angle, Vector3 axis, float time, float delay=0){

        

        Quaternion startRotation = bone.transform.rotation;
        float speed = angle/time;
        float deltaAngle = 0;

        do{
            deltaAngle += speed * Time.deltaTime;
            deltaAngle = Mathf.Min(deltaAngle, angle);

            bone.transform.rotation = startRotation * Quaternion.AngleAxis(deltaAngle, axis);

            yield return null;
            
        } while (deltaAngle < angle);

        yield return new WaitForSeconds(.1f);

    }





        /*
        back	... Vector3(0, 0, -1).
        forward	... Vector3(0, 0, 1).

        left	... Vector3(-1, 0, 0).
        right	... Vector3(1, 0, 0).
        
        up	    ... Vector3(0, 1, 0).
        down	... Vector3(0, -1, 0).
        
        zero	... Vector3(0, 0, 0).
        one	    ... Vector3(1, 1, 1).
        */



        /*
        
            NICO Bone Rotations

        
        
        */

    public void clickTest(){
        Debug.Log ("button clicked" + bone.name + "transform: "+ bone.transform.rotation);
        StartCoroutine(rotateJoint(bone2, 125, Vector3.right, 2));
        StartCoroutine(rotateJoint(bone, 65, Vector3.up, 2, 3));
        StartCoroutine(rotateJoint(bone2, 55, Vector3.left, 2, 5));     // to avoid jittering, don't use negative numbers ... flip vec3
        StartCoroutine(rotateJoint(bone, 25, Vector3.down, 3, 7));
        StartCoroutine(rotateJoint(bone3, 5, Vector3.up, 2, 9));
        
	}

}