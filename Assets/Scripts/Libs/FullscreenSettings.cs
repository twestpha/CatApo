using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenSettings {
    public float fadeDuration = 0.0f;
    public float sunIntensity = 0.0f;
    public float ambientIntensity = 0.0f;
    public float reflectionIntensity = 0.0f;
    public float playerLightIntensity = 0.0f;
    public float scatteringCoeffecient = 0.0f;
    public float bloom = 0.0f;

    public float LerpSunIntensity(FullscreenSettings other, float t){
        return sunIntensity + (other.sunIntensity - sunIntensity) * t;
    }

    public float LerpAmbientIntensity(FullscreenSettings other, float t){
        return ambientIntensity + (other.ambientIntensity - ambientIntensity) * t;
    }

    public float LerpReflectionIntensity(FullscreenSettings other, float t){
        return reflectionIntensity + (other.reflectionIntensity - reflectionIntensity) * t;
    }

    public float LerpPlayerLightIntensity(FullscreenSettings other, float t){
        return playerLightIntensity + (other.playerLightIntensity - playerLightIntensity) * t;
    }

    public float LerpScatteringCoeffecient(FullscreenSettings other, float t){
        return scatteringCoeffecient + (other.scatteringCoeffecient - scatteringCoeffecient) * t;
    }

    public float LerpBloom(FullscreenSettings other, float t){
        return bloom + (other.bloom - bloom) * t;
    }
}
