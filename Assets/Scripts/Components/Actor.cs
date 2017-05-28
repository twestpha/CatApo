using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Actor : MonoBehaviour {

    // Constants
    protected const int kTerrainCollisionMask = 1 << 8;

    [Header("Base Actor Properties")]

    [Header("Health")]
    public float maxHealth;
    public float currentHealth;

    [Header("Movement")]
    public float maxMoveSpeed;
    public float currentMoveSpeed;

    protected Vector3 targetPosition;
    protected Vector3 velocity;

    public float gravity = 9.8f;

    // Component References
    protected CharacterController characterController;

    private bool actorInFramePosition;

    // Status timers
    private Timer rootTimer = new Timer();

	protected void Start(){
        currentHealth = maxHealth;
        currentMoveSpeed = maxMoveSpeed;

        characterController = GetComponent<CharacterController>();
        targetPosition = characterController.transform.position;
	}

    //##########################################################################
    // Actor Update
    //##########################################################################
	protected void Update(){
        if(rootTimer.Finished()){
            currentMoveSpeed = maxMoveSpeed;
        }
	}

    //##########################################################################
    // Actor Actions
    //##########################################################################
    virtual public void HandleInputs(){

    }

    protected void HandleMove(){
        Vector3 movementVelocity = Vector3.zero;
        // Apply movement
        Vector3 moveVector3D = targetPosition - characterController.transform.position;
        Vector3 moveVector2D = new Vector3(moveVector3D.x, 0.0f, moveVector3D.z);

        if(moveVector2D.magnitude > 0.1f){
            moveVector2D.Normalize();
            movementVelocity += moveVector2D * currentMoveSpeed * Time.deltaTime;
        }

        velocity = movementVelocity;

        if(!characterController.isGrounded){
            velocity.y -= gravity * Time.deltaTime;
        }

        characterController.Move(velocity);

        // rotate caster to face click target position
        if(movementVelocity.magnitude > 0.01f){
            float angle = Mathf.Rad2Deg * Mathf.Atan2(velocity.x, velocity.z);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
        }
    }

    //##########################################################################
    // Status methods
    //##########################################################################
    public void Root(float duration){
        // this is kind of a hack around the poor movement model we're using
        rootTimer = new Timer(duration);
        rootTimer.Start();
        currentMoveSpeed = 0.0f;
        targetPosition = transform.position;
    }

    //##########################################################################
    // Something
    //##########################################################################
    virtual public Vector3 AbilityTargetPoint(){
        return characterController.transform.position;
    }

    //##########################################################################
    // Iunno
    //##########################################################################
    public Vector3 FinalActorPosition(){
        return transform.position + velocity;
    }
}
