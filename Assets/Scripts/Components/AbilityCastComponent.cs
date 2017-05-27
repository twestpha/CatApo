using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent (typeof (PlayerComponent))]
public class AbilityCastComponent : MonoBehaviour {

    [Header("Ability Key Bindings")]
    public KeyCode[] abilityHotkeys;

    [Header("Abilities")]
    public List<Ability> abilities;

    private PlayerComponent playercomponent;

	void Start(){
        for(int i = 0; i < abilities.Count; ++i){
            abilities[i] = UnityEngine.Object.Instantiate(abilities[i]);
            abilities[i].Start();
            abilities[i].SetCaster(gameObject.GetComponent<Actor>());
        }
	}

	void Update(){
        for(int i = 0; i < abilities.Count; ++i){
            if(Input.GetKeyDown(abilityHotkeys[i])){
                abilities[i].Notify();
            }

            abilities[i].Update();
        }
	}

    void LateUpdate(){
        for(int i = 0; i < abilities.Count; ++i){
            abilities[i].LateUpdate();
        }
    }
}
