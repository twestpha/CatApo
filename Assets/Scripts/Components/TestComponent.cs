using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestComponent : MonoBehaviour {

    public GameObject player;

    public bool occludingPlayer;

    public float fadeDuration;
    private Timer fadeOutTimer;
    private Timer fadeInTimer;

    private Renderer rend;

	void Start(){
        //Debug.LogError("DO NOT USE THIS SCRIPT FOR ANYTHING EXCEPT TESTING.");
        rend = GetComponent<Renderer>();
        fadeOutTimer = new Timer(fadeDuration);
        fadeInTimer = new Timer(fadeDuration);
	}

	void Update(){
        if(occludingPlayer){
            // check occlusion
            Vector3 direction = player.transform.position - Camera.main.transform.position;
            RaycastHit hit;
            Debug.DrawRay(Camera.main.transform.position, direction);
            if(!Physics.Raycast(Camera.main.transform.position, direction, out hit, direction.magnitude, 1 << 12)){
                occludingPlayer = false;
                fadeInTimer.Start();
            } else {
                if(hit.collider.GetComponent<TestComponent>() != this){
                    occludingPlayer = false;
                    fadeInTimer.Start();
                }
            }

            Color newColor = rend.material.color;
            newColor.a = 1.0f - fadeOutTimer.Parameterized();
            rend.material.color = newColor;
        } else {
            Color newColor = rend.material.color;
            newColor.a = fadeInTimer.Parameterized();
            rend.material.color = newColor;
        }
	}

    public void EnableNonOcclusion(){
        if(!occludingPlayer){
            occludingPlayer = true;
            fadeOutTimer.Start();
        }
    }

}
