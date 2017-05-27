using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class AbilityPlacement : ScriptableObject {
    public bool debug;

    private const float splatHeight = 15.0f;
    private const int maxTargets = 16;

    private Actor caster;
    private Ability parent;
    public GameObject splat;
    private Projector splatProjector;
    public float splatSize;

    [Header("Placement Details")]
    public AbilityVolume volume;
    public bool castOnlyOnSelf;
    public bool castOnActor; // really just would filter the closest actor and then keeps a reference probably
    public bool rotateFromCaster;
    public float maxDistance = 0.0f;
    public float minDistance = 0.0f;

    public void Start(){
        splat = Object.Instantiate(splat);
        splatProjector = splat.GetComponent<Projector>();
        splatProjector.enabled = false;
        splatProjector.orthographicSize	= splatSize;
    }

    public void SetParent(Ability ability){
        parent = ability;
    }

    public void SetCaster(Actor actor){
        caster = actor;
    }

    public void Update(){
        if(debug){
            Assert.IsTrue(splat, "Missing splat prefab");
            Assert.IsTrue(volume, "Missing volume data");
        }
        Vector3 mousePosition = caster.AbilityTargetPoint();
        Vector3 casterpos = caster.transform.position;
        mousePosition.y = 0.0f;
        casterpos.y = 0.0f;

        Vector3 mouseDir = (mousePosition - casterpos);

        float mouseDirMag = Mathf.Min(Mathf.Max(mouseDir.magnitude, minDistance), maxDistance);
        mouseDir.Normalize();

        Vector3 splatPos = mouseDir * mouseDirMag;
        splatPos.y = splatHeight;
        splat.transform.position = splatPos + casterpos;

        if(rotateFromCaster){
            splat.transform.rotation = Quaternion.LookRotation(mouseDir) * Quaternion.Euler(90.0f, 0.0f, 0.0f);
        }

        splatProjector.enabled = parent.state == Ability.AbilityState.Notified;
    }

    // FUCK THIS NOISE TODO
    // public Actor[] GetTargetsInCast(Vector3 castPosition){
    //     Actor[] targets = new Actor[maxTargets];
    //
    //     if(castOnlyOnSelf){
    //         targets[0] = caster;
    //         return targets;
    //     }
    //
    //     Actor[] actors = FindObjectsOfType(typeof(Actor)) as Actor[];
    //
    //
    //     // Set position and rotation of volume
    //     volume.SetPosition(splat.transform.position);
    //     if(rotateFromCaster){
    //         float rotation = (-splat.transform.rotation.eulerAngles.y + 90.0f) * Mathf.Deg2Rad;
    //         volume.SetRotation(rotation);
    //     }
    //
    //     int targetCount = 0;
    //     for(int i = 0; i < actors.Length && targetCount < maxTargets; ++i){
    //         Vector3 targetpos = actors[i].transform.position;
    //         if(volume.ContainsPoint(actors[i].transform.position)){
    //             targets[++targetCount] = actors[i];
    //         }
    //     }
    //
    //     return targets;
    // }

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
