using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

    [Header("Base Actor Properties")]

    [Header("Health")]
    public float maxHealth;
    public float currentHealth;

    [Header("Movement")]
    public float maxMoveSpeed;
    public float currentMoveSpeed;
    public float moveAcceleration;
    public float turnSpeed;

    // Hidden movement variables
    protected float accelerationMultiplier;
    protected float decelerationRadius;

    protected Vector3 targetPosition;
    protected Vector3 velocity;


	void Start(){
        currentHealth = maxHealth;
        currentMoveSpeed = maxMoveSpeed;
	}

	void Update(){

	}
}
