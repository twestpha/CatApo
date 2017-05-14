using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraComponent : MonoBehaviour {

    // might have it smoothdamp & follow player, fairly quickly
    // with a tiny bit of rotation wiggle from mouse movement
    private Quaternion originalRotation;
    public GameObject targetObject;
    private Vector3 cameraOffset;
    private Vector3 velocity;
    public float positionSmoothTime;
    public float rotationSmoothTime;
    public float xscale;
    public float yscale;
    private float xvel;
    private float yvel;
    private float lastx;
    private float lasty;
    // public float maxDegrees;

	void Start(){
        originalRotation = transform.rotation;
        cameraOffset = transform.position - targetObject.transform.position;
	}

	void Update(){
        // Slight rotation
        Vector3 mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f, 0.0f);

        float xrot = xscale * mousePosition.x;
        float yrot = yscale * mousePosition.y;

        float xnew = Mathf.SmoothDamp(lastx, xrot, ref xvel, rotationSmoothTime);
        float ynew = Mathf.SmoothDamp(lasty, yrot, ref yvel, rotationSmoothTime);

        lastx = xnew;
        lasty = ynew;

        transform.rotation = originalRotation * Quaternion.Euler(-ynew, xnew, 0.0f);

        // Following targetObject
        transform.position = Vector3.SmoothDamp(transform.position, targetObject.transform.position + cameraOffset, ref velocity, positionSmoothTime);
	}
}
