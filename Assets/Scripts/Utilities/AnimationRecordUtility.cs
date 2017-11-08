using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRecordUtility : MonoBehaviour {
    public bool record;
    public bool load;
    public bool mirror;

    public AnimationPose recordPose;

    public GameObject[] bones;

    public int[] mirrorIndices;

    public void Start(){
        record = false;
        load = false;
    }

    public void Update(){
        if(record){
            for(int i = 0; i < bones.Length; ++i){
                recordPose.joints[i] = bones[i].transform.rotation;
            }
            record = false;
        }

        if(load){
            for(int i = 0; i < bones.Length; ++i){
                bones[i].transform.rotation = recordPose.joints[i];
            }
            load = false;
        }

        if(mirror){
            Quaternion[] newBonesrotation = new Quaternion[bones.Length];

            for(int i = 0; i < bones.Length; ++i){
                Quaternion oldrot = bones[mirrorIndices[i]].transform.rotation;

                oldrot.w *= -1.0f;
                oldrot.y *= -1.0f;

                newBonesrotation[i] = oldrot;
            }

            for(int i = 0; i < bones.Length; ++i){
                bones[i].transform.rotation = newBonesrotation[i];
            }

            mirror = false;
        }
    }
}
