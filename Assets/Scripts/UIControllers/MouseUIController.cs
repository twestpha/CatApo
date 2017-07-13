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

        if(player.GetComponent<PlayerComponent>().MouseIntersectionWithDialogue()){
            GetComponent<Image>().sprite = cursor_2;
        } else {
            GetComponent<Image>().sprite = cursor_1;
        }
	}
}
