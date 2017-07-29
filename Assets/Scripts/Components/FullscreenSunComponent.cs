using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;
using UnityEngine;

[RequireComponent (typeof (Light))]
[RequireComponent (typeof (VolumetricLight))]
public class FullscreenSunComponent : MonoBehaviour {

    private GameObject playerLight;
    private BloomOptimized cameraBloom;
    private VolumetricLight volumetricLight;

    private Timer fadeTimer;

    private FullscreenSettings newSettings;
    private FullscreenSettings oldSettings;

    private bool fading;

    void Start(){
        fadeTimer = new Timer();
        fading = false;

        playerLight = GameObject.FindWithTag("PlayerLight");
        volumetricLight = GetComponent<VolumetricLight>();
        cameraBloom = Camera.main.gameObject.GetComponent<BloomOptimized>();

        newSettings = new FullscreenSettings();
        newSettings.sunIntensity = GetComponent<Light>().intensity;
        newSettings.ambientIntensity = RenderSettings.ambientIntensity;
        newSettings.reflectionIntensity = RenderSettings.reflectionIntensity;
        newSettings.playerLightIntensity = playerLight.GetComponent<Light>().intensity;
        newSettings.scatteringCoeffecient = volumetricLight.ScatteringCoef;
        newSettings.bloom = cameraBloom.intensity;
    }

    void Update(){
        // TODO if performance from some of these effects gets too expensive, we should consider disabling them aggresively we can.

        if(fading){
            GetComponent<Light>().intensity = oldSettings.LerpSunIntensity(newSettings, fadeTimer.Parameterized());
            RenderSettings.ambientIntensity = oldSettings.LerpAmbientIntensity(newSettings, fadeTimer.Parameterized());
            RenderSettings.reflectionIntensity = oldSettings.LerpReflectionIntensity(newSettings, fadeTimer.Parameterized());

            float newPlayerLightIntensity = oldSettings.LerpPlayerLightIntensity(newSettings, fadeTimer.Parameterized());
            playerLight.GetComponent<Light>().intensity = newPlayerLightIntensity;

            volumetricLight.ScatteringCoef = oldSettings.LerpScatteringCoeffecient(newSettings, fadeTimer.Parameterized());
            cameraBloom.intensity = oldSettings.LerpBloom(newSettings, fadeTimer.Parameterized());

            if(newPlayerLightIntensity > 0.95f){
                playerLight.GetComponent<FireLightComponent>().Enable();
            } else if(newPlayerLightIntensity < 0.05f){
                playerLight.GetComponent<FireLightComponent>().Disable();
            }
        }

        fading = !fadeTimer.Finished();
    }

    public void ApplyFullscreenSettings(FullscreenSettings settings){
        fadeTimer.SetDuration(settings.fadeDuration);
        fading = true;

        // cache previous settings to lerp between
        oldSettings = newSettings;
        newSettings = settings;

        fadeTimer.Start();
    }
}
