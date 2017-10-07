using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class BotComponent : Actor {

    [Header("BOT COMPONENT")]
    // Temp stuff
    public GameObject player;


    public bool foundGoal;

    public Vector3 goal;

    private List<Vector3> waypoints;
    private int maxWaypoints = 8;
    private int lookbackDistance = 3;

	new void Start(){
        base.Start();
        goal = player.transform.position;
        foundGoal = true;
        waypoints = new List<Vector3>();

        waypoints.Add(transform.position);
        lookbackDistance = 3;
	}

	new void Update(){
        base.Update();

        goal = player.transform.position;

        UpdatePath();
        SetTargetAlongPath();
	}

    void UpdatePath(){
        // First, build the line
        Vector3 startposition = waypoints[waypoints.Count-1];
        Vector3 segment = goal - startposition;

        if(Physics.Raycast(startposition, segment, segment.magnitude, kTerrainCollisionMask) && waypoints.Count < maxWaypoints){
            waypoints.Add(goal);
        }

        Debug.DrawRay(startposition, segment, Color.blue);
        for(int i = 1; i < waypoints.Count; ++i){
            Debug.DrawRay(waypoints[i-1], waypoints[i] - waypoints[i-1], Color.red);
        }

        // See if we can unbreak the line by looking back
        if(waypoints.Count > 1){
            for(int i = 0; i < lookbackDistance && i < waypoints.Count - 1; ++i){
                startposition = waypoints[waypoints.Count-2-i];
                segment = goal - startposition;
                Debug.DrawRay(startposition, segment, Color.green);

                if(!Physics.Raycast(startposition, segment, segment.magnitude, kTerrainCollisionMask)){
                    for(int j = 0; j < i + 1; ++j){
                        waypoints.RemoveAt(waypoints.Count-1);
                    }
                    break;
                }
            }
        }

        // see if we can nullify the line entirely by getting simple los from self to goal
        if(waypoints.Count > 3){
            startposition = transform.position;
            segment = goal - startposition;

            Debug.DrawRay(startposition, segment, Color.yellow);
            if(!Physics.Raycast(startposition, segment, segment.magnitude, kTerrainCollisionMask)){
                waypoints.Clear();
                waypoints.Add(transform.position);
            }
        }
    }

    void SetTargetAlongPath(){

    }
}
