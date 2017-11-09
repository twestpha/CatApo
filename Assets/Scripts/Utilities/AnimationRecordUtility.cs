using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationRecordUtility : EditorWindow {
    [MenuItem ("Window/My Window")]

    public static void  ShowWindow () {
        EditorWindow.GetWindow(typeof(AnimationRecordUtility));
    }

    void OnGUI() {
        if (GUI.Button(new Rect(10, 70, 50, 30), "Click"))
            Debug.Log("Clicked the button with text");
    }
}

// public class AnimationRecordUtility : MonoBehaviour {
//     public bool record;
//     public bool load;
//     public bool mirror;
//
//     public AnimationPose recordPose;
//
//     public GameObject[] bones;
//
//     public int[] mirrorIndices;
//
//     public void Start(){
//         record = false;
//         load = false;
//         mirror = false;
//
//         recordPose = null;
//     }
//
//     public void Update(){
//         if(record){
//             Debug.Log("SAVING POSE");
//             for(int i = 0; i < bones.Length; ++i){ // something's fucked up with this...
//                 recordPose.joints[i] = bones[i].transform.localRotation;
//             }
//
//             SetDirty(recordPose);
//             recordPose = null;
//
//             record = false;
//         }
//
//         if(load){
//             Debug.Log("LOADING POSE");
//
//             AnimationPose newPose = ScriptableObject.Instantiate(recordPose);
//
//             for(int i = 0; i < bones.Length; ++i){
//                 bones[i].transform.localRotation = newPose.joints[i];
//             }
//             load = false;
//         }
//
//         // if(mirror){
//         //     Debug.Log("MIRRORING POSE");
//         //     Quaternion[] newBonesrotation = new Quaternion[bones.Length];
//         //
//         //     for(int i = 0; i < bones.Length; ++i){
//         //         Quaternion oldrot = bones[mirrorIndices[i]].transform.localRotation;
//         //
//         //         oldrot.w *= -1.0f;
//         //         oldrot.y *= -1.0f;
//         //
//         //         newBonesrotation[i] = oldrot;
//         //     }
//         //
//         //     for(int i = 0; i < bones.Length; ++i){
//         //         bones[i].transform.localRotation = newBonesrotation[i];
//         //     }
//         //
//         //     mirror = false;
//         // }
//     }
// }
