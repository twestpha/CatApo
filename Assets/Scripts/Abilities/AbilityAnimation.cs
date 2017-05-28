using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu()]
[System.Serializable]
public class AbilityAnimation : ScriptableObject {
    public bool debug;

    [Header("Always Effects")]
    public List<AbilityEffect> alwaysEffects;
    public List<int> alwaysPlacementIndex;

    [Header("Timed Effects")]
    public List<AbilityEffect> effects;
    public List<float> effectsTiming;
    private List<Timer> effectsTimers = new List<Timer>();
    public List<int> effectsPlacementIndex;

    private bool complete;

    public void Start(){
        if(debug){
            Assert.IsTrue(alwaysEffects.Count == alwaysPlacementIndex.Count, "Always Effect's count doesn't match the indices list");
            Assert.IsTrue(effects.Count == effectsTiming.Count, "Effect's count doesn't match timer count");
            Assert.IsTrue(effects.Count == effectsPlacementIndex.Count, "Effect's count doesn't match indices count");
        }
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

    public void Apply(List<AbilityPlacement> placements){
        // Apply all always effects
        for(int i = 0; i < alwaysEffects.Count; ++i){
            List<Actor> actorsInCast = placements[alwaysPlacementIndex[i]].GetTargetsInCast();

            foreach(Actor actor in actorsInCast){
                alwaysEffects[i].Apply(actor);
            }
        }

        // If the timer is up this frame, apply the normal effects
        for(int i = 0; i < effects.Count; ++i){
            if(effectsTimers[i].FinishedThisFrame()){
                List<Actor> actorsInCast = placements[effectsPlacementIndex[i]].GetTargetsInCast();

                foreach(Actor actor in actorsInCast){
                    effects[i].Apply(actor);
                }
            }
        }
    }
}
