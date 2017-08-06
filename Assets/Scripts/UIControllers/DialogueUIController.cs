using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogueUIController : MonoBehaviour {

    private Vector3 dialogueOffset = new Vector3(0.0f, 100.0f);
    private GameObject dialogueObject;
    private GameObject oldDialogueObject;

    public GameObject dialogueText;
    public GameObject dialoguePanel;
    public GameObject dialogueImage;

    private Timer fadeTimer;

    public GameObject dialogueLight;
    private Light dialogueLightComponent;
    private float dialogueLightBrightness = 8.0f;

    public enum DialogueState {
        FadingIn,
        FadingOut,
        FadingOutIn,
        Enabled,
        Disabled,
    };

    public DialogueState state;

    void Start(){
        dialogueLightComponent = dialogueLight.GetComponent<Light>();

        state = DialogueState.Disabled;
        fadeTimer = new Timer(0.2f);
    }

    void Update(){
        if(state == DialogueState.FadingIn){
            SetDialogueFade(fadeTimer.Parameterized());

            transform.position = Camera.main.WorldToScreenPoint(dialogueObject.transform.position) + dialogueOffset;
            dialogueLight.transform.position = dialogueObject.transform.position;

            if(fadeTimer.Finished()){
                state = DialogueState.Enabled;
            }
        } else if(state == DialogueState.FadingOut || state == DialogueState.FadingOutIn){
            SetDialogueFade(1.0f - fadeTimer.Parameterized());

            if(oldDialogueObject){
                transform.position = Camera.main.WorldToScreenPoint(oldDialogueObject.transform.position) + dialogueOffset;
                dialogueLight.transform.position = oldDialogueObject.transform.position;
            } else {
                transform.position = Camera.main.WorldToScreenPoint(dialogueObject.transform.position) + dialogueOffset;
                dialogueLight.transform.position = dialogueObject.transform.position;
            }

            if(fadeTimer.Finished()){
                if(state == DialogueState.FadingOut){
                    dialogueLightComponent.enabled = false;
                    GetComponent<Canvas>().enabled = false;

                    dialogueObject.GetComponent<DialogueComponent>().NotifyNotBeingUsedByUI();
                    dialogueObject = null;

                    state = DialogueState.Disabled;
                } else {
                    oldDialogueObject.GetComponent<DialogueComponent>().NotifyNotBeingUsedByUI();
                    dialogueObject.GetComponent<DialogueComponent>().NotifyBeingUsedByUI();

                    dialogueText.GetComponent<Text>().text = dialogueObject.GetComponent<DialogueComponent>().GetString();

                    oldDialogueObject = null;

                    state = DialogueState.FadingIn;
                    fadeTimer.Start();
                }
            }
        } else if(state == DialogueState.Enabled){
            transform.position = Camera.main.WorldToScreenPoint(dialogueObject.transform.position) + dialogueOffset;
            dialogueLight.transform.position = dialogueObject.transform.position;
        } else if(state == DialogueState.Disabled){

        }
    }

    private void SetDialogueFade(float alpha){
        Color textColor = dialogueText.GetComponent<Text>().color;
        Color imageColor = dialogueImage.GetComponent<Image>().color;
        Color panelColor = dialoguePanel.GetComponent<Image>().color;

        textColor.a = alpha;
        imageColor.a = alpha;
        panelColor.a = alpha;

        dialogueText.GetComponent<Text>().color = textColor;
        dialogueImage.GetComponent<Image>().color = textColor;
        dialoguePanel.GetComponent<Image>().color = textColor;

        dialogueLightComponent.intensity = dialogueLightBrightness * alpha;
    }

    public void EnableDialogueUI(GameObject newDialogueObject){
        if(dialogueObject){
            state = DialogueState.FadingOutIn;
        } else {
            state = DialogueState.FadingIn;
            dialogueText.GetComponent<Text>().text = newDialogueObject.GetComponent<DialogueComponent>().GetString();
        }

        oldDialogueObject = dialogueObject;
        dialogueObject = newDialogueObject;

        fadeTimer.Start();

        GetComponent<Canvas>().enabled = true;
        dialogueLightComponent.enabled = true;
    }

    public void DisableDialogueUI(){
        state = DialogueState.FadingOut;
        fadeTimer.Start();

        if(dialogueObject){
            dialogueObject.GetComponent<DialogueComponent>().NotifyNotBeingUsedByUI();
        }
    }
}
