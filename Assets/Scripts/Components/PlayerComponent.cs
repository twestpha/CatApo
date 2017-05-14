using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : Actor {

    // Constants
    private const int kTerrainCollisionMask = 1 << 8;
    private const float kMouseDragTime = 0.25f;
    private const float kPlayerStopRadius = 0.1f;
    private const float kPlayerPlaneOffset = -1.0f;

    [Header("Jumping")]
    public float jumpSpeed = 0.1f;
    public float jumpTime = 0.5f;
    public float gravity = 8.0f;
    private float jumpTimeElapsed;

    // [Header("Actor Connections")]
    // public Camera playerCamera;
    // public GameObject uiRotator;
    // public GameObject playerModel;

    public enum ActionState { // Seems not descriptive... probably should set this up better
        None,
        Jumping,
        CastingAbility1,
        CastingAbility2,
        CastingAbility3,
        CastingUltimate,
        Channeling,
    };

    public enum UIState {
        InventoryHidden,
        InventoryOpen
    }

    public bool moving;
    private bool jumpUnreleased;
    public bool jumping;
    public bool dashing;

    public ActionState actionState;
    public UIState uiState;

    // Component References
    private CharacterController characterController;

    // Player Plane
    private Plane playerPlane;

	void Start(){
        characterController = GetComponent<CharacterController>();
        targetPosition = characterController.transform.position;

        accelerationMultiplier = 0.0f;
        decelerationRadius = currentMoveSpeed / (2.0f * moveAcceleration);

        // Setting initial states
        moving  = false;
        jumping = false;
        dashing = false;
        actionState = ActionState.None;
        currentHealth = maxHealth;

        // Setup player plane
        playerPlane = new Plane(Vector3.down, characterController.transform.position.y + kPlayerPlaneOffset);
	}

    void Update(){
        HandleInputs();
        // should put this in late update probably
        characterController.Move(velocity);
    }

    void FixedUpdate(){
        // PHYSICS ONLY SHIT
    }

    void HandleInputs(){
        velocity.x = 0.0f;
        velocity.z = 0.0f;

        if(uiState == UIState.InventoryHidden){
            // updateGameUI();

            // HandleAbilities();
            // Vector3 abilityVelocity  = HandleAbilities();
            Vector3 jumpVelocity     = HandleJump();
            Vector3 movementVelocity = HandleMove();

            bool canMove = (moving || jumping) && !dashing;
            bool canJump = jumpVelocity.y != 0.0f && !dashing;
            // bool canDash = (moving || dashing) && !jumping;

            if(canMove){
                velocity += movementVelocity;
            }

            if(canJump){
                velocity.y = jumpVelocity.y;
            }

            if(jumping || !characterController.isGrounded){
                velocity.y -= gravity * Time.deltaTime;
            }
        } else {
            // HandleUIInputs();
        }
    }

    //##########################################################################
    // Player Actions
    //##########################################################################
    Vector3 HandleMove(){
        Vector3 movementVelocity = Vector3.zero;

        bool rightMouseButton = Input.GetButton("Fire2");

        // Input Logic
        if(rightMouseButton){
            targetPosition = MouseTarget();
            moving = true;
            actionState = ActionState.None;
        }

        // Apply movement
        Vector3 moveVector3D = targetPosition - characterController.transform.position;
        Vector3 moveVector2D = new Vector3(moveVector3D.x, 0.0f, moveVector3D.z);

        if(moveVector2D.magnitude > kPlayerStopRadius){
            if(moveVector2D.magnitude > decelerationRadius){
                accelerationMultiplier = Mathf.Min(accelerationMultiplier + (moveAcceleration * Time.deltaTime), 1.0f);
            } else {
                accelerationMultiplier = Mathf.Max(accelerationMultiplier - (moveAcceleration * Time.deltaTime), 0.0f);
            }

            moveVector2D.Normalize();
            movementVelocity += moveVector2D * currentMoveSpeed * Time.deltaTime * accelerationMultiplier;

            // Vector3 playerModelDirection = Vector3.RotateTowards(playerModel.transform.forward, moveVector2D, turnSpeed * Time.deltaTime, 0.0f);
            // playerModel.transform.rotation = Quaternion.LookRotation(playerModelDirection);
        } else {
            moving = false;
            accelerationMultiplier = 0.0f;
        }

        return movementVelocity;
    }

    Vector3 HandleJump(){
        Vector3 jumpVelocity = Vector3.zero;

        bool jumpButton = Input.GetKey("space");

        if(characterController.isGrounded){
            jumping = false;
            jumpUnreleased = false;
        }

        if(jumpButton && characterController.isGrounded && !jumping){
            Debug.Log("Jumped");
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
    Vector3 MouseTarget(){
        // Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, kTerrainCollisionMask)){
            return hit.point;
        }

        return IntersectionWithPlayerPlane(); // As fallback, use plane player is on.
    }

    Vector3 IntersectionWithPlayerPlane(){
        playerPlane.distance = characterController.transform.position.y + kPlayerPlaneOffset;
        // Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float rayDistance;

        if(playerPlane.Raycast(ray, out rayDistance)){
            return ray.GetPoint(rayDistance);
        }

        Debug.LogError("Intersection with player plane has failed unexpectedly.");
        return Vector3.zero;
    }
}
