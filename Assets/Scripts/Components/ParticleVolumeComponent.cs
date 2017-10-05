using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (ParticleSystem))]
[RequireComponent (typeof (BoxCollider))]
public class ParticleVolumeComponent : MonoBehaviour {

    private ParticleSystem particles;

	void Start(){
        particles = GetComponent<ParticleSystem>();
        particles.Pause();
	}

	void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            particles.Play();
        }
    }

    void OnTriggerExit(Collider other){
        if(other.tag == "Player"){
            particles.Pause();
        }
    }
}
