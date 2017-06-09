using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Ability : ScriptableObject {
    // Ability is the base class for ability scripts to descend from and implement custom behavior

    public Actor selfActor;
    public AbilityCastComponent castComponent;

    // Handling cooldown
    protected float cooldown;
    private Stopwatch cooldownStopwatch;

    System.Threading.Thread alwaysCastThread = null;
    System.Threading.Thread SequentialCastThread = null;

    public void Cast(){
        Setup();

        cooldownStopwatch = new Stopwatch();
        cooldownStopwatch.Start();

        alwaysCastThread = new System.Threading.Thread(AlwaysCastStub);
        alwaysCastThread.Start();

        SequentialCastThread = new System.Threading.Thread(SequentialCast);
        SequentialCastThread.Start();
    }

    public void AlwaysCastStub(){
        while(cooldownStopwatch.ElapsedMilliseconds <= cooldown){
            AlwaysCast();
        }
    }

    // virtual methods that sub-classes will implement
    protected virtual void Setup(){}
    protected virtual void AlwaysCast(){}
    protected virtual void SequentialCast(){}

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
    protected void Delay(int duration){
        System.Threading.Thread.Sleep(duration);
    }

    //##########################################################################
    // Actor Utility Functions
    //##########################################################################
    protected Actor ActorFromName(string name){
        return GameObject.Find(name).GetComponent<Actor>();
    }
}
