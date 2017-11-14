using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SyncVariable<T> {
        private bool dirty;
        private T value;

        public SyncVariable(){
            dirty = false;
        }

        public void SetValue(T value_){
            value = value_;
            dirty = true;
        }

        public T GetValue(){
            return value;
        }

        public T Synchronize(T reference){
            dirty = false;

            if(!dirty){
                value = reference;
            }

            return value;
        }
}
