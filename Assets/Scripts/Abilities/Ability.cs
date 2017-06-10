using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Ability : ScriptableObject {
    // Ability is the base class for ability scripts to descend from and implement custom behavior

    public Actor selfActor;
    public AbilityCastComponent castComponent;
    public int selfAbilityIndex;

    public bool castable = true;

    public KeyCode hotkey;

    // State tracking and type
    protected enum AbilityState {
        Idle,
        Notified,
        Waiting,
        Casted,
    };

    protected enum AbilityType {
        passive,
        onHotkey,
        onClick,
        vector,
    }

    protected AbilityState state;
    protected AbilityType type;

    // Cast locations
    protected Vector3 castClickDownPosition;
    protected Vector3 castClickUpPosition;

    // Cooldown
    protected float cooldown;
    private Stopwatch cooldownStopwatch;

    // Threads
    System.Threading.Thread alwaysCastThread = null;
    System.Threading.Thread sequentialCastThread = null;

    public void Update(){
        if(type == AbilityType.passive){
            // if passive, cast once and let alwaysCast handle logic
            if(state == AbilityState.Idle){
                state = AbilityState.Casted;
                Cast();
            }
        } else if(type == AbilityType.onHotkey){
            // if hotkey, cast on hotkey pressed
            if(state == AbilityState.Idle && Input.GetKeyDown(hotkey)){
                state = AbilityState.Casted;
                castClickDownPosition = Vector3.zero;
                Cast();
            }
        } else if(type == AbilityType.onClick){
            // if on click, notify on hotkey, cast on click
            if(state == AbilityState.Idle && Input.GetKeyDown(hotkey)){
                state = AbilityState.Notified;
            } else if(state == AbilityState.Notified && Input.GetMouseButtonDown(0)){
                state = AbilityState.Casted;
                castClickDownPosition = Vector3.zero;
                Cast();
            }
        } else if(type == AbilityType.vector){
            // if vector, notify on hotkey, wait on click, cast on unclick
            if(state == AbilityState.Idle && Input.GetKeyDown(hotkey)){
                state = AbilityState.Notified;
            } else if(state == AbilityState.Notified && Input.GetMouseButtonDown(0)){
                state = AbilityState.Waiting;
                castClickDownPosition = Vector3.zero;
            } else if(state == AbilityState.Waiting && Input.GetMouseButtonUp(0)){
                state = AbilityState.Casted;
                castClickUpPosition = Vector3.zero;
                Cast();
            }
        }
    }

    public void Cast(){
        cooldownStopwatch = new Stopwatch();
        cooldownStopwatch.Start();

        alwaysCastThread = new System.Threading.Thread(AlwaysCastStub);
        sequentialCastThread = new System.Threading.Thread(SequentialCast);

        alwaysCastThread.Start();
        sequentialCastThread.Start();
    }

    public void AlwaysCastStub(){
        if(type == AbilityType.passive){
            // we're passive, loop until stopped
            while(state == AbilityState.Casted){
                AlwaysCast();
            }
        } else {
            // If we're not passive, wait for cooldown
            while(cooldownStopwatch.ElapsedMilliseconds <= cooldown && state == AbilityState.Casted){
                AlwaysCast();
            }

            state = AbilityState.Idle;
        }
    }

    // virtual methods that sub-classes will implement
    public virtual void Setup(){}
    protected virtual void AlwaysCast(){}
    protected virtual void SequentialCast(){}

    // shutdown methods kill threads
    public void OnDestroy(){
        End();
    }

    // interface methods for "registering" effects
    // These will be the thread-safe methods of reading and writing actor data

    //##########################################################################
    // Singular Attribute Getters and Setters
    //##########################################################################
    protected void SetHealth(Actor actor, float health){
        actor.currentHealth = health;
    }

    protected float GetHealth(Actor actor){
        return actor.currentHealth;
    }

    protected void SetSpeed(Actor actor, float speed){
        actor.currentMoveSpeed = speed;
    }

    protected float GetSpeed(Actor actor){
        return actor.currentMoveSpeed;
    }

    protected bool GetGrounded(Actor actor){
        return actor.GetMTGrounded();
    }

    protected void SetSteerable(Actor actor, bool steerable){
        actor.steerable = steerable;
    }

    protected bool GetSteerable(Actor actor){
        return actor.steerable;
    }

    //##########################################################################
    // Vector Attribute Getters and Setters
    //##########################################################################
    protected void SetPosition(Actor actor, Vector3 position){
        actor.SetMTPosition(position);
    }

    protected Vector3 GetPosition(Actor actor){
        return actor.GetMTPosition();
    }

    protected void SetTarget(Actor actor, Vector3 position){
        actor.targetPosition = position;
    }

    protected Vector3 GetTarget(Actor actor){
        return actor.targetPosition;
    }

    protected void SetVelocity(Actor actor, Vector3 velocity){
        actor.velocity = velocity;
    }

    protected Vector3 GetVelocity(Actor actor){
        return actor.velocity;
    }

    //##########################################################################
    // Scripting Utility Functions
    //##########################################################################
    protected void Delay(float duration){
        System.Threading.Thread.Sleep((int)duration);
    }

    protected void End(){
        state = AbilityState.Idle;

        if(alwaysCastThread != null && alwaysCastThread.IsAlive){
            alwaysCastThread.Abort();
        }

        if(sequentialCastThread != null && sequentialCastThread.IsAlive){
            sequentialCastThread.Abort();
        }
    }

    //##########################################################################
    // Actor Utility Functions
    //##########################################################################
    protected Actor ActorFromName(string name){
        return GameObject.Find(name).GetComponent<Actor>();
    }
}
