using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Actor : MonoBehaviour {

    // Constants
    protected const int kTerrainCollisionMask = 1 << 8;

    [Header("Base Actor Properties")]

    [Header("Health and Armor")]
    public int maxHealth;
    public int currentHealth;

    public int maxArmor;
    public int currentArmor;

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

        if(moveDistance > 0.1f){
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
