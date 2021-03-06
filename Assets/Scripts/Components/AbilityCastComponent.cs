﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using System;

[RequireComponent (typeof (PlayerComponent))]
public class AbilityCastComponent : MonoBehaviour {

    [Header("THESE ARE ONLY FOR DEBUGGING")]

    [Header("Ability Key Bindings")]
    public KeyCode[] abilityHotkeys;

    [Header("Abilities")]
    public List<String> abilityNames;
    private List<Ability> abilities;

    private PlayerComponent playercomponent;

    public enum AbilitySlot {
        QSlot,
        WSlot,
        ESlot,
        RSlot,
        DefaultAbilities,
        JumpSlot = DefaultAbilities,
        DashSlot,
        OverflowAbilities,
        Overflow1Slot = OverflowAbilities,
        Overflow2Slot,
        Overflow3Slot,
        Overflow4Slot,
        MaxAbilities,
    }


	void Start(){
        abilities = new List<Ability>();

        for(int i = 0; i < abilityNames.Count; ++i){
            Ability ability = (Ability) ScriptableObject.CreateInstance(abilityNames[i]);
            Assert.IsTrue(ability, "Error creating ability " + abilityNames[i]);

            ability.selfActor = GetComponent<Actor>();
            ability.castComponent = this;
            ability.selfAbilityIndex = i;

            ability.hotkey = abilityHotkeys[i];

            ability.SetupAbility();

            abilities.Add(ability);
        }

	}

	void Update(){
        for(int i = 0; i < abilities.Count; ++i){
            abilities[i].Update();
        }
	}

    public float GetCooldownPercentage(int index){
        if(index < abilities.Count && abilities[index]){
            return abilities[index].GetCooldownPercentage();
        }
        return 1.0f;
    }

    void LateUpdate(){

    }

    void OnDestroy() {
        for(int i = 0; i < abilities.Count; ++i){
            abilities[i].OnDestroy();
        }
    }
}
