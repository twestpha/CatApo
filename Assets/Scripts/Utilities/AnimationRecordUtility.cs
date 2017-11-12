using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
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
        mirror = false;

        recordPose = null;
    }

    public void Update(){
        if(record){
            Debug.Log("SAVING POSE");
            for(int i = 0; i < bones.Length; ++i){
                if(i == 0){
                    Debug.Log(bones[i].transform.localRotation);
                }
                recordPose.joints[i] = bones[i].transform.localRotation;
            }

            EditorUtility.SetDirty(recordPose);
            AssetDatabase.SaveAssets();

            record = false;
        }

        if(load){
            Debug.Log("LOADING POSE");

            for(int i = 0; i < bones.Length; ++i){
                bones[i].transform.localRotation = recordPose.joints[i];
            }
            load = false;
        }

        // if(mirror){
        //     Debug.Log("MIRRORING POSE");
        //     Quaternion[] newBonesrotation = new Quaternion[bones.Length];
        //
        //     for(int i = 0; i < bones.Length; ++i){
        //         Quaternion oldrot = bones[mirrorIndices[i]].transform.rotation;
        //
        //         oldrot.w *= -1.0f;
        //         oldrot.y *= -1.0f;
        //
        //         newBonesrotation[i] = oldrot;
        //     }
        //
        //     for(int i = 0; i < bones.Length; ++i){
        //         bones[i].transform.rotation = newBonesrotation[i];
        //     }
        //
        //     mirror = false;
        // }
    }
}
