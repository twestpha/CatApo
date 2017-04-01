using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

[CreateAssetMenu()]
[System.Serializable]
public class SelfAbility : Ability {
    override public void Cast(){
        foreach(AbilityEffect effect in effects){
            effect.Apply(selfActor);
        }
    }
}
