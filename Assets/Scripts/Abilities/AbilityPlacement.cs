using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu()]
[System.Serializable]
public class AbilityPlacement : ScriptableObject {
    public bool debug;

    private const int maxTargets = 16;

    private Actor caster;
    private Ability parent;
    public GameObject splat;
    private SplatComponent splatComponent;
    private MeshRenderer meshRenderer;
    public float splatSize;

    // TODO ability filter at some point
    public bool castOnlyOnSelf;
    public bool castOnActor;

    [Header("Placement Details")]
    public bool rotateFromCaster;
    public float minDistance = 0.0f;
    public float maxDistance = 0.0f;
    public float angleOffset = 0.0f;
    public Vector3 positionOffset = Vector3.zero;

    public void Start(){
        if(debug){
            Assert.IsTrue(splat, "Missing splat prefab");
            Assert.IsTrue(splat.GetComponent<SplatComponent>(), "Missing splat component on prefab " + splat);
        }

        splat = Object.Instantiate(splat);
        splatComponent = splat.GetComponent<SplatComponent>();
        meshRenderer = splat.GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        splat.transform.localScale = new Vector3(splatSize, splatSize, splatSize);

        angleOffset = Mathf.Deg2Rad * angleOffset;
    }

    public void SetParent(Ability ability){
        parent = ability;
    }

    public void SetCaster(Actor actor){
        caster = actor;
    }

    public void Update(){
        meshRenderer.enabled = parent.state == Ability.AbilityState.Notified;

        // Get vector from caster -> mouse on zero plane
        Vector3 mousePosition = caster.AbilityTargetPoint();
        Vector3 casterpos = caster.transform.position;
        mousePosition.y = 0.0f;
        casterpos.y = 0.0f;

        Vector3 mouseDir = (mousePosition - casterpos);

        // clamp magnitude
        float mouseDirMag = Mathf.Min(Mathf.Max(mouseDir.magnitude, minDistance), maxDistance);
        mouseDir.Normalize();

        // apply angle offset to direction
        mouseDir = new Vector3(mouseDir.x * Mathf.Cos(angleOffset) - mouseDir.z * Mathf.Sin(angleOffset),
                               0.0f,
                               mouseDir.z * Mathf.Cos(angleOffset) + mouseDir.x * Mathf.Sin(angleOffset));

        Vector3 splatPos = mouseDir * mouseDirMag;
        splatPos.y = caster.transform.position.y - 1.0f; // TODO setup player offset
        splat.transform.position = splatPos + casterpos + positionOffset;

        if(rotateFromCaster){
            splat.transform.rotation = Quaternion.LookRotation(mouseDir) * Quaternion.Euler(90.0f, 0.0f, 0.0f);
        }
    }

    public List<Actor> GetTargetsInCast(){
        List<Actor> actorsInCast = splatComponent.GetActorsInTrigger();

        // TODO add filtering step

        return actorsInCast;
    }

    private Vector3 MousePositionOnCasterPlane(){
        // Fix when caster starts working proper
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if(plane.Raycast(ray, out rayDistance)){
            return ray.GetPoint(rayDistance);
        }

        Debug.LogError("Intersection with caster plane has failed unexpectedly.");
        return Vector3.zero;
    }
};
