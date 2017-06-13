using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestClick : Ability {

    float jumpSpeed;

    override protected void Setup(){
        type = AbilityType.onClick;
        cooldown = 1000.0f;

        placementNames.Add("TestClick");
        placementNames.Add("TestClick2");
        placementNames.Add("TestClick3");
    }

    override protected void AlwaysCast(){
    }

    override protected void SequentialCast(){
        UnityEngine.Debug.Log("Casting TestClick");
    }
}
