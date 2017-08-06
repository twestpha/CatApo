using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseUIController : MonoBehaviour {

    public GameObject player;
    public Sprite cursor_1;
    public Sprite cursor_2;

	void Start(){
        Cursor.visible = false;
	}

	void Update(){
        transform.position = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, Mathf.Infinity, Interactable.InteractableCollisionMask)){
            GetComponent<Image>().sprite = cursor_2;
        } else {
            GetComponent<Image>().sprite = cursor_1;
        }
	}
}
