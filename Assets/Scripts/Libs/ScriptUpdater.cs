using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScriptUpdater : MonoBehaviour {

    public Action actionQueue;

	void Start(){}

	void Update(){
        if(actionQueue != null){
            actionQueue();
            actionQueue = null;
        }
	}
}
