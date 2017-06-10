using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using System;

[RequireComponent (typeof (PlayerComponent))]
public class AbilityCastComponent : MonoBehaviour {

    // [Header("Ability Key Bindings")]
    // public KeyCode[] abilityHotkeys;

    [Header("Abilities")]
    public List<String> abilityNames;
    private List<Ability> abilities;

    private PlayerComponent playercomponent;

	void Start(){
        abilities = new List<Ability>();

        for(int i = 0; i < abilityNames.Count; ++i){
            Ability ability = (Ability) ScriptableObject.CreateInstance(abilityNames[i]);
            Assert.IsTrue(ability, "Error creating ability " + abilityNames[i]);

            ability.selfActor = GetComponent<Actor>();
            ability.castComponent = this;
            ability.selfAbilityIndex = i;

            ability.Setup();

            abilities.Add(ability);
        }

	}

	void Update(){
        for(int i = 0; i < abilities.Count; ++i){
            abilities[i].Update();
        }
	}

    void LateUpdate(){

    }

    void OnDestroy() {
        for(int i = 0; i < abilities.Count; ++i){
            abilities[i].OnDestroy();
        }
    }
}
