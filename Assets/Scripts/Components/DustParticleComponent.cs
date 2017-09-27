using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (ParticleSystem))]
public class DustParticleComponent : MonoBehaviour {

    public GameObject character;

    private bool wasGrounded = true;

    private ParticleSystem particles;

	void Start(){
        particles = GetComponent<ParticleSystem>();
	}

	void Update(){
        bool isGrounded = character.GetComponent<CharacterController>().isGrounded;

        if(isGrounded && !wasGrounded){
            particles.Play();
        }

        wasGrounded = isGrounded;
	}
}
