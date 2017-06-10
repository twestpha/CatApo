using UnityEngine;

public class GlobalJump : Ability {

    float jumpSpeed;

    override public void Setup(){
        type = AbilityType.onHotkey;
        cooldown = 1000.0f;
        jumpSpeed = 14.0f;
    }

    override protected void AlwaysCast(){

    }

    override protected void SequentialCast(){
        if(!GetGrounded(selfActor)){
            return;
        }

        Vector3 finalVelocity = GetVelocity(selfActor);
        finalVelocity.y = jumpSpeed;
        SetVelocity(selfActor, finalVelocity);
    }
}
