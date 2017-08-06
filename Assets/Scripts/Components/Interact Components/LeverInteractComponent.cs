using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverInteractComponent : Interactable {

    public bool lockOnWhenClicked;
    public GameObject interactToNotify;
    public GameObject leverModelObject;

    private enum LeverState{
        On,
        Off,
        MovingOn,
        MovingOff,
    };

    private LeverState state;

    private Timer leverTimer;

    private float rotationAmount = 60.0f;
    private float startRotation;

    public void Start(){
        state = LeverState.Off;
        leverTimer = new Timer(0.3f);
        startRotation = leverModelObject.transform.eulerAngles.x;
    }

    public void Update(){
        if(state == LeverState.MovingOn){

            leverModelObject.transform.rotation = Quaternion.Euler(startRotation - (leverTimer.Parameterized() * rotationAmount), 90.0f, 0.0f);

            if(leverTimer.Finished()){
                state = LeverState.On;
            }
        } else if(state == LeverState.MovingOff){

            leverModelObject.transform.rotation = Quaternion.Euler(-startRotation + (leverTimer.Parameterized() * rotationAmount), 90.0f, 0.0f);

            if(leverTimer.Finished()){
                state = LeverState.Off;
            }
        }
    }

    override public void NotifyClicked(){
        if((state == LeverState.On && !lockOnWhenClicked) || state == LeverState.Off){
            Interactable interact = interactToNotify.GetComponent<Interactable>();
            if(interact){
                interact.NotifyClicked();
            }

            if(state == LeverState.On){
                state = LeverState.MovingOff;
            } else {
                state = LeverState.MovingOn;
            }

            leverTimer.Start();
        }
    }
}
