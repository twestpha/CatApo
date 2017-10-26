using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationPoseRequest  {
    public AnimationPose pose;
    public AnimationComponent.CurveType curve;
    public float duration;
    public bool immediate;
    public bool requeueWhenFinished;

    public AnimationPoseRequest(AnimationPose pose_, AnimationComponent.CurveType curve_, float duration_, bool immediate_, bool requeueWhenFinished_){
        pose = pose_;
        curve = curve_;
        duration = duration_;
        immediate = immediate_;
        requeueWhenFinished = requeueWhenFinished_;
    }
}
