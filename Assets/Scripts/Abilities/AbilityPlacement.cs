using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class AbilityPlacement : ScriptableObject {

    private const float splatHeight = 15.0f;
    private const int maxTargets = 16;

    // Old placement types
    // Passive,       // Benefits caster without activations
    // Radius,        // Ability affects things in a radius around the hero
    // Direction,     // Ability is aimed where hero is already facing
    // Autocast,      // Toggleable to enhance or modify other attacks
    // Self,          // effects self only
    // Target,        // Must click on enemy hero or unit
    // Skillshot,     // Click on terrain and spell is cast that direction from hero
    // Location,      // Place spell where effects occur around it. Can be limited to a range
    // Summon,        // create creature or persistent actor

    public enum PlacementType {
        onHotkey, // Placements below require a button press
        onClick, // Placements below require a button press, followed by a click to confirm
    }

    private Actor caster;
    private Ability parent;
    public PlacementType type;
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
        Vector3 mousePosition = caster.MouseTarget();
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

    public Actor[] GetTargetsInCast(Vector3 castPosition){
        Actor[] targets = new Actor[maxTargets];

        if(castOnlyOnSelf){
            targets[0] = caster;
            return targets;
        }

        Actor[] actors = FindObjectsOfType(typeof(Actor)) as Actor[];


        // Set position and rotation of volume
        volume.SetPosition(splat.transform.position);
        if(rotateFromCaster){
            float rotation = (-splat.transform.rotation.eulerAngles.y + 90.0f) * Mathf.Deg2Rad;
            volume.SetRotation(rotation);
        }

        int targetCount = 0;
        for(int i = 0; i < actors.Length && targetCount < maxTargets; ++i){
            Vector3 targetpos = actors[i].transform.position;
            if(volume.ContainsPoint(actors[i].transform.position)){
                targets[++targetCount] = actors[i];
            }
        }

        return targets;
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
