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

    private enum MouseButton {
        Right,
        Left,
        Middle,
    }


    // State tracking and type
    public enum AbilityState {
        Idle,
        Notified,
        Waiting,
        Casted,
    };

    public enum AbilityType {
        passive,
        onHotkey,
        onClick,
        vector,
    }

    public AbilityState state;
    public AbilityType type;

    // Placements
    protected List<string> placementNames;
    private List<AbilityPlacement> placements;

    // Cast locations
    public Vector3 castClickDownPosition;
    public Vector3 castClickUpPosition;

    // Cooldown
    protected float cooldown;
    private Stopwatch cooldownStopwatch;

    // Threads
    System.Threading.Thread alwaysCastThread = null;
    System.Threading.Thread sequentialCastThread = null;

    public void SetupAbility(){
        placementNames = new List<string>();

        Setup();

        if(UsePlacement()){
            placements = new List<AbilityPlacement>();

            for(int i = 0; i < placementNames.Count; ++i){
                string assetName = "Abilities/" + placementNames[i];
                AbilityPlacement placement = Resources.Load(assetName, typeof(AbilityPlacement)) as AbilityPlacement;
                placement = ScriptableObject.Instantiate(placement);
                placement.SetCaster(selfActor);
                placement.SetAbility(this);
                placement.Start();

                placements.Add(placement);
            }
        }
    }

    public void Update(){
        if(UsePlacement()){
            for(int i = 0; i < placements.Count; ++i){
                placements[i].Update();
            }
        }

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
                castClickDownPosition = selfActor.AbilityTargetPoint();
                Cast();
            }
        } else if(type == AbilityType.onClick){
            // if on click, notify on hotkey, cast on click
            if(state == AbilityState.Idle && Input.GetKeyDown(hotkey)){
                state = AbilityState.Notified;
                SetPlacementEnabled(true);
            } else if(state == AbilityState.Notified && Input.GetMouseButtonDown((int)MouseButton.Left)){
                state = AbilityState.Idle;
                SetPlacementEnabled(false);
            } else if(state == AbilityState.Notified && Input.GetMouseButtonDown((int)MouseButton.Right)){
                state = AbilityState.Casted;
                SetPlacementEnabled(false);
                castClickDownPosition = selfActor.AbilityTargetPoint();
                Cast();
            }
        } else if(type == AbilityType.vector){
            // if vector, notify on hotkey, wait on click, cast on unclick
            if(state == AbilityState.Idle && Input.GetKeyDown(hotkey)){
                state = AbilityState.Notified;
            } else if(state == AbilityState.Notified && Input.GetMouseButtonDown((int)MouseButton.Right)){
                state = AbilityState.Waiting;
                SetPlacementEnabled(true);
                castClickDownPosition = selfActor.AbilityTargetPoint();
            } else if(state == AbilityState.Notified && Input.GetMouseButtonDown((int)MouseButton.Left)){
                state = AbilityState.Idle;
                SetPlacementEnabled(false);
            } else if(state == AbilityState.Waiting && Input.GetMouseButtonDown((int)MouseButton.Left)){
                state = AbilityState.Idle;
                SetPlacementEnabled(false);
            } else if(state == AbilityState.Waiting && Input.GetMouseButtonUp((int)MouseButton.Right)){
                state = AbilityState.Casted;
                SetPlacementEnabled(false);
                castClickUpPosition = selfActor.AbilityTargetPoint();
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
    protected virtual void Setup(){}
    protected virtual void AlwaysCast(){}
    protected virtual void SequentialCast(){}

    // shutdown methods kill threads
    public void OnDestroy(){
        End();
    }

    private bool UsePlacement(){
        return type == AbilityType.onClick || type == AbilityType.vector;
    }

    private void SetPlacementEnabled(bool enabled){
        for(int i = 0; i < placements.Count; ++i){
            placements[i].SetEnabled(enabled);
        }
    }

    public float GetCooldownPercentage(){
        if(cooldownStopwatch != null){
            return Mathf.Min(Mathf.Max(cooldownStopwatch.ElapsedMilliseconds / cooldown, 0.0f), 1.0f);
        }
        return 1.0f;
    }

    // interface methods for "registering" effects
    // These will be the thread-safe methods of reading and writing actor data

    //##########################################################################
    // Singular Attribute Getters and Setters
    //##########################################################################
    protected void SetHealth(Actor actor, int health){
        actor.currentHealth = health;
    }

    protected int GetHealth(Actor actor){
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
