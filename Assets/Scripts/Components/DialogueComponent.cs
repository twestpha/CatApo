using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueComponent : MonoBehaviour {

    public const int DialogueCollisionMask = 1 << 9;

    public Strings.LocalizedString text;

    private bool dialogueInUse;
    private GameObject player;
    public PlayerUIController playerUIController;

    void Start(){
        player = GameObject.FindWithTag("Player");
        dialogueInUse = false;
    }

    void Update(){
        if(dialogueInUse && playerUIController){
            if((transform.position - player.transform.position).magnitude > PlayerComponent.DialogueDistance){
                playerUIController.DisableDialogueUI();
            }
        }
    }

	public string GetString(){
        return Strings.GetString(text);
	}

    public void NotifyBeingUsedByUI(){
        dialogueInUse = true;
    }

    public void NotifyNotBeingUsedByUI(){
        dialogueInUse = false;
    }
}
