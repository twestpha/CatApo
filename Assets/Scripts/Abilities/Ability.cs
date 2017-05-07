using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu()]
[System.Serializable]
public class Ability : ScriptableObject {
    // Defines an ability; abilities modify the world and the actors in it
    // when the actor who owns the ability casts it

    public enum AbilitySource {
        Talent,
        Equipment,
    }

    public enum AbilityState {
        Idle,
        Notified,
        Casted,
    }

    public Actor selfActor;

    public AbilityState state;

    public string abilityName;

    public AbilitySource source;
    public AbilityPlacement placement;

    public float cooldown;
    private float currentCooldown;

    // cast location stuff
    private Vector3 castPosition;

    [Header("Effects and Timing")]
    public List<AbilityEffect> effects;
    public List<float> effectsTiming;
    private int effectsIndex;

    [Header("GameObjects and Timing")]
    public List<GameObject> gameObjects;
    public List<float> gameObjectTiming;
    private int gameObjectsIndex;

    // Default Methods
    public void Start(){
        state = AbilityState.Idle;

        // Instantiate copies of the effects and placement data
        for(int i = 0; i < effects.Count; ++i){
            effects[i] = UnityEngine.Object.Instantiate(effects[i]);
        }

        placement = UnityEngine.Object.Instantiate(placement);

        // Assert on the data
        Assert.IsTrue(placement, "Ability \"" + abilityName + "\" is missing an AbilityPlacement.");
        Assert.IsTrue(effects.Count == effectsTiming.Count, "Effects list for \"" + abilityName + "\" needs to match timing list length");
        Assert.IsTrue(gameObjects.Count == gameObjectTiming.Count, "GameObjects list for \"" + abilityName + "\" needs to match timing list length");

        // Run start on necessary parts
        placement.Start();
        placement.SetParent(this);

        // Setting up lists of effects and gameobjects
        effectsIndex = 0;
        gameObjectsIndex = 0;

        // probably should at least assert to see if they're sorted...
        // also assert if the last element in timing's is less than the cooldown... that would cause bugs
    }

    public void Update(){
        currentCooldown += Time.deltaTime;

        placement.Update();

        // If we're notified, wait for right or left click to either cast or cancel
        if(state == AbilityState.Notified){

            if(Input.GetMouseButtonDown(0) && currentCooldown > cooldown){
                currentCooldown = 0.0f;

                // reset indices
                effectsIndex = 0;
                gameObjectsIndex = 0;

                castPosition = placement.splat.transform.position;

                state = AbilityState.Casted;
            } else if(Input.GetMouseButtonDown(1)){
                state = AbilityState.Idle;
            }
        }
        // If we're casted, iterate up our effects/gameobjects list
        else if(state == AbilityState.Casted){
            // iterate through remaining effects and cast on targets at time specified
            for(int i = effectsIndex; i < effects.Count; ++i){
                if(effectsTiming[i] < currentCooldown){
                    Actor[] targets = placement.GetTargetsInCast(castPosition);

                    effectsIndex++;
                    foreach(Actor actor in targets){
                        if(!actor){
                            continue;
                        }

                        effects[i].Apply(actor);
                    }
                }
            }
            // iterate through remaining gameobject and create at time specified
            // use castPosition for creation

            // If we've done all the effects and created all the gameobjects, we're done
            if(effectsIndex == effects.Count && gameObjectsIndex == gameObjects.Count){
                state = AbilityState.Idle;
            }
        }
    }

    public void Notify(){
        if(placement.type > AbilityPlacement.PlacementType.ONHOTKEY &&
           placement.type < AbilityPlacement.PlacementType.ONCLICK){
            // If ability requires hotkey only
            state = AbilityState.Casted;
        } else if(placement.type > AbilityPlacement.PlacementType.ONCLICK){
            // If ability requires a left click
            state = AbilityState.Notified;
        }
    }

    public void SetSelfActor(Actor actor){
        selfActor = actor;
    }
}
