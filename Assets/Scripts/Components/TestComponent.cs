using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestComponent : MonoBehaviour {

    private Vector3 velocity;
    public float seektime;
    public float springConstant;
    public float mass;
    private Plane plane;

	void Start(){
        Debug.LogError("DO NOT USE THIS SCRIPT FOR ANYTHING EXCEPT TESTING.");
        plane = new Plane(Vector3.up, 0.0f);
	}

	void Update(){
        float newx = transform.position.x;
        float newz = transform.position.z;

        float newxvel = velocity.x;
        float newzvel = velocity.z;

        Vector3 mousePosition = IntersectionWithPlayerPlane();

        DampSpring(newx, mousePosition.x, ref newxvel);
        DampSpring(newz, mousePosition.z, ref newzvel);

        velocity = new Vector3(newxvel, velocity.y, newzvel);

        transform.position += velocity * Time.deltaTime;
	}

    // use a damped spring to move v0 towards target given a current velocity,
    // time over which the spring would cover 90% of the distance from rest;
    // and dt, the change in time.

    public void DampSpring(float x, float target, ref float vel){
        float delta = x - target;

        float w = Mathf.Sqrt(springConstant / mass);

        float acceleration = (-w * w * delta) - (2.0f * w * vel);
        vel += (Time.deltaTime * acceleration);
    }

    Vector3 IntersectionWithPlayerPlane(){;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayDistance;

        if(plane.Raycast(ray, out rayDistance)){
            return ray.GetPoint(rayDistance);
        }

        Debug.LogError("Intersection with player plane has failed unexpectedly.");
        return Vector3.zero;
    }
}
