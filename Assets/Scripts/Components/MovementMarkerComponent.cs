using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementMarkerComponent : MonoBehaviour {

    private float showTimerDuration = 0.2f;
    private Timer showTimer;

    private bool showing;
    private float originalScale;

    private PlayerComponent player;

	void Start(){
        player = GameObject.FindWithTag("Player").GetComponent<PlayerComponent>();

        showTimer = new Timer(showTimerDuration);
        originalScale = transform.localScale.x;
        showing = false;
	}

	void Update(){
        if(showing){
            transform.position = player.targetPosition;

            float news = showTimer.Parameterized() * originalScale;
            transform.localScale = new Vector3(news, news, news);;

            if(showTimer.Finished() && !Input.GetButton("Fire2")){
                showing = false;
                showTimer.Start();
            }
        } else {
            float news = (1.0f - showTimer.Parameterized()) * originalScale;
            transform.localScale = new Vector3(news, news, news);;
        }
	}

    public void ShowMovementMarker(){
        if(!showing){
            showTimer.Start();
            showing = true;
        }
    }
}
