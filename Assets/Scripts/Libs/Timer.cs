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

    public float Parameterized(){
        return Mathf.Max(Mathf.Min(Elapsed() / duration, 1.0f), 0.0f);
    }

    public bool Finished(){
        return Elapsed() >= duration;
    }
};
