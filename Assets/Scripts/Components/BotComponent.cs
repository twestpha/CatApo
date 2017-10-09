using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotComponent : Actor {

    [Header("Bot Component")]
    private GameObject player;

    public bool foundGoal;

    public Vector3 goal;

    private List<Vector3> waypoints;
    private int maxWaypoints = 8;
    private int lookbackDistance = 3;

    public enum BotState {
        Pursuing,
        Engaging,
        Fleeing,
        Waiting
    };
    [Header("Bot State")]
    public BotState state;

    private Timer updateTimer;
    private float updateTimerDuration = 0.5f;

    [Header("Confidence Heuristic")]
    [Range(-1.0f, 1.0f)]
    public float confidence = 0.0f;
    public float healthTuning = 1.0f;
    public float armorTuning = 0.5f;
    public float playerMovementTuning = 300.0f;

    private Vector3 previousPlayerPosition;

	new void Start(){
        base.Start();
        player = GameObject.FindWithTag("Player");

        goal = player.transform.position;
        foundGoal = true;
        waypoints = new List<Vector3>();

        waypoints.Add(transform.position);
        lookbackDistance = 3;

        updateTimer = new Timer(updateTimerDuration);
	}

	new void Update(){
        base.Update();

        goal = player.transform.position;

        HandleMove();

        if(!updateTimer.Finished()){
            return;
        }

        updateTimer.Start();

        UpdateConfidence();

        if(state == BotState.Pursuing){
            UpdatePath();
            NavigateAlongPath();
        } else if(state == BotState.Engaging){

        } else if(state == BotState.Fleeing){
            NavigateFleeing();
        } else if(state == BotState.Waiting){
            targetPosition = transform.position;
        }

	}

    void UpdateConfidence(){
        // goes from -1 to 0 to 1
        // negative means scared, get away from the danger
        // positive means confident, attack
        // around 0 means hover around

        // Some variable setup
        Vector3 previousPlayerVector = transform.position - previousPlayerPosition;
        Vector3 playerVector = transform.position - player.transform.position;

        // for measuring how confident I am, how scared I am, how confident the player appears to be
        // confidence =
        //              + how much health I have
        //              + how much armor I have
        //              + is my dodge ability off cooldown
        //              + how many abilities do I have (off colldown)
        //              + did the player use an ability recently and miss
        //              + do I have buddies nearby
        //              + how close the player is (if i'm melee and players's not)
        //              - how close the player is (if he's melee and I'm not)
        //              - is the player moving aggressively towards me
        //              - health of the player

        float previousConfidence = confidence;

        confidence = 0; // TODO more confidence measurement and tuning
        // confidence += Mathf.Lerp(-1.0f, 1.0f, (float)currentHealth / (float)maxHealth) * healthTuning;
        // confidence += (float)currentArmor * armorTuning;
        confidence += ((playerVector.magnitude - previousPlayerVector.magnitude) * Time.deltaTime) * playerMovementTuning;

        // confidence moving average of two
        confidence = (confidence + previousConfidence) / 2.0f;

        // set state dependent on confidence
        if(confidence >= 0.25f){
            Vector3 segment = goal - transform.position;
            // Debug.DrawRay(transform.position, segment, Color.cyan);
            if(Physics.Raycast(transform.position, segment, segment.magnitude, kTerrainCollisionMask)){
                state = BotState.Pursuing;
            } else {
                state = BotState.Engaging;
            }
        } else if(confidence <= -0.25f){
            state = BotState.Fleeing;
        } else {
            state = BotState.Waiting;
        }

        // final variable cleanup
        previousPlayerPosition = player.transform.position;
    }

    void UpdatePath(){
        // First, build the line
        Vector3 startposition = waypoints[waypoints.Count-1];
        Vector3 segment = goal - startposition;

        if(Physics.Raycast(startposition, segment, segment.magnitude, kTerrainCollisionMask) && waypoints.Count < maxWaypoints){
            waypoints.Add(goal);
        }

        // Debug.DrawRay(startposition, segment, Color.blue);
        // for(int i = 1; i < waypoints.Count; ++i){
        //     Debug.DrawRay(waypoints[i-1], waypoints[i] - waypoints[i-1], Color.red);
        // }

        // See if we can unbreak the line by looking back
        if(waypoints.Count > 1){
            for(int i = 0; i < lookbackDistance && i < waypoints.Count - 1; ++i){
                startposition = waypoints[waypoints.Count-2-i];
                segment = goal - startposition;
                // Debug.DrawRay(startposition, segment, Color.green);

                if(!Physics.Raycast(startposition, segment, segment.magnitude, kTerrainCollisionMask)){
                    for(int j = 0; j < i + 1; ++j){
                        waypoints.RemoveAt(waypoints.Count-1);
                    }
                    break;
                }
            }
        }

        // see if we can nullify the line entirely by getting simple los from self to goal
        if(waypoints.Count > lookbackDistance){
            startposition = transform.position;
            segment = goal - startposition;

            // Debug.DrawRay(startposition, segment, Color.yellow);
            if(!Physics.Raycast(startposition, segment, segment.magnitude, kTerrainCollisionMask)){
                waypoints.Clear();
                waypoints.Add(transform.position);
            }
        }
    }

    void NavigateAlongPath(){

    }

    // Fleeing Navigation Attributes
    private const float fleeVectorScale = 6.0f;
    private const int fleeVectorCount = 9;
    void NavigateFleeing(){
        Vector3 fleeVector = -1.0f * (player.transform.position - transform.position);
        fleeVector.y = 0.0f;
        fleeVector.Normalize();
        fleeVector *= fleeVectorScale;

        List<Vector3> validPoints = new List<Vector3>();
        List<Vector3> invalidPoints = new List<Vector3>();

        fleeVector = Quaternion.AngleAxis(-90.0f, Vector3.up) * fleeVector;
        float degreesPerIncrement = 180.0f / ((float)fleeVectorCount - 1.0f);

        for(int i = 0; i < fleeVectorCount; ++i){
            Debug.DrawRay(transform.position, fleeVector, Color.red, updateTimerDuration);
            RaycastHit hit;
            if(!Physics.Raycast(transform.position, fleeVector, out hit, fleeVector.magnitude, kTerrainCollisionMask)){
                validPoints.Add(transform.position + fleeVector);
            } else {
                invalidPoints.Add(hit.point);
            }

            fleeVector = Quaternion.AngleAxis(degreesPerIncrement, Vector3.up) * fleeVector;
        }

        // if we have valid points, pick a random one from them, else pick the longest
        if(validPoints.Count > 0){
            targetPosition = validPoints[(int)Random.Range(0.0f, (float)validPoints.Count - 1.0f)];
        } else {
            Vector3 longest = new Vector3();

            for(int i = 0; i < invalidPoints.Count; ++i){
                if(invalidPoints[i].magnitude > longest.magnitude){
                    longest = invalidPoints[i];
                }
            }

            targetPosition = longest;
        }

        // Debug.DrawRay(transform.position, targetPosition - transform.position, Color.green, updateTimerDuration);
    }
}
