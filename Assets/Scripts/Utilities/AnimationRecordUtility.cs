using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRecordUtility : MonoBehaviour {
    public bool record;

    public AnimationPose recordPose;

    public GameObject[] bones;

    public void Start(){
        record = false;
    }

    public void Update(){
        if(record){
            for(int i = 0; i < bones.Length; ++i){
                recordPose.joints[i] = bones[i].transform.rotation;
            }
            record = false;
        }
    }
}
