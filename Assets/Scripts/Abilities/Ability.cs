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

    private Actor caster;

    public AbilityState state;
    private AbilityState previousState;

    public string abilityName;

    public AbilitySource source;
    public bool includeSelf;
    // tags to include or exclude
    // maybe a filter class
    // include or exclude
    // self, friendly, enemy, types of enemies?
    // then an override to always add self to target list (and deprecate that from placement)

    // channeled ability? Ends after duration, then cooldown starts. Maybe we want an "until" ability effect with a timer

    public float cooldown;
    private Timer abilityTimer;

    // cast location stuff
    private Vector3 castPosition;

    // TODO rework this to make more sense...
    [Header("Equipment")]
    public List<Equipment> equipments;
    private int equipmentEffectsIndex;

    [Header("Placements")]
    public List<AbilityPlacement> placements;

    // Group this as a structure of these thing? Can't access it...
    // could make AbilityAnimation as a collection of these things
    [Header("Effects and Timing")]
    // add effects list of "always" effects
    public List<AbilityEffect> effects;
    public List<float> effectsTiming;
    // list for effects and always effects, referencing which placement to use for effect
    private int effectsIndex;

    // Default Methods
    public void Start(){
        state = AbilityState.Idle;

        abilityTimer = new Timer(cooldown);

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

        if(debug){
            Assert.IsTrue(placements.Count > 0, "Ability \"" + abilityName + "\" is missing an AbilityPlacement.");
            Assert.IsTrue(effects.Count == effectsTiming.Count, "Effects list for \"" + abilityName + "\" needs to match timing list length");

            // Check to see if the timings are sorted
            for(int i = 1; i < effectsTiming.Count; ++i){
                Assert.IsTrue(effectsTiming[i] >= effectsTiming[i-1], "Effects Timing is not sorted for ability " + abilityName);
            }

            // the timings must occur within the cooldown
            if(effectsTiming.Count > 0){
                Assert.IsTrue(effectsTiming[effectsTiming.Count - 1] <= cooldown, "Timings must be less than or equal to the cooldown of the ability " + abilityName);
            }
        }
    }

    public void Update(){
        // If we're notified, wait for right or left click to either cast or cancel
        if(state == AbilityState.Notified){
            if(Input.GetMouseButton(0)){
                state = AbilityState.Casted;
            } else if(Input.GetMouseButton(1)){
                state = AbilityState.Idle;
            }
        }
        // If we're casted, get all the targets from all placements and cast effects on said targets
        if(state == AbilityState.Casted){
            // Casted last frame
            if(previousState != AbilityState.Casted){
                // Start the timer
                abilityTimer.Start();

                // TODO Get casting position and snap to target over set amount of time, then cast the ability
                // this needs to follow the movement model refactor
                castPosition = caster.AbilityTargetPoint();

                // Reset indices
                effectsIndex = 0;
                equipmentEffectsIndex = 0;
            }

            bool equipmentComplete = true;

            // FUCK
            // we need abilities with matching timings
            // AND
            // abilities without timing (always)
            // Ideally, a struct with several lists would suit, an AbilityAnimation perhaps
            // it would need to know about the timer and the equipment...?
            // also, which abilities correspond to which placement...?

            // iterate through remaining effects and cast on targets at time specified
            for(int i = effectsIndex; i < effects.Count; ++i){
                if(effectsTiming[i] < abilityTimer.Elapsed()){
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

            foreach(Equipment equipment in equipments){
                for(int i = equipmentEffectsIndex; i < equipment.effects.Count; ++i){
                    if(equipment.effectsTiming[i] < abilityTimer.Elapsed()){
                        foreach(AbilityPlacement placement in placements){
                            Actor[] targets = placement.GetTargetsInCast(castPosition);

                            equipmentEffectsIndex++;
                            foreach(Actor actor in targets){
                                if(!actor || (!includeSelf && actor == caster)){
                                    continue;
                                }

                                equipment.effects[i].Apply(actor);
                            }
                        }
                    }
                }

                equipmentComplete &= (equipmentEffectsIndex == equipment.effects.Count);
            }


            // If we've done all the effects and we're off cooldown, we're done
            if(effectsIndex == effects.Count && equipmentComplete && abilityTimer.Finished()){
                state = AbilityState.Idle;
            }
        }

        previousState = state;
    }

    public void LateUpdate(){
        // Update placements _after_ the caster has moved
        foreach(AbilityPlacement placement in placements){
            placement.Update();
        }
    }

    public void Notify(){
        if(abilityTimer.Finished()){
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
    }

    public void SetCaster(Actor actor){
        caster = actor;

        for(int i = 0; i < placements.Count; ++i){
            placements[i].SetCaster(actor);
        }
    }
}
