using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueComponent : Interactable {

    public Strings.LocalizedString text;

    private bool dialogueInUse;
    private GameObject player;
    private PlayerUIController playerUIController;

    void Start(){
        player = GameObject.FindWithTag("Player");
        playerUIController = GameObject.FindWithTag("PlayerUI").GetComponent<PlayerUIController>();
        dialogueInUse = false;
    }

    void Update(){
        if(dialogueInUse){
            if(!NearPlayer()){
                playerUIController.DisableDialogueUI();
                dialogueInUse = false;
            }
        }
    }

    private bool NearPlayer(){
        return (transform.position - player.transform.position).magnitude <= PlayerComponent.DialogueDistance;
    }

    override public void NotifyClicked(){
        if(NearPlayer()){
            dialogueInUse = true;
            playerUIController.EnableDialogueUI(gameObject);
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
