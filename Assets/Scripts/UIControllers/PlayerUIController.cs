using System.Collections;
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
    private Actor playerActor;

    [Header("Ability Icons")]
    public List<GameObject> abilityIcons;
    public List<GameObject> abilityIconCovers;

    private AbilityCastComponent castComponent;


    [Header("Health Bars")]
    private int lastHealth;
    private int lastArmor;

    public GameObject healthBarParent;
    public GameObject healthBarPrefab;
    private List<HealthUIController> healthBars;

    private AudioSource audioSource;
    public AudioClip playerLostArmorSound;
    public AudioClip playerGainedArmorSound;
    public AudioClip playerLostHealthSound;
    public AudioClip playerGainedHealthSound;

    [Header("Backpack UI")]
    public GameObject grabbedIcon;

    public List<GameObject> gridIcons;

    [Header("Dialogue")]
    public GameObject dialogueCanvas;
    private DialogueUIController dialogueCanvasController;

    private bool backpackEnabled;

	void Start(){
        // abilities
        castComponent = player.GetComponent<AbilityCastComponent>();

        // health bars
        healthBars = new List<HealthUIController>();

        Actor[] actors = (Actor[]) GameObject.FindObjectsOfType (typeof(Actor));

        for(int i = 0; i < actors.Length; ++i){
            GameObject healthBar = Object.Instantiate(healthBarPrefab);
            HealthUIController healthUIController = healthBar.GetComponent<HealthUIController>();

            healthUIController.target = actors[i].gameObject;
            healthUIController.CreateHearts();
            healthBar.transform.SetParent(healthBarParent.GetComponent<Canvas>().transform);

            healthBars.Add(healthUIController);
        }

        // ui sounds
        audioSource = GetComponent<AudioSource>();
        playerActor = player.GetComponent<Actor>();
        lastHealth = playerActor.currentHealth;
        lastArmor = playerActor.currentArmor;

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

        // Dialogue
        dialogueCanvasController = dialogueCanvas.GetComponent<DialogueUIController>();
	}

    public void ToggleBackpack(){
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
        if(lastHealth != playerActor.currentHealth){
            if(lastHealth > playerActor.currentHealth){
                audioSource.clip = playerLostHealthSound;
                audioSource.Play();
            } else {
                audioSource.clip = playerGainedHealthSound;
                audioSource.Play();
            }
            lastHealth = playerActor.currentHealth;
        }

        if(lastArmor != playerActor.currentArmor){
            if(lastArmor > playerActor.currentArmor){
                audioSource.clip = playerLostArmorSound;
                audioSource.Play();
            } else {
                audioSource.clip = playerGainedArmorSound;
                audioSource.Play();
            }
            lastArmor = playerActor.currentArmor;
        }

        // backpack ui
        if(Input.GetKeyDown(KeyCode.Tab)){
            // backpackCanvas.GetComponent<BackpackUIController>().ToggleBackpack();
        }

        if(enabled){
            // on key down, iterate over grid icons to see if we've pressed one
            if(Input.GetMouseButtonDown((int) MouseButton.Right)){
                for(int i = 0; i < gridIcons.Count; ++i){

                }
            }
        }
	}

    public void EnableDialogueUI(GameObject dialogueObject){
        dialogueObject.GetComponent<DialogueComponent>().playerUIController = this;

        dialogueCanvasController.EnableDialogueUI(dialogueObject);
    }

    public void DisableDialogueUI(){
        dialogueCanvasController.DisableDialogueUI();
    }
}
