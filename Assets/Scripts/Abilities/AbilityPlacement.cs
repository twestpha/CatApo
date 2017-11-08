using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu()]
[System.Serializable]
public class AbilityPlacement : ScriptableObject {
    public bool debug;

    private const int maxTargets = 16;
    private const float playerPlaneOffset = -0.9f;

    private Actor caster;
    private Ability ability;
    public GameObject splat;
    private MeshRenderer meshRenderer;
    public float splatSize;

    public bool rotateFromCaster;
    public float minDistance = 0.0f;
    public float maxDistance = 0.0f;
    public Vector3 startPositionOffset = Vector3.zero;
    public Vector3 endPositionOffset = Vector3.zero;

    public void Start(){
        if(debug){
            Assert.IsTrue(splat, "Missing splat prefab");
            Assert.IsTrue(splat.GetComponent<SplatComponent>(), "Missing splat component on prefab " + splat);
        }

        splat = Object.Instantiate(splat);
        meshRenderer = splat.GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        splat.transform.localScale = new Vector3(splatSize, splatSize, splatSize);
    }

    public void SetCaster(Actor actor){
        caster = actor;
    }

    public void SetAbility(Ability parentability){
        ability = parentability;
    }

    public void Update(){
        // if we're a vector type placement, get vector from click down to mouse position
        // if we're not, get the vector from the actor position to the mouse position
        Vector3 origin = ability.type == Ability.AbilityType.vector ? ability.castClickDownPosition : caster.transform.position;
        Vector3 mouse = caster.AbilityTargetPoint();

        origin.y = 0.0f;
        mouse.y = 0.0f;

        Vector3 mouseDir = (mouse - origin);

        Vector3 startPosition = origin + startPositionOffset;
        Vector3 endPosition = mouse + endPositionOffset;

        // if we're not a vector, rotate to face mouse direction
        if(ability.type != Ability.AbilityType.vector){
            float angle = Mathf.Atan2(mouseDir.z, mouseDir.x);

            startPosition = new Vector3( startPositionOffset.z * Mathf.Cos(angle) - startPositionOffset.x * Mathf.Sin(angle),
                                        0.0f,
                                        startPositionOffset.x * Mathf.Cos(angle) + startPositionOffset.z * Mathf.Sin(angle));
            endPosition = new Vector3( endPositionOffset.z * Mathf.Cos(angle) - endPositionOffset.x * Mathf.Sin(angle),
                                       0.0f,
                                       endPositionOffset.x * Mathf.Cos(angle) + endPositionOffset.z * Mathf.Sin(angle));

            startPosition += origin;
            endPosition += mouse;

            mouseDir = (endPosition - startPosition);
        }

        // clamp magnitude
        float mouseDirMag = Mathf.Min(Mathf.Max(mouseDir.magnitude, minDistance), maxDistance);
        mouseDir.Normalize();

        if(rotateFromCaster){
            splat.transform.rotation = Quaternion.LookRotation(mouseDir) * Quaternion.Euler(90.0f, 0.0f, 0.0f);
        }

        Vector3 splatPos = startPosition + mouseDir * mouseDirMag;
        splatPos.y = caster.transform.position.y + playerPlaneOffset;
        splat.transform.position = splatPos;
    }

    public void SetEnabled(bool enabled){
        meshRenderer.enabled = enabled;
    }
};
