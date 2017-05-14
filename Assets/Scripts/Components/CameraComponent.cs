using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraComponent : MonoBehaviour {

    // might have it smoothdamp & follow player, fairly quickly
    // with a tiny bit of rotation wiggle from mouse movement
    private Quaternion originalRotation;
    public float xscale;
    public float yscale;
    public float maxDegrees;

	void Start(){
        originalRotation = transform.rotation;
	}

	void Update(){
        Vector3 mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f, 0.0f);
        mousePosition = new Vector3(xscale * mousePosition.x, yscale * mousePosition.y, mousePosition.z);

        Debug.Log(mousePosition);
        transform.rotation = originalRotation * Quaternion.Euler(-mousePosition.y * maxDegrees, mousePosition.x * maxDegrees, 0.0f);;
	}
}
