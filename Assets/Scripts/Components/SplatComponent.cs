using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Collider))]
public class SplatComponent : MonoBehaviour {
    private List<Actor> actorsInTrigger;

    void Start(){
        actorsInTrigger = new List<Actor>();
        Assert.IsTrue(GetComponent<Collider>().isTrigger, gameObject + "'s collider isn't set as a trigger");
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
