using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackUIController : MonoBehaviour {

    private enum MouseButton {
        Right,
        Left,
        Middle,
    }

    public GameObject player;
    public GameObject abilityController;

    public GameObject grabbedIcon;

    public List<GameObject> gridIcons;

    private bool backpackEnabled;

    void Start(){
        // set up the icons from the player's inventory component
        InventoryComponent playerInventory = player.GetComponent<InventoryComponent>();
        for(int i = 0; i < playerInventory.backpackCastables.Length; ++i){
            Castable castable = playerInventory.backpackCastables[i];
            if(castable){
                Image castableIconImage = gridIcons[i].GetComponent<Image>();
                Debug.Log("Setting up castable " + castable);
                castableIconImage.sprite = castable.icon;
                castableIconImage.enabled = true;
            }
        }
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
        if(!enabled){
            return;
        }

        if(Input.GetMouseButtonDown((int) MouseButton.Right)){

        }
    }
}
