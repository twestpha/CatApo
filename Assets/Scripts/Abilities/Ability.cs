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

    public enum Type {
        Passive,  // will automatically cast when off cooldown
        Toggle,   // will automatically cast when off cooldown and on, and hotkey toggles on/off
        OnHotkey, // requires a hotkey to be pressed
        OnClick,  // requires a hotkey to be pressed, then a left mouse click to confirm the cast location
    }

    private Actor caster;

    public AbilityState state;
    private AbilityState previousState;
    public Type type;
    // private bool enabled;

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
    private Timer cooldownTimer;

    // cast location
    private Vector3 castPosition;

    [Header("Equipment")]
    public List<Equipment> equipments;
    private int equipmentEffectsIndex;

    [Header("Placements")]
    public List<AbilityPlacement> placements;

    // which effect goes to which volume goes to which placement
    // parallel list of placements and volumes, effect has index into that?
    // well if we have a heal in 3 circles, then we need three seperate ability effects... seems shit

    [Header("Ability Animation")]
    public AbilityAnimation abilityAnimation;

    public void Start(){
        if(debug){
            Assert.IsTrue(abilityAnimation, "Ability \"" + abilityName + "\" is missing an AbilityAnimation.");
            Assert.IsTrue(placements.Count > 0, "Ability \"" + abilityName + "\" is missing an AbilityPlacement.");
        }

        state = AbilityState.Idle;

        cooldownTimer = new Timer(cooldown);

        for(int i = 0; i < placements.Count; ++i){
            placements[i] = UnityEngine.Object.Instantiate(placements[i]);
            placements[i].Start();
            placements[i].SetParent(this);
        }

        abilityAnimation = UnityEngine.Object.Instantiate(abilityAnimation);
        abilityAnimation.Start();
    }

    public void Update(){
        // If we're notified, wait for right or left click to either cast or cancel
        if(state == AbilityState.Notified){
            if(Input.GetMouseButtonDown(0)){
                state = AbilityState.Casted;
            } else if(Input.GetMouseButtonDown(1)){
                state = AbilityState.Idle;
            }
        }
        // If we're casted, get all the targets from all placements and cast effects on said targets
        if(state == AbilityState.Casted){
            // Casted last frame
            if(previousState != AbilityState.Casted){
                // Start the timer
                cooldownTimer.Start();

                // TODO Get casting position and snap to target over set amount of time, then cast the ability
                // this needs to follow the movement model refactor
                castPosition = caster.AbilityTargetPoint();

                abilityAnimation.Cast();

            }

            bool effectsComplete = abilityAnimation.Apply(castPosition, placements);

            // If we've done all the effects and we're off cooldown, we're done
            if(effectsComplete && cooldownTimer.Finished()){
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
        if(cooldownTimer.Finished()){
            if(type == Type.OnHotkey){
                // If ability requires hotkey only
                state = AbilityState.Casted;
            } else if(type == Type.OnClick){
                // If ability requires a left click
                state = AbilityState.Notified;
            }

            // foreach(AbilityPlacement placement in placements){
            // enable or disable placements as appropriate?
            // }
        }
    }

    public void SetCaster(Actor actor){
        caster = actor;

        for(int i = 0; i < placements.Count; ++i){
            placements[i].SetCaster(actor);
        }
    }
}
