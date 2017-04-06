using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class AbilityPlacement : ScriptableObject {

    private const float splatHeight = 15.0f;
    private const int maxTargets = 16;

    public enum PlacementType {
        Passive,       // Benefits player without activations
    ONHOTKEY, // Placements below require a button press
        Radius,        // Ability affects things in a radius around the hero
        Direction,     // Ability is aimed where hero is already facing
        Autocast,      // Toggleable to enhance or modify other attacks
        Self,          // effects self only
    ONCLICK, // Placements below require a button press, followed by a click to confirm
        Target,        // Must click on enemy hero or unit
        Skillshot,     // Click on terrain and spell is cast that direction from hero
        Location,      // Place spell where effects occur around it
        ConstrainedLocation // Place spell, limited distance from player
        Summon,        // create creature or persistent actor
    }

    private Ability parent;
    // This should be it's own fucking thing or at least descended from placement... fucking confusing
    public PlacementType type;
    public GameObject splat;
    private Projector splatProjector;
    // how to describe arbitrary shape? need some type. Use radius for now...
    public float splatSize;

    // Some sort of filtering targets

    public void Start(){
        splat = Object.Instantiate(splat);
        splatProjector = splat.GetComponent<Projector>();
        splatProjector.enabled = false;
        splatProjector.orthographicSize	= splatSize;
    }

    public void SetParent(Ability ability){
        parent = ability;
    }

    public void Update(){
        switch(type){
        // Location placement
        case PlacementType.Location:
            Vector3 mousePosition = MousePositionOnPlayerPlane();
            splat.transform.position = new Vector3(mousePosition.x, splatHeight, mousePosition.z);

            if(parent.state == Ability.AbilityState.Notified){
                splatProjector.enabled = true;
            } else if(parent.state == Ability.AbilityState.Casted){
                splatProjector.enabled = false;
            }
        break;
        default:
        break;
        }
    }

    public Actor[] GetTargetsInCast(){
        Actor[] targets = new Actor[1];

        switch(type){
        // Location placement
        case PlacementType.Location:
            targets[0] = parent.selfActor;
        break;
        default:
        break;
        }

        return targets;
    }

    private Vector3 MousePositionOnPlayerPlane(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Fix when player is implemented properly
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if(plane.Raycast(ray, out rayDistance)){
            return ray.GetPoint(rayDistance);
        }

        Debug.LogError("Intersection with player plane has failed unexpectedly.");
        return Vector3.zero;
    }
};
