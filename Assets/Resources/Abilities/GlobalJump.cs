using UnityEngine;

public class GlobalJump : Ability {

    float jumpSpeed;

    override protected void Setup(){
        type = AbilityType.onHotkey;
        cooldown = 500.0f;
        jumpSpeed = 8.0f;
    }

    override protected void AlwaysCast(){
        if(GetGrounded(selfActor)){
            End();
        }
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
