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
        ConstrainedLocation, // Place spell, limited distance from player
        Summon,        // create creature or persistent actor
    }

    private GameObject player;
    private Ability parent;
    public PlacementType type;
    public GameObject splat;
    private Projector splatProjector;
    // how to describe arbitrary shape? need some type. Use radius for now...
    // probably list of dimenions: 1-> radius, 2->oriented box, etc
    public float splatSize;

    public void Start(){
        splat = Object.Instantiate(splat);
        splatProjector = splat.GetComponent<Projector>();
        splatProjector.enabled = false;
        splatProjector.orthographicSize	= splatSize;

        player = GameObject.FindWithTag("Player");
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

            splatProjector.enabled = parent.state == Ability.AbilityState.Notified;
        break;
        default:
        break;
        }
    }

    public Actor[] GetTargetsInCast(Vector3 castPosition){
        Actor[] actors = FindObjectsOfType(typeof(Actor)) as Actor[];
        Actor[] targets = new Actor[maxTargets];
        int targetCount = 0;

        switch(type){
        // Location placement
        case PlacementType.Location:
            for(int i = 0; i < actors.Length && targetCount < maxTargets; ++i){
                Vector3 targetpos = actors[i].transform.position;

                targetpos.y = 0.0f;
                castPosition.y = 0.0f;

                if((targetpos - castPosition).magnitude <= splatSize){
                    targets[++targetCount] = actors[i];
                }
            }
        break;
        default:
        break;
        }

        return targets;
    }

    private Vector3 MousePositionOnPlayerPlane(){
        // Why the fuck is it misplacing the splat?
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 offset = new Vector3(0.0f, player.transform.position.y, 0.0f);
        Plane plane = new Plane(Vector3.up - offset, Vector3.zero);
        float rayDistance;

        if(plane.Raycast(ray, out rayDistance)){
            return ray.GetPoint(rayDistance);
        }

        Debug.LogError("Intersection with player plane has failed unexpectedly.");
        return Vector3.zero;
    }
};
