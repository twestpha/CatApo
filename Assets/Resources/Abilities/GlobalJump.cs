using UnityEngine;

public class GlobalJump : Ability {

    float jumpSpeed;
    AudioClip jumpSound;

    override protected void Setup(){
        type = AbilityType.onHotkey;
        cooldown = 500.0f;
        jumpSpeed = 8.0f;
        jumpSound = Resources.Load("Sounds/lever_interact") as AudioClip;
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

        PlaySound(selfActor, jumpSound, false);

        Vector3 finalVelocity = GetVelocity(selfActor);
        finalVelocity.y = jumpSpeed;
        SetVelocity(selfActor, finalVelocity);
    }
}
