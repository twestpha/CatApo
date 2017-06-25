using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestComponent : MonoBehaviour {

    public GameObject moveme;
    public float movedistance;

	void Start(){
        //Debug.LogError("DO NOT USE THIS SCRIPT FOR ANYTHING EXCEPT TESTING.");
        moveme.transform.position += Vector3.up * movedistance;
	}

	void Update(){

	}

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            moveme.transform.position -= Vector3.up * movedistance;
        }
    }

    void OnTriggerExit(Collider other) {
        if(other.tag == "Player"){
            moveme.transform.position += Vector3.up * movedistance;
        }
    }
}
