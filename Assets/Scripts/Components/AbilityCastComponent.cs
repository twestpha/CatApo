using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent (typeof (PlayerComponent))]
public class AbilityCastComponent : MonoBehaviour {

    // Might want hidden passive abilities
    private const int ABILITY_MAX = 4;

    [Header("Ability Key Bindings")]
    public KeyCode[] abilityHotkeys;

    [Header("Abilities")]
    public List<Ability> abilities;

    private PlayerComponent playercomponent;

	void Start(){
        for(int i = 0; i < abilities.Count; ++i){
            abilities[i] = UnityEngine.Object.Instantiate(abilities[i]);
            abilities[i].SetSelfActor(gameObject.GetComponent<Actor>());
            abilities[i].Validate();
            abilities[i].Start();
        }
	}

	void Update(){
        for(int i = 0; i < abilities.Count; ++i){
            if(Input.GetKeyDown(abilityHotkeys[i])){
                abilities[i].Cast();
            }

            abilities[i].Update();
        }
	}

    void LateUpdate(){
        foreach(Ability ability in abilities){
            ability.LateUpdate();
        }
    }
}
