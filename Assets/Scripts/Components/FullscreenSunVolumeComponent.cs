using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Collider))]
public class FullscreenSunVolumeComponent : MonoBehaviour {

	private FullscreenSunComponent sun;

    [Header("Fullscreen Settings on Trigger Enter")]
    public float fadeDuration = 1.0f;
    public float sunIntensity = 1.0f;
    public float ambientIntensity = 0.0f;
    public float reflectionIntensity = 1.0f;
    public float playerLightIntensity = 1.0f;

	void Start(){
        sun = GameObject.FindWithTag("Sun").GetComponent<FullscreenSunComponent>();
	}

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            sun.ApplyFullscreenSettings(fadeDuration, sunIntensity, ambientIntensity, reflectionIntensity, playerLightIntensity);
        }
    }

}
