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

    private bool fading;

    void Start(){
        fadeTimer = new Timer();
        playerLight = GameObject.FindWithTag("PlayerLight");
        fading = false;
    }

    void Update(){
        if(fading){
            GetComponent<Light>().intensity = previousSunIntensity + ((sunIntensity - previousSunIntensity)*fadeTimer.Parameterized());
            RenderSettings.ambientIntensity = previousAmbientIntensity + ((ambientIntensity - previousAmbientIntensity)*fadeTimer.Parameterized());
            RenderSettings.reflectionIntensity = previousReflectionIntensity + ((reflectionIntensity - previousReflectionIntensity)*fadeTimer.Parameterized());
            float newPlayerLightIntensity = previousPlayerLightIntensity + ((playerLightIntensity - previousPlayerLightIntensity)*fadeTimer.Parameterized());
            playerLight.GetComponent<Light>().intensity = newPlayerLightIntensity;

            if(newPlayerLightIntensity > 0.95f){
                playerLight.GetComponent<FireLightComponent>().Enable();
            } else if(newPlayerLightIntensity < 0.05f){
                playerLight.GetComponent<FireLightComponent>().Disable();
            }
        }

        fading = !fadeTimer.Finished();
    }

    public void ApplyFullscreenSettings(float fadeDuration, float sunIntensity_, float ambientIntensity_, float reflectionIntensity_, float playerLightIntensity_){
        // Debug.Log("Applying Fullscreen Settings: " + sunIntensity_ + " " + ambientIntensity_ + " " + reflectionIntensity_ + " " + playerLightIntensity_);

        fadeTimer.SetDuration(fadeDuration);
        fading = true;

        // cache previous settings to lerp between
        previousSunIntensity = GetComponent<Light>().intensity;
        previousAmbientIntensity = RenderSettings.ambientIntensity;
        previousReflectionIntensity = RenderSettings.reflectionIntensity;
        previousPlayerLightIntensity = playerLight.GetComponent<Light>().intensity;

        // save new values
        sunIntensity = sunIntensity_;
        ambientIntensity = ambientIntensity_;
        reflectionIntensity = reflectionIntensity_;
        playerLightIntensity = playerLightIntensity_;

        fadeTimer.Start();
    }
}
