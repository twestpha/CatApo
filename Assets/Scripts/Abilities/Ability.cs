using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu()]
[System.Serializable]
public class Ability : ScriptableObject {
    // Defines an ability; abilities modify the world and the actors in it
    // when the actor who owns the ability casts it

    public bool debug;

    public enum AbilitySource {
        Talent,
        Equipment,
    }

    public enum AbilityState {
        Idle,
        Notified,
        Casted,
    }

    public Actor caster;

    public AbilityState state;

    public string abilityName;

    public AbilitySource source;
    public bool includeSelf;
    // channeled ability?
    // tags to include or exclude

    public float cooldown;
    private float currentCooldown;

    // cast location stuff
    private Vector3 castPosition;

    // Multiple placements
    [Header("Placements")]
    public List<AbilityPlacement> placements;

    [Header("Effects and Timing")]
    public List<AbilityEffect> effects;
    public List<float> effectsTiming;
    private int effectsIndex;

    // [Header("GameObjects and Timing")]
    // public List<GameObject> gameObjects; // TODO make creation of a game object an effect and lump these together
    // public List<float> gameObjectTiming;
    // private int gameObjectsIndex;

    // Default Methods
    public void Start(){
        state = AbilityState.Idle;

        // Instantiate copies of the effects and placement data
        for(int i = 0; i < effects.Count; ++i){
            effects[i] = UnityEngine.Object.Instantiate(effects[i]);
        }

        for(int i = 0; i < placements.Count; ++i){
            placements[i] = UnityEngine.Object.Instantiate(placements[i]);
            placements[i].Start();
            placements[i].SetParent(this);
        }

        // Setting up lists of effects and gameobjects
        effectsIndex = 0;
        gameObjectsIndex = 0;

        if(debug){
            Assert.IsTrue(placements.Count > 0, "Ability \"" + abilityName + "\" is missing an AbilityPlacement.");
            Assert.IsTrue(effects.Count == effectsTiming.Count, "Effects list for \"" + abilityName + "\" needs to match timing list length");
            Assert.IsTrue(gameObjects.Count == gameObjectTiming.Count, "GameObjects list for \"" + abilityName + "\" needs to match timing list length");

            // Check to see if the timings are sorted
            for(int i = 1; i < effectsTiming.Count; ++i){
                Assert.IsTrue(effectsTiming[i] >= effectsTiming[i-1], "Effects Timing is not sorted for ability " + abilityName);
            }

            for(int i = 1; i < gameObjectTiming.Count; ++i){
                Assert.IsTrue(gameObjectTiming[i] >= gameObjectTiming[i-1], "Effects Timing is not sorted for ability " + abilityName);
            }

            // the timings must occur within the cooldown
            if(effectsTiming.Count > 0){
                Assert.IsTrue(effectsTiming[effectsTiming.Count - 1] <= cooldown, "Timings must be less than or equal to the cooldown of the ability " + abilityName);
            }
            if(gameObjectTiming.Count > 0){
                Assert.IsTrue(gameObjectTiming[gameObjectTiming.Count - 1] <= cooldown, "Timings must be less than or equal to the cooldown of the ability " + abilityName);
            }
        }
    }

    public void Update(){
        currentCooldown += Time.deltaTime;

        // If we're notified, wait for right or left click to either cast or cancel
        if(state == AbilityState.Notified){

            if(Input.GetMouseButton(0) && currentCooldown >= cooldown){
                currentCooldown = 0.0f;

                // reset indices
                effectsIndex = 0;
                gameObjectsIndex = 0;

                castPosition = caster.MouseTarget();

                state = AbilityState.Casted;
            } else if(Input.GetMouseButton(1)){
                state = AbilityState.Idle;
            }
        }
        // If we're casted, get all the targets from all placements and cast effects on said targets
        if(state == AbilityState.Casted){
            // iterate through remaining effects and cast on targets at time specified
            for(int i = effectsIndex; i < effects.Count; ++i){
                if(effectsTiming[i] < currentCooldown){
                    foreach(AbilityPlacement placement in placements){
                        Actor[] targets = placement.GetTargetsInCast(castPosition);

                        effectsIndex++;
                        foreach(Actor actor in targets){
                            if(!actor || (!includeSelf && actor == caster)){
                                continue;
                            }

                            effects[i].Apply(actor);
                        }
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

    public void LateUpdate(){
        // Update placements _after_ the caster has moved
        foreach(AbilityPlacement placement in placements){
            placement.Update();
        }
    }

    public void Notify(){
        foreach(AbilityPlacement placement in placements){
            if(placement.type == AbilityPlacement.PlacementType.onHotkey){
                // If ability requires hotkey only
                state = AbilityState.Casted;
            } else if(placement.type == AbilityPlacement.PlacementType.onClick){
                // If ability requires a left click
                state = AbilityState.Notified;
            }
        }
    }

    public void SetCaster(Actor actor){
        caster = actor;

        for(int i = 0; i < placements.Count; ++i){
            placements[i].SetCaster(actor);
        }
    }
}
