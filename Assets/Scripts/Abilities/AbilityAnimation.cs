using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class AbilityAnimation : ScriptableObject {
    [Header("Always Effects")]
    public List<AbilityEffect> alwaysEffects;
    // public List<float> placementIndex;

    [Header("Time Effects")]
    public List<AbilityEffect> effects;
    public List<float> effectsTiming;
    private List<Timer> effectsTimers = new List<Timer>();
    // public List<float> placementIndex;


    private bool complete;

    public void Start(){
        // Instantiate timers for given timings
        for(int i = 0; i < effectsTiming.Count; ++i){
            effectsTimers.Add(new Timer(effectsTiming[i]));
        }

        // Instantiate effects
        for(int i = 0; i < effects.Count; ++i){
            effects[i] = UnityEngine.Object.Instantiate(effects[i]);
        }

        // Instantiate always effects
        for(int i = 0; i < alwaysEffects.Count; ++i){
            alwaysEffects[i] = UnityEngine.Object.Instantiate(alwaysEffects[i]);
        }

        complete = true;
    }

    public void Cast(){
        foreach(Timer timer in effectsTimers){
            timer.Start();
        }

        complete = false;
    }

    public bool Apply(Vector3 castPosition, List<AbilityPlacement> placements){
        if(!complete){
            return true;
        }

        foreach(AbilityEffect alwaysEffect in alwaysEffects){
            foreach(AbilityPlacement placement in placements){

            }
        }

        foreach(Timer timer in effectsTimers){
            if(timer.FinisedThisFrame()){
                // All sorts of shit
            }
        }

        return false; // return completion status of all the effects
    }
}
