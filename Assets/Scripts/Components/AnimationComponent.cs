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
    public AnimationPose testPose;

    private Queue<AnimationPoseRequest> requests;
    private AnimationPoseRequest currentRequest;
    private AnimationPose previousPose;

    public void Start(){
        animationTimer = new Timer(0.0f);
        animationTimer.Start();

        previousPose = initialPose;
        currentRequest = new AnimationPoseRequest(previousPose, CurveType.Linear, 0.0f, false, false);

        requests = new Queue<AnimationPoseRequest>();

        RequestAnimation(new AnimationPoseRequest(testPose, CurveType.Spring, 5.0f, false, false));
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

        for(int i = 0; i < bones.Length; ++i){
            bones[i].transform.rotation = Quaternion.SlerpUnclamped(previousPose.joints[i], currentRequest.pose.joints[i], tvalue);
        }

    }

    public void RequestAnimation(AnimationPoseRequest request){
        if(request.immediate){
            requests.Clear();
        }

        requests.Enqueue(request);
    }
}
