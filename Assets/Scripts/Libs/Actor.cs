using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Actor : MonoBehaviour {

    // Constants
    protected const int kTerrainCollisionMask = 1 << 8;
    protected const float kMoveDistanceNear = 0.1f;

    [Header("Base Actor Properties")]

    [Header("Health and Armor")]
    public int maxHealth;
    public int currentHealth;

    public int maxArmor;
    public int currentArmor;

    [Header("Movement")]
    public float maxMoveSpeed;
    public float currentMoveSpeed;
    public float turnSpeed = 15.0f;

    public Vector3 targetPosition;
    public Vector3 velocity;

    public float gravity = -9.8f;

    [Header("Statuses")]
    public bool steerable = true;
    public bool moving = true;
    public bool dashing = false;

    // Component References
    protected CharacterController characterController;

	protected void Start(){
        // currentHealth = maxHealth;
        currentMoveSpeed = maxMoveSpeed;

        characterController = GetComponent<CharacterController>();
        targetPosition = characterController.transform.position;
	}

    //##########################################################################
    // Actor Update
    //##########################################################################
	protected void Update(){

	}

    protected void LateUpdate(){
        if(mtPositionChanged){
            mtPositionChanged = false;
            transform.position = mtPosition;
        }

        mtPosition = transform.position;
        mtGrounded = characterController.isGrounded;
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

        float moveDistance = moveVector.magnitude;
        moveVector.Normalize();

        if(moveDistance > kMoveDistanceNear){
            moving = true;
            movementVelocity += moveVector * currentMoveSpeed;

            // rotate towards velocity
            float step = turnSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, moveVector, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        } else {
            moving = false;
        }

        if(!characterController.isGrounded){
            velocity.y += gravity * Time.deltaTime;
        }

        characterController.Move((velocity + movementVelocity) * Time.deltaTime);
    }

    //##########################################################################
    // Thread safe interface
    // These are necessary for reading and writing to the Unity API from the
    // ability scripts, because they're threaded
    //##########################################################################
    private Vector3 mtPosition;
    private bool mtPositionChanged;
    private bool mtGrounded;

    public void SetMTPosition(Vector3 position){
        mtPositionChanged = true;
        mtPosition = position;
    }

    public Vector3 GetMTPosition(){
        return mtPosition;
    }

    public bool GetMTGrounded(){
        return mtGrounded;
    }

    //##########################################################################
    // common operations
    //##########################################################################
    public void Heal(int amount){
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void Damage(int amount){
        if(currentArmor > 0){
            currentArmor =  Mathf.Max(currentArmor - amount, 0);
        } else {
            currentHealth = Mathf.Max(currentHealth - amount, 0);
        }
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
