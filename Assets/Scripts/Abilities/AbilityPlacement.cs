using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class AbilityPlacement : ScriptableObject {

    private const float splatHeight = 15.0f;
    private const int maxTargets = 16;

    public enum PlacementType {
        // Passive,       // Benefits caster without activations
        ONHOTKEY, // Placements below require a button press
        // Radius,        // Ability affects things in a radius around the hero
        // Direction,     // Ability is aimed where hero is already facing
        // Autocast,      // Toggleable to enhance or modify other attacks
        // Self,          // effects self only
        ONCLICK, // Placements below require a button press, followed by a click to confirm
        // Target,        // Must click on enemy hero or unit
        // Skillshot,     // Click on terrain and spell is cast that direction from hero
        // Location,      // Place spell where effects occur around it. Can be limited to a range
        // Summon,        // create creature or persistent actor
    }

    private Actor caster;
    private Ability parent;
    public PlacementType type;
    public GameObject splat;
    private Projector splatProjector;
    public float splatSize;

    [Header("Placement Details")]
    public AbilityVolume volume;
    public bool castOnActor; // really just filters to the closest actor and then keeps a reference probably
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
            Vector3 mousePosition = MousePositionOnPlayerPlane();
            Vector3 casterpos = caster.transform.position;
            mousePosition.y = 0.0f;
            casterpos.y = 0.0f;

            Vector3 mouseDir = (mousePosition - casterpos);

            float mouseDirMag = Mathf.Min(Mathf.Max(mouseDir.magnitude, minDistance), maxDistance);
            mouseDir.Normalize();

            Vector3 splatPos = mouseDir * mouseDirMag;
            splatPos.y = splatHeight;
            splat.transform.position = splatPos;

            if(rotateFromCaster){
                splat.transform.rotation = Quaternion.LookRotation(mouseDir) * Quaternion.Euler(90.0f, 0.0f, 0.0f);
            }

            splatProjector.enabled = parent.state == Ability.AbilityState.Notified;

        // switch(type){
        // // Skillshot placement
        // case PlacementType.Skillshot:{
        //     Vector3 mousePosition = MousePositionOnPlayerPlane();
        //     Vector3 casterpos = caster.transform.position;
        //     casterpos.y = 0.0f;
        //
        //     Vector3 mouseDirNorm = (mousePosition - casterpos);
        //     mouseDirNorm.Normalize();
        //
        //     Vector3 splatPos = mouseDirNorm * splatSize;
        //     splatPos.y = splatHeight;
        //     splat.transform.position = splatPos;
        //
        //     splat.transform.rotation = Quaternion.LookRotation(mouseDirNorm) * Quaternion.Euler(90.0f, 0.0f, 0.0f);
        //
        //     splatProjector.enabled = parent.state == Ability.AbilityState.Notified;
        // } break;
        // // Location placement
        // case PlacementType.Location:{
        //     Vector3 mousePosition = MousePositionOnPlayerPlane();
        //     splat.transform.position = new Vector3(mousePosition.x, splatHeight, mousePosition.z);
        //
        //     splatProjector.enabled = parent.state == Ability.AbilityState.Notified;
        // } break;
        // default:
        // break;
        // }
    }

    public Actor[] GetTargetsInCast(Vector3 castPosition){
        // Actor[] actors = FindObjectsOfType(typeof(Actor)) as Actor[];
        Actor[] targets = new Actor[maxTargets];
        // int targetCount = 0;
        //
        // switch(type){
        // // Skillshot placement
        // case PlacementType.Skillshot:{
        //     Vector3 skillShotDirection = (splat.transform.position - caster.transform.position);
        //     skillShotDirection.y = 0.0f;
        //     skillShotDirection.Normalize();
        //
        //     // some setup for ability space, like direction and range
        //
        //     for(int i = 0; i < actors.Length && targetCount < maxTargets; ++i){
        //         if(volume.ContainsPoint(actors[i].transform.position)){
        //             targets[++targetCount] = actors[i];
        //         }
        //     }
        // } break;
        // // Location placement
        // case PlacementType.Location:{
        //     for(int i = 0; i < actors.Length && targetCount < maxTargets; ++i){
        //         Vector3 targetpos = actors[i].transform.position;
        //
        //         volume.SetPosition(splat.transform.position);
        //
        //         if(volume.ContainsPoint(actors[i].transform.position)){
        //             targets[++targetCount] = actors[i];
        //         }
        //     }
        // } break;
        // default:
        // break;
        // }

        return targets;
    }

    private Vector3 MousePositionOnPlayerPlane(){
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
