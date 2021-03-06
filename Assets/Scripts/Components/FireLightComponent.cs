﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Light))]
public class FireLightComponent : MonoBehaviour {

    private Light lightComponent;

    private bool useFireLight = false;

	void Start(){
        lightComponent = GetComponent<Light>();
	}

	void Update(){
        if(useFireLight){
            float x = Time.time * 3.0f;
            float intensity = ((Mathf.Sin(x * 0.1f) + Mathf.Sin(x) + Mathf.Sin(x * 0.5f) + Mathf.Sin(x * 0.25f)) / 4.0f) + 2.0f;
            lightComponent.intensity = intensity;
        }
	}

    public void Enable(){
        useFireLight = true;
    }

    public void Disable(){
        useFireLight = false;
    }
}
