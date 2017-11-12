using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimationComponent))]
public class PlayerComponent : Actor {

    // Constants
    private const float PlayerPlaneOffset = -1.0f;
    public const float InteractDistance = 4.0f;
    public const float TerrainLayerHeight = 5.0f;

    public enum UIState {
        InventoryHidden,
        InventoryOpen
    }

    [Header("Other Stuff Fix Me")]
    public bool usingTimer;

    private bool jumpUnreleased;

    public UIState uiState;

    private float moveTime = 0.3f;
    private Timer moveTimer;

    // Actor Plane
    protected Plane playerPlane;

    private GameObject playerUI;
    private GameObject playerModel;

    public GameObject movementMarker;


    [Header("Animation Poses")]
    public AnimationPose runPose0;
    public AnimationPose runPose1;
    public AnimationPose runPose2;
    public AnimationPose runPose3;

    public AnimationPose idlePose0;

    public AnimationPose jumpPose0;
    public AnimationPose jumpPose1;

    public AnimationPose dashPose0;
    public AnimationPose dashPose1;

    private AnimationComponent animationComponent;

    public enum AnimationState {
        Idling,
        Moving,
        Jumping,
        Dashing,
    };

    public AnimationState animState;

	new void Start(){
        base.Start();

        // Setting initial states
        usingTimer = false;
        moveTimer = new Timer(moveTime);

        // Setup actor plane
        playerPlane = new Plane(Vector3.down, characterController.transform.position.y + PlayerPlaneOffset);

        playerUI = GameObject.FindWithTag("PlayerUI");
        playerModel = GameObject.FindWithTag("PlayerModel");

        animState = AnimationState.Idling;
        animationComponent = GetComponent<AnimationComponent>();
	}

    new void Update(){
        base.Update();

        HandleInputs();
        HandleMove();
        HandleAnimation();
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
                    if(!usingTimer){
                        usingTimer = true;
                        moveTimer.Start();
                    }
                    targetPosition = terrainIntersection;

                    movementMarker.GetComponent<MovementMarkerComponent>().ShowMovementMarker();
                }
            }

            // if released before timer expires, continue moving. if released after, stop.
            if(usingTimer && moveTimer.Finished()){
                if(!rmbdown){
                    usingTimer = false;
                    targetPosition = transform.position;
                }
            } else if(!rmbdown){
                usingTimer = false;
            }

            // Interact clicking
            if(Input.GetMouseButtonDown(0)){
                MouseIntersectionWithInteract();
            }
        } else {

        }
    }

    public void HandleAnimation(){
        AnimationState prevState = animState;

        if(!characterController.isGrounded){
            animState = AnimationState.Jumping;
        } else if(moving){
             if(dashing){
                animState = AnimationState.Dashing;
            } else {
                animState = AnimationState.Moving;
            }
        } else {
            animState = AnimationState.Idling;
        }

        if(prevState != animState){
            switch(animState){
            case AnimationState.Idling:
                animationComponent.RequestAnimation(idlePose0, AnimationComponent.CurveType.Linear, 0.3f, true, false);
            break;
            case AnimationState.Moving:
                animationComponent.RequestAnimation(runPose0, AnimationComponent.CurveType.EaseIn, 0.15f, true, true);
                animationComponent.RequestAnimation(runPose1, AnimationComponent.CurveType.EaseOut, 0.2f, false, true);
                animationComponent.RequestAnimation(runPose2, AnimationComponent.CurveType.EaseIn, 0.15f, false, true);
                animationComponent.RequestAnimation(runPose3, AnimationComponent.CurveType.EaseOut, 0.2f, false, true);
            break;
            case AnimationState.Jumping:
                animationComponent.RequestAnimation(jumpPose0, AnimationComponent.CurveType.EaseInOut, 0.3f, true, true);
                animationComponent.RequestAnimation(jumpPose1, AnimationComponent.CurveType.EaseInOut, 0.3f, false, true);
            break;
            case AnimationState.Dashing:
                animationComponent.RequestAnimation(dashPose0, AnimationComponent.CurveType.Linear, 0.05f, true, false);
                animationComponent.RequestAnimation(dashPose1, AnimationComponent.CurveType.Linear, 0.05f, false, false);
            break;
            }
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
