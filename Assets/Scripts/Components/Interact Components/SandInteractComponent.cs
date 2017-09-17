using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandInteractComponent : Interactable {

    public float dropDistance;
    public float dropDuration;

    private Timer dropTimer;

    private bool dropping;
    private float previousHeight;

    private AudioSource audioSource;

    public void Start(){
        dropTimer = new Timer(dropDuration);
        audioSource = GetComponent<AudioSource>();
    }

    public void Update(){
        if(dropping){
            transform.position = new Vector3(transform.position.x, previousHeight - dropTimer.Parameterized() * dropDistance, transform.position.z);
            audioSource.volume = 1.0f - Mathf.Pow((dropTimer.Parameterized()), 3.0f);

            if(dropTimer.Finished()){
                dropping = false;
                audioSource.Stop();
            }

        }
    }

    override public void NotifyClicked(){
        if(!enabled){
            return;
        }

        dropping = true;
        previousHeight = transform.position.y;
        dropTimer.Start();
        audioSource.Play();
    }
}
