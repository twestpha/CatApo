using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogueUIController : MonoBehaviour {

    private bool dialogueEnabled;
    private Vector3 dialogueOffset = new Vector3(0.0f, 100.0f);
    private GameObject dialogueObject;
    public GameObject dialogueText;

    public GameObject dialogueLight;
    private Light dialogueLightComponent;

    void Start(){
        dialogueEnabled = false;
        dialogueLightComponent = dialogueLight.GetComponent<Light>();
    }

    void Update(){
        if(dialogueEnabled){
            transform.position = Camera.main.WorldToScreenPoint(dialogueObject.transform.position) + dialogueOffset;
            dialogueLight.transform.position = dialogueObject.transform.position;
            GetComponent<Canvas>().enabled = true;
            dialogueLightComponent.enabled = true;
        }
    }

    public void EnableDialogueUI(GameObject dialogueObject){
        DialogueComponent dialogueComponent = dialogueObject.GetComponent<DialogueComponent>();
        dialogueComponent.NotifyBeingUsedByUI();

        dialogueText.GetComponent<Text>().text = dialogueComponent.GetString();
        this.dialogueObject = dialogueObject;
        dialogueEnabled = true;
    }

    public void DisableDialogueUI(){
        if(dialogueObject){
            dialogueObject.GetComponent<DialogueComponent>().NotifyNotBeingUsedByUI();
        }

        GetComponent<Canvas>().enabled = false;
        dialogueLightComponent.enabled = false;
        dialogueEnabled = false;
    }
}
