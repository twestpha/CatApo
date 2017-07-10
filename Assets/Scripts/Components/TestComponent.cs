using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestComponent : MonoBehaviour {

    public GameObject player;

    public enum OccludingState {
        NotOccluding,
        StartedOccluding,
        Occluding,
    }

    public OccludingState state;

    public float fadeDuration;
    private Timer fadeOutTimer;
    private Timer fadeInTimer;

    private Renderer rend;

	void Start(){
        state = OccludingState.NotOccluding;
        //Debug.LogError("DO NOT USE THIS SCRIPT FOR ANYTHING EXCEPT TESTING.");
        rend = GetComponent<Renderer>();
        fadeOutTimer = new Timer(fadeDuration);
        fadeInTimer = new Timer(fadeDuration);
	}

	void Update(){
        if(state == OccludingState.StartedOccluding){
            fadeOutTimer.Start();
            state = OccludingState.Occluding;
        }

        if(state == OccludingState.Occluding){
            // check current occlusion
            Vector3 direction = player.transform.position - Camera.main.transform.position;
            RaycastHit hit;
            Debug.DrawRay(Camera.main.transform.position, direction);

            if(Physics.Raycast(Camera.main.transform.position, direction, out hit, direction.magnitude, 1 << 12)){
                if(hit.collider.GetComponent<TestComponent>() != this){
                    state = OccludingState.NotOccluding;;
                    fadeInTimer.Start();
                }
            } else {
                state = OccludingState.NotOccluding;
                fadeInTimer.Start();
            }

            Color newColor = rend.material.color;
            newColor.a = 1.0f - fadeOutTimer.Parameterized();
            rend.material.color = newColor;
        }

        if(state == OccludingState.NotOccluding){
            Color newColor = rend.material.color;
            newColor.a = fadeInTimer.Parameterized();
            rend.material.color = newColor;
        }
	}

    public void EnableNonOcclusion(){
        if(state == OccludingState.NotOccluding){
            state = OccludingState.StartedOccluding;
        }
    }

}
