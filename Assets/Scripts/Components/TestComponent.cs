﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestComponent : MonoBehaviour {
    // try to be a server for multi threaded update stuff

    public Action actionQueue;

	void Start(){

	}

	void Update(){
        // Debug.Log(actionQueue);
        if(actionQueue != null){
            actionQueue();
            actionQueue = null;
        }
	}
}
