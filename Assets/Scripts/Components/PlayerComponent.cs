using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : Actor {

    // Constants
    private const float PlayerPlaneOffset = -1.0f;
    public const float InteractDistance = 4.0f;
    public const float TerrainLayerHeight = 5.0f;

    public enum UIState {
        InventoryHidden,
        InventoryOpen
    }

    public bool usingtimer;

    private bool jumpUnreleased;
    public bool jumping;
    public bool dashing;

    public UIState uiState;

    private float moveTime = 0.3f;
    private Timer moveTimer;

    // Actor Plane
    protected Plane playerPlane;

    private GameObject playerUI;
    private GameObject playerModel;

    public GameObject movementMarker;

	new void Start(){
        base.Start();

        // Setting initial states
        usingtimer = false;
        moveTimer = new Timer(moveTime);

        // Setup actor plane
        playerPlane = new Plane(Vector3.down, characterController.transform.position.y + PlayerPlaneOffset);

        playerUI = GameObject.FindWithTag("PlayerUI");
        playerModel = GameObject.FindWithTag("PlayerModel");
	}

    new void Update(){
        base.Update();

        HandleInputs();
        HandleMove();
    }

    void FixedUpdate(){

    }

    override public void HandleInputs(){
        if(uiState == UIState.InventoryHidden){
            // Movement
            bool rmbdown = Input.GetButton("Fire2");

            if(rmbdown && steerable){
                Vector3 terrainIntersection = MouseIntersectionWithPlayerPlane();

                if((terrainIntersection - transform.position).magnitude >= kMoveDistanceNear){
                    if(!usingtimer){
                        usingtimer = true;
                        moveTimer.Start();
                    }
                    targetPosition = terrainIntersection;

                    movementMarker.GetComponent<MovementMarkerComponent>().ShowMovementMarker();
                }
            }

            // if released before timer expires, continue moving. if released after, stop.
            if(usingtimer && moveTimer.Finished()){
                if(!rmbdown){
                    usingtimer = false;
                    targetPosition = transform.position;
                }
            } else if(!rmbdown){
                usingtimer = false;
            }

            // Interact clicking
            if(Input.GetMouseButtonDown(0)){
                MouseIntersectionWithInteract();
            }
        } else {

        }
    }

    //##########################################################################
    // Mouse-to-world helper functions
    //##########################################################################
    override public Vector3 AbilityTargetPoint(){
        return MouseIntersectionWithPlayerPlane();
    }

    public void MouseIntersectionWithInteract(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, Interactable.InteractableCollisionMask)){
            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
            if(interactable && (interactable.gameObject.transform.position - transform.position).magnitude <= InteractDistance){
                interactable.NotifyClicked();
            }
        }
    }

    public Vector3 MouseIntersectionWithTerrain(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, kTerrainCollisionMask)){
            return hit.point;
        }

        // fallback
        return MouseIntersectionWithPlayerPlane();
    }

    protected Vector3 MouseIntersectionWithPlayerPlane(){
        playerPlane.distance = characterController.transform.position.y + PlayerPlaneOffset;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float rayDistance;

        if(playerPlane.Raycast(ray, out rayDistance)){
            return ray.GetPoint(rayDistance);
        }

        //Debug.LogError("Intersection with player plane has failed unexpectedly.");
        return Vector3.zero;
    }

    public int GetPlayerTerrainLayer(){
        int terrainHeight = (int)(transform.position.y / TerrainLayerHeight);
        terrainHeight = Mathf.Clamp(terrainHeight, 0, 7);
        return 1 << terrainHeight;
    }

    public GameObject GetPlayerModel(){
        return playerModel;
    }
}
