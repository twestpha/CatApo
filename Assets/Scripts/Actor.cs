using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

    [Header("Base Actor Properties")]
    public float maxHealth;
    public float currentHealth;
    public float moveSpeed;
    public float currentMoveSpeed;

	void Start(){
        currentHealth = maxHealth;
        currentMoveSpeed = moveSpeed;
	}

	void Update(){

	}
}
