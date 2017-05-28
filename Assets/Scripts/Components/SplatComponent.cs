using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatComponent : MonoBehaviour {
    private List<Actor> actorsInTrigger;

    void Start(){
        actorsInTrigger = new List<Actor>();
        // TODO Add some assertions
        // has collider
        // collider is trigger
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.GetComponent<Actor>()){
            actorsInTrigger.Add(other.gameObject.GetComponent<Actor>());
        }
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.GetComponent<Actor>()){
            actorsInTrigger.Remove(other.gameObject.GetComponent<Actor>());
        }
    }

    public List<Actor> GetActorsInTrigger(){
        return actorsInTrigger;
    }
}
