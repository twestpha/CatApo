using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Light))]
public class FullscreenSunComponent : MonoBehaviour {

    private GameObject playerLight;

    private Timer fadeTimer;
    private float previousSunIntensity;
    private float previousAmbientIntensity;
    private float previousReflectionIntensity;
    private float previousPlayerLightIntensity;

    private float sunIntensity;
    private float ambientIntensity;
    private float reflectionIntensity;
    private float playerLightIntensity;

    void Start(){
        fadeTimer = new Timer();
        playerLight = GameObject.FindWithTag("PlayerLight");
    }

    void Update(){
        if(!fadeTimer.Finished()){
            GetComponent<Light>().intensity = previousSunIntensity + ((sunIntensity - previousSunIntensity)*fadeTimer.Parameterized());
            RenderSettings.ambientIntensity = previousAmbientIntensity + ((ambientIntensity - previousAmbientIntensity)*fadeTimer.Parameterized());
            RenderSettings.reflectionIntensity = previousReflectionIntensity + ((reflectionIntensity - previousReflectionIntensity)*fadeTimer.Parameterized());
            playerLight.GetComponent<Light>().intensity = previousPlayerLightIntensity + ((playerLightIntensity - previousPlayerLightIntensity)*fadeTimer.Parameterized());
        }
    }

    public void ApplyFullscreenSettings(float fadeDuration, float sunIntensity_, float ambientIntensity_, float reflectionIntensity_, float playerLightIntensity_){
        Debug.Log("Applying Fullscreen Settings: " + sunIntensity_ + " " + ambientIntensity_ + " " + reflectionIntensity_ + " " + playerLightIntensity_);

        fadeTimer.SetDuration(fadeDuration);

        // Cache previous settings to lerp between
        previousSunIntensity = GetComponent<Light>().intensity;
        previousAmbientIntensity = RenderSettings.ambientIntensity;
        previousReflectionIntensity = RenderSettings.reflectionIntensity;
        previousPlayerLightIntensity = playerLight.GetComponent<Light>().intensity;

        sunIntensity = sunIntensity_;
        ambientIntensity = ambientIntensity_;
        reflectionIntensity = reflectionIntensity_;
        playerLightIntensity = playerLightIntensity_;

        fadeTimer.Start();
    }
}
