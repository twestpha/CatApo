using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestFuture<T> {
        public bool ready;
        public T value;

        TestFuture(){
            ready = false;
        }
}

public class TestComponent : MonoBehaviour {
    // try to be a server for multi threaded update stuff

	void Start(){

	}

	void Update(){

	}
}
