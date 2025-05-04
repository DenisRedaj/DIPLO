using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBox : MonoBehaviour
{


    private Vector3 offset;

    private float coord;


    void OnMouseDown(){
        coord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        offset = gameObject.transform.position-MousePos();
    }

    private Vector3 MousePos(){
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = coord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag(){
        
        transform.position = MousePos() + offset;
    }
}
