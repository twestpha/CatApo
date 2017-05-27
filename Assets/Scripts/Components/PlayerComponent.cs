using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : Actor {

    // Constants

    private const float kMouseDragTime = 0.25f;
    private const float kPlayerStopRadius = 0.1f;
    private const float kPlayerPlaneOffset = -1.0f;

    [Header("Jumping")]
    public float jumpSpeed = 0.09f;
    public float jumpTime = 0.22f;

    private float jumpTimeElapsed;

    // [Header("Actor Connections")]
    // public Camera playerCamera;
    // public GameObject uiRotator;
    // public GameObject playerModel;

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

        // accelerationMultiplier = 0.0f;
        // decelerationRadius = currentMoveSpeed / (2.0f * moveAcceleration);

        // Setting initial states
        moving  = false;
        jumping = false; // probably can be an ability too
        dashing = false; // really should be an ability now
        currentHealth = maxHealth;

        // Setup actor plane
        playerPlane = new Plane(Vector3.down, characterController.transform.position.y/* + kPlayerPlaneOffset*/);
	}

    new void Update(){
        base.Update();

        HandleInputs();
        HandleMove();
    }

    void FixedUpdate(){
        // PHYSICS ONLY SHIT
    }

    override public void HandleInputs(){
        if(uiState == UIState.InventoryHidden){
            // updateGameUI();

            // Input Logic
            if(Input.GetButton("Fire2")){
                targetPosition = MouseIntersectionWithTerrain();
            }


        } else {
            // HandleUIInputs();
        }
    }

    //##########################################################################
    // Player Actions
    //##########################################################################

    Vector3 HandleJump(){
        Vector3 jumpVelocity = Vector3.zero;

        bool jumpButton = Input.GetKey("space");

        if(characterController.isGrounded){
            jumping = false;
            jumpUnreleased = false;
        }

        if(jumpButton && characterController.isGrounded && !jumping){
            jumping = true;
            jumpUnreleased = true;
            jumpTimeElapsed = 0.0f;

            jumpVelocity.y = jumpSpeed;
        }

        if(jumpButton && !characterController.isGrounded && jumpUnreleased){
            jumpTimeElapsed += Time.deltaTime;

            if(jumpTimeElapsed < jumpTime){
                jumpVelocity.y = jumpSpeed;
            }
        }

        if(jumping && !jumpButton){
            jumpUnreleased = false;
        }

        return jumpVelocity;
    }

    //##########################################################################
    // Mouse-to-world helper functions
    //##########################################################################
    override public Vector3 AbilityTargetPoint(){
        return MouseIntersectionWithPlayerPlane();
    }

    public Vector3 MouseIntersectionWithTerrain(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, kTerrainCollisionMask)){
            return hit.point;
        }

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
