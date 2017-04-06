using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu()]
[System.Serializable]
public class Ability : ScriptableObject {

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

    public List<AbilityEffect> effects;

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

        // Run start on necessary parts
        placement.Start();
        placement.SetParent(this);
    }

    public void Update(){
        currentCooldown += Time.deltaTime;

        // If we're notified, wait for right or left click to either cast or cancel
        if(state == AbilityState.Notified){
            if(Input.GetMouseButtonDown(0)){
                Cast();
            } else if(Input.GetMouseButtonDown(1)){
                state = AbilityState.Idle;
            }
        }

        placement.Update();
    }

    public void Notify(){
        if(placement.type > AbilityPlacement.PlacementType.ONHOTKEY &&
           placement.type < AbilityPlacement.PlacementType.ONCLICK){
            // If ability requires hotkey only
            Cast();
        } else if(placement.type > AbilityPlacement.PlacementType.ONCLICK){
            // If ability requires a left click
            state = AbilityState.Notified;
        }
    }

    public void Cast(){
        if(currentCooldown > cooldown){
            Actor[] targets = placement.GetTargetsInCast();

            currentCooldown = 0.0f;

            foreach(Actor actor in targets){
                foreach(AbilityEffect effect in effects){
                    effect.Apply(actor);
                }
            }

            state = AbilityState.Casted;
        }
    }

    public void SetSelfActor(Actor actor){
        selfActor = actor;
    }
}
