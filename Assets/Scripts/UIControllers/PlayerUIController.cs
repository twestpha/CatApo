﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerUIController : MonoBehaviour {

    // Reminder from AbilityCastComponent
    // QSlot
    // WSlot
    // ESlot
    // RSlot
    // JumpSlot
    // DashSlot

    private enum MouseButton {
        Right,
        Left,
        Middle,
    }

    public GameObject player;

    //public GameObject backpackCanvas;

    [Header("Icons")]
    public List<GameObject> abilityIcons;
    public List<GameObject> abilityIconCovers;

    private AbilityCastComponent castComponent;

    public GameObject healthBarPrefab;

    private List<HealthUIController> healthBars;

    private int lastHealth;
    private int lastArmor;

    private AudioSource audioSource;
    public AudioClip playerLostArmorSound;
    public AudioClip playerGainedArmorSound;
    public AudioClip playerLostHealthSound;
    public AudioClip playerGainedHealthSound;

    [Header("Icons")]
    public GameObject grabbedIcon;

    public List<GameObject> gridIcons;

    private bool backpackEnabled;

	void Start(){
        // abilities
        castComponent = player.GetComponent<AbilityCastComponent>();

        // health bars
        healthBars = new List<HealthUIController>();

        Actor[] actors = (Actor[]) GameObject.FindObjectsOfType (typeof(Actor));

        for(int i = 0; i < actors.Length; ++i){
            GameObject healthBar = Object.Instantiate(healthBarPrefab);
            healthBar.GetComponent<HealthUIController>().target = actors[i].gameObject;
            healthBar.GetComponent<HealthUIController>().CreateHearts();
            healthBar.transform.SetParent(GetComponent<Canvas>().transform);
            healthBars.Add(healthBar.GetComponent<HealthUIController>());
        }

        // ui sounds
        audioSource = GetComponent<AudioSource>();
        lastHealth = player.GetComponent<Actor>().currentHealth;
        lastArmor = player.GetComponent<Actor>().currentArmor;

        // something
        // set up the icons from the player's inventory component
        // InventoryComponent playerInventory = player.GetComponent<InventoryComponent>();
        // for(int i = 0; i < playerInventory.backpackCastables.Length; ++i){
        //     Castable castable = playerInventory.backpackCastables[i];
        //     if(castable){
        //         Image castableIconImage = gridIcons[i].GetComponent<Image>();
        //         castableIconImage.sprite = castable.icon;
        //         castableIconImage.enabled = true;
        //     }
        // }
	}

    public void Toggle(){
        backpackEnabled = !backpackEnabled;
        if(backpackEnabled){
            Enable();
        } else {
            Disable();
        }
    }

    private void Enable(){
        GetComponent<Canvas>().enabled = true;
    }

    private void Disable(){
        GetComponent<Canvas>().enabled = false;
    }

	void Update(){
        // abilities
        for(int i = 0; i < (int)AbilityCastComponent.AbilitySlot.OverflowAbilities; ++i){
            abilityIconCovers[i].GetComponent<Image>().fillAmount = 1.0f - castComponent.GetCooldownPercentage(i);
        }

        // health bars
        for(int i = 0; i < healthBars.Count; ++i){
            healthBars[i].UpdateUI();
        }

        // ui sounds
        if(lastHealth != player.GetComponent<Actor>().currentHealth){
            if(lastHealth > player.GetComponent<Actor>().currentHealth){
                audioSource.clip = playerLostHealthSound;
                audioSource.Play();
            } else {
                audioSource.clip = playerGainedHealthSound;
                audioSource.Play();
            }
            lastHealth = player.GetComponent<Actor>().currentHealth;
        }

        if(lastArmor != player.GetComponent<Actor>().currentArmor){
            if(lastArmor > player.GetComponent<Actor>().currentArmor){
                audioSource.clip = playerLostArmorSound;
                audioSource.Play();
            } else {
                audioSource.clip = playerGainedArmorSound;
                audioSource.Play();
            }
            lastArmor = player.GetComponent<Actor>().currentArmor;
        }

        // backpack ui
        if(Input.GetKeyDown(KeyCode.Tab)){
            // backpackCanvas.GetComponent<BackpackUIController>().Toggle();
        }

        if(enabled){
            // on key down, iterate over grid icons to see if we've pressed one
            if(Input.GetMouseButtonDown((int) MouseButton.Right)){
                for(int i = 0; i < gridIcons.Count; ++i){

                }
            }
        }
	}
}