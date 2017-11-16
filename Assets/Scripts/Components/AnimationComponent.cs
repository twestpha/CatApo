using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationComponent : MonoBehaviour {
    public GameObject[] bones;
    private Timer animationTimer;

    public enum CurveType {
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut,
        Spring,
    }

    public AnimationPose initialPose;

    private Queue<AnimationPoseRequest> requests;
    private AnimationPoseRequest currentRequest;
    private AnimationPose previousPose;

    public void Start(){
        animationTimer = new Timer(0.0f);
        animationTimer.Start();

        requests = new Queue<AnimationPoseRequest>();

        previousPose = ScriptableObject.Instantiate(initialPose);
        currentRequest = new AnimationPoseRequest(previousPose, CurveType.Linear, 0.0f, false, false);
    }

    public void Update(){
        if(animationTimer.Finished() && requests.Count > 0){
            if(currentRequest.requeueWhenFinished){
                RequestAnimation(currentRequest);
            }

            if(currentRequest.pose){
                previousPose = currentRequest.pose;
            }

            currentRequest = requests.Dequeue();

            animationTimer.SetDuration(currentRequest.duration);
            animationTimer.Start();
        }

        float tvalue = animationTimer.Parameterized();

        if(tvalue > 1.0f){
            return;
        }

        switch(currentRequest.curve){
        case CurveType.EaseIn:
            tvalue = Mathfx.Sinerp(0.0f, 1.0f, tvalue);
            break;
        case CurveType.EaseOut:
            tvalue = Mathfx.Coserp(0.0f, 1.0f, tvalue);
            break;
        case CurveType.EaseInOut:
            tvalue = Mathfx.Hermite(0.0f, 1.0f, tvalue);
            break;
        case CurveType.Spring:
            tvalue = Mathfx.Berp(0.0f, 1.0f, tvalue); // could use some work
            break;
        }

        bones[0].transform.localPosition = Vector3.Lerp(previousPose.zerojoint, currentRequest.pose.zerojoint, tvalue);
        for(int i = 0; i < bones.Length; ++i){
            bones[i].transform.localRotation = Quaternion.SlerpUnclamped(previousPose.joints[i], currentRequest.pose.joints[i], tvalue);
        }
    }

    public void RequestAnimation(AnimationPose pose, CurveType curvetype, float duration, bool immediate, bool requeueWhenFinished){
        RequestAnimation(new AnimationPoseRequest(ScriptableObject.Instantiate(pose), curvetype, duration, immediate, requeueWhenFinished));
    }

    private void RequestAnimation(AnimationPoseRequest request){
        if(request.immediate){
            requests.Clear();

            // finish old pose
            animationTimer.SetDuration(0.0f);
            animationTimer.Start();

            // cache pose where previous animation paused
            currentRequest.pose.zerojoint = bones[0].transform.localPosition;
            for(int i = 0; i < bones.Length; ++i){
                currentRequest.pose.joints[i] = bones[i].transform.localRotation;
            }

            // old animation can't requeue
            currentRequest.requeueWhenFinished = false;

            // dont want it to requeue as immediate
            request.immediate = false;
        }

        requests.Enqueue(request);
    }
}
