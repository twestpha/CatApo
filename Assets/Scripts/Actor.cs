using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Actor : MonoBehaviour {

    // Constants
    protected const int kTerrainCollisionMask = 1 << 8;

    [Header("Base Actor Properties")]

    [Header("Health")]
    public float maxHealth;
    public float currentHealth;

    [Header("Movement")]
    public float maxMoveSpeed;
    public float currentMoveSpeed;
    public float moveAcceleration;
    public float turnSpeed;

    // Hidden movement variables
    protected float accelerationMultiplier;
    protected float decelerationRadius;

    protected Vector3 targetPosition;
    protected Vector3 velocity;

    // Component References
    protected CharacterController characterController;

    // Actor Plane
    protected Plane actorPlane;

    // fucking timers
    // man I should really make a timer helper class
    private float rootTime;
    private float currentRootTime;

	protected void Start(){
        currentHealth = maxHealth;
        currentMoveSpeed = maxMoveSpeed;

        characterController = GetComponent<CharacterController>();

        // Setup player plane
        actorPlane = new Plane(Vector3.down, characterController.transform.position.y/* + kPlayerPlaneOffset*/);
	}

	protected void Update(){
        currentRootTime += Time.deltaTime;

        if(currentRootTime > rootTime){
            currentMoveSpeed = maxMoveSpeed;
        }
	}

    //##########################################################################
    // Status methods
    //##########################################################################
    public void Root(float duration){
        // this is kind of a hack around the poor movement model we're using
        currentMoveSpeed = 0.0f;
        currentRootTime = 0.0f;
        targetPosition = transform.position;
        rootTime = duration;
    }

    //##########################################################################
    // Mouse-to-world helper functions
    //##########################################################################
    public Vector3 MouseTarget(){
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // RaycastHit hit;
        //
        // if(Physics.Raycast(ray, out hit, Mathf.Infinity, kTerrainCollisionMask)){
        //     return hit.point;
        // }

        return IntersectionWithPlayerPlane(); // As fallback, use plane player is on.
    }

    protected Vector3 IntersectionWithPlayerPlane(){
        actorPlane.distance = characterController.transform.position.y - 1.0f/*+ kPlayerPlaneOffset /* not sure about this bad boy yet */;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float rayDistance;

        if(actorPlane.Raycast(ray, out rayDistance)){
            return ray.GetPoint(rayDistance);
        }

        Debug.LogError("Intersection with player plane has failed unexpectedly.");
        return Vector3.zero;
    }
}
