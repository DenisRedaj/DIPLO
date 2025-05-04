using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointConstraints : MonoBehaviour
{
 

    private Dictionary<string, Vector3> axis = new Dictionary<string, Vector3>();
    void Start()
    {
        axis.Add("Base_BONE",                   new Vector3(0, 1, 0));
        axis.Add("Torso_BONE",                  new Vector3(0, 1, 0));
        axis.Add("virt_BONE",                    new Vector3(1, 0, 0));

        axis.Add("Neck_BONE_2",                 new Vector3(0, 1, 0));
        axis.Add("Neck_BONE",                   new Vector3(0, 1, 0));
        axis.Add("Neck_BONE_end",               new Vector3(0, 1, 0));

        // LEFT ARMa
        axis.Add("LEFT_Shoulder_BONE",          new Vector3(1, 0, 0));
        axis.Add("LEFT_Upper_Arm_BONE",         new Vector3(0, -1, 0));  
        axis.Add("LEFT_Lower_Arm_BONE",         new Vector3(0, -1, 0));
        axis.Add("LEFT_Hand_BONE",                  new Vector3(0, -1, 0));
        axis.Add("LEFT_Palm_BONE",                  new Vector3(0, 1, 0));
        axis.Add("LEFT_Palm_BONE_end",                   new Vector3(0, 1, 0));

    
       
        axis.Add("LEFT_Finger3_3_BONE",                   new Vector3(0, 1, 0));
        axis.Add("LEFT_Finger3_2_BONE",                   new Vector3(0, 1, 0));
        axis.Add("LEFT_Finger3_1_BONE",                   new Vector3(0, 1, 0));
        axis.Add("LEFT_Finger3_1_BONE_end",               new Vector3(0, 1, 0));  

        axis.Add("LEFT_Finger2_3_BONE",                   new Vector3(0, 1, 0));
        axis.Add("LEFT_Finger2_2_BONE",                   new Vector3(0, 1, 0));
        axis.Add("LEFT_Finger2_1_BONE",                   new Vector3(0, 1, 0));
        axis.Add("LEFT_Finger2_1_BONE_end",                  new Vector3(0, 1, 0));  

        axis.Add("LEFT_Thumb_3_BONE",                   new Vector3(0, 1, 0));
        axis.Add("LEFT_Thumb_2_BONE",                   new Vector3(0, 1, 0));
        axis.Add("LEFT_Thumb_1_BONE",                   new Vector3(0, 1, 0));
        axis.Add("LEFT_Thumb_1_BONE_end",                  new Vector3(0, 1, 0));


       
        // RIGHT ARM
        axis.Add("RIGHT_Thumb_3_BONE",                  new Vector3(0, 0, 1));
        axis.Add("RIGHT_Thumb_2_BONE",                  new Vector3(0, 0, 1));
        axis.Add("RIGHT_Thumb_1_BONE",                  new Vector3(0, 0, 1));  
        axis.Add("RIGHT_Thumb_1_BONE_end",              new Vector3(0, 0, 1));

        axis.Add("RIGHT_Finger2_3_BONE",                new Vector3(0, 0, 1));
        axis.Add("RIGHT_Finger2_2_BONE",                new Vector3(0, 0, 1));
        axis.Add("RIGHT_Finger2_1_BONE",                new Vector3(0, 0, 1));      
        axis.Add("RIGHT_Finger2_1_BONE_end",            new Vector3(0, 0, 1));

        axis.Add("RIGHT_Finger3_3_BONE",                new Vector3(0, 0, 1));
        axis.Add("RIGHT_Finger3_2_BONE",                new Vector3(0, 0, 1));
        axis.Add("RIGHT_Finger3_1_BONE",                new Vector3(0, 0, 1));
        axis.Add("RIGHT_Finger3_1_BONE_end",            new Vector3(0, 0, 1));  

        axis.Add("RIGHT_Shoulder_BONE",                 new Vector3(1, 0, 0));
        axis.Add("RIGHT_Upper_Arm_BONE",                new Vector3(0, 0, 1));
        axis.Add("RIGHT_Lower_Arm_BONE",                new Vector3(-1, 0, 0));
        axis.Add("RIGHT_Hand_BONE",                     new Vector3(-1, 0, 0));
        axis.Add("RIGHT_Palm_BONE",                     new Vector3(0, 1, 0));
        axis.Add("RIGHT_Palm_BONE_end",                 new Vector3(0, 1, 0));

    }

    public Dictionary<string, Vector3> getJointConstraints(){
        return axis;
    }
}
