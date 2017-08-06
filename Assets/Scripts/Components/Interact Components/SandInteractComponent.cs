using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandInteractComponent : Interactable {

    public float dropDistance;
    public float dropDuration;

    private Timer dropTimer;

    private bool dropping;
    private float previousHeight;

    public void Start(){
        dropTimer = new Timer(dropDuration);
    }

    public void Update(){
        if(dropping){

            transform.position = new Vector3(transform.position.x, previousHeight - dropTimer.Parameterized() * dropDistance, transform.position.z);

            if(dropTimer.Finished()){
                dropping = false;
            }
        }
    }

    override public void NotifyClicked(){
        dropping = true;
        previousHeight = transform.position.y;
        dropTimer.Start();
    }
}
