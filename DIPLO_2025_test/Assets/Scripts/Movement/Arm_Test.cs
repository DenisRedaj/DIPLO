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
    void Start(){}

    IEnumerator rotateJoint(GameObject bone, float angle, Vector3 axis, float time, float delay=0){

        yield return new WaitForSeconds(delay);

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

    public void clickTest(){
        Debug.Log ("button clicked" + bone.name + "transform: "+ bone.transform.rotation);
        StartCoroutine(rotateJoint(bone, 45, Vector3.up, 3));
        StartCoroutine(rotateJoint(bone2, 45, Vector3.up, 3, 4));
	}

}