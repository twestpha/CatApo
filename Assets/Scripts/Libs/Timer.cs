using UnityEngine;

public class Timer {
    private bool started;
    private float duration;
    private float startTime;

    public Timer(){
        duration = -1.0f;
    }

    public Timer(float duration_){
        duration = duration_;
    }

    public void Start(){
        startTime = Time.time;
    }

    public float Elapsed(){
        return Time.time - startTime;
    }

    public bool Finished(){
        return Elapsed() >= duration;
    }
};
