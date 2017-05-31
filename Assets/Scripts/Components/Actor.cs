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

    public Vector3 targetPosition;
    public Vector3 velocity;

    public float gravity = 9.8f;

    [Header("Statuses")]
    public bool steerable = true;

    // Component References
    protected CharacterController characterController;

    private bool actorInFramePosition;

    public GameObject target;

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
        target.transform.position = targetPosition;
        Debug.DrawLine(transform.position, transform.forward);
	}

    //##########################################################################
    // Actor Actions
    //##########################################################################
    virtual public void HandleInputs(){

    }

    protected void HandleMove(){
        Vector3 movementVelocity = Vector3.zero;

        // Apply movement
        Vector3 moveVector = targetPosition - characterController.transform.position;
        moveVector.y = 0.0f;

        if(moveVector.magnitude > 0.1f){
            moveVector.Normalize();
            movementVelocity += moveVector * currentMoveSpeed;

            float angle = Mathf.Rad2Deg * Mathf.Atan2(velocity.x, velocity.z);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
        }

        movementVelocity.y = velocity.y;
        velocity = movementVelocity;

        if(!characterController.isGrounded){
            velocity.y -= gravity;
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    //##########################################################################
    // Status methods
    //##########################################################################
    public void Root(float duration){
        // this is kind of a hack around the poor movement model we're using
        // rootTimer = new Timer(duration);
        // rootTimer.Start();
        // currentMoveSpeed = 0.0f;
        // targetPosition = transform.position;
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
