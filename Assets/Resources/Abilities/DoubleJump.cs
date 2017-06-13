using UnityEngine;

public class DoubleJump : Ability {

    float jumpSpeed;
    static int jumpTimes;

    override protected void Setup(){
        type = AbilityType.onHotkey;
        cooldown = 10.0f;
        jumpSpeed = 14.0f;
    }

    override protected void AlwaysCast(){

    }

    override protected void SequentialCast(){
        if(GetGrounded(selfActor)){
            jumpTimes = 0;
        }

        if(!GetGrounded(selfActor) && jumpTimes > 1){
            return;
        }

        jumpTimes++;

        Vector3 finalVelocity = GetVelocity(selfActor);
        finalVelocity.y = jumpSpeed;
        SetVelocity(selfActor, finalVelocity);
    }
}
