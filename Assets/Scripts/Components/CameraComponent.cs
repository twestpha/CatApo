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
    public float xscale;
    public float yscale;

	void Start(){
        cameraOffset = transform.position - targetObject.transform.position;
	}

	void Update(){
        // Slight translation
        Vector3 mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f, 0.0f);

        float xtrans = xscale * mousePosition.x;
        float ytrans = yscale * mousePosition.y;

        // Following targetObject
        transform.position = Vector3.SmoothDamp(transform.position, targetObject.transform.position + cameraOffset + new Vector3(xtrans, 0.0f, ytrans), ref velocity, positionSmoothTime);

        // cast ray from camera to  player, get occluding objects, enable the non-occlusion shader
        // RaycastHit hit;
        // Vector3 direction = targetObject.transform.position - transform.position;
        // if(Physics.Raycast(transform.position, direction, out hit, direction.magnitude, 1 << 12)){
        //     if(hit.collider){
        //         hit.collider.GetComponent<TestComponent>().EnableNonOcclusion();
        //     }
        // }
	}
}
