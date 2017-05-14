﻿using System.Collections;
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
    public float gravity = 0.45f;
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

	new void Start(){
        base.Start();

        targetPosition = characterController.transform.position;

        accelerationMultiplier = 0.0f;
        decelerationRadius = currentMoveSpeed / (2.0f * moveAcceleration);

        // Setting initial states
        moving  = false;
        jumping = false;
        dashing = false;
        actionState = ActionState.None;
        currentHealth = maxHealth;
	}

    new void Update(){
        base.Update();

        HandleInputs();
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
            Vector3 movementVelocity = HandleMove(); // TODO rework this to use smoothdamp

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

            Vector3 playerModelDirection = Vector3.RotateTowards(transform.forward, moveVector2D, turnSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(playerModelDirection);

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
}
