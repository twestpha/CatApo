using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : Actor {

    // Constants

    private const float kMouseDragTime = 0.25f;
    private const float kPlayerStopRadius = 0.1f;
    private const float kPlayerPlaneOffset = -1.0f;

    public enum UIState {
        InventoryHidden,
        InventoryOpen
    }

    public bool moving;
    private bool jumpUnreleased;
    public bool jumping;
    public bool dashing;

    public UIState uiState;

    // Actor Plane
    protected Plane playerPlane;

	new void Start(){
        base.Start();

        // Setting initial states
        moving  = false;

        // Setup actor plane
        playerPlane = new Plane(Vector3.down, characterController.transform.position.y/* + kPlayerPlaneOffset*/);
	}

    new void Update(){
        base.Update();

        HandleInputs();
        HandleMove();
    }

    void FixedUpdate(){

    }

    override public void HandleInputs(){
        if(uiState == UIState.InventoryHidden){
            // updateGameUI();

            // Input Logic
            if(Input.GetButton("Fire2") && steerable){
                targetPosition = MouseIntersectionWithTerrain();
            }
        } else {
            // HandleUIInputs();
        }
    }

    //##########################################################################
    // Mouse-to-world helper functions
    //##########################################################################
    override public Vector3 AbilityTargetPoint(){
        return MouseIntersectionWithPlayerPlane();
    }

    public Vector3 MouseIntersectionWithTerrain(){
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // RaycastHit hit;
        //
        // if(Physics.Raycast(ray, out hit, Mathf.Infinity, kTerrainCollisionMask)){
        //     return hit.point;
        // }

        // fallback
        return MouseIntersectionWithPlayerPlane();
    }

    protected Vector3 MouseIntersectionWithPlayerPlane(){
        playerPlane.distance = characterController.transform.position.y - 1.0f/*+ kPlayerPlaneOffset /* not sure about this bad boy yet */;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float rayDistance;

        if(playerPlane.Raycast(ray, out rayDistance)){
            return ray.GetPoint(rayDistance);
        }

        Debug.LogError("Intersection with player plane has failed unexpectedly.");
        return Vector3.zero;
    }
}
