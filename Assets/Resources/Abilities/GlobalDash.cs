using UnityEngine;

public class GlobalDash : Ability {

    private float dashSpeed;
    private float dashTime;

    override protected void Setup(){
        type = AbilityType.onHotkey;
        cooldown = 10150.0f;

        dashSpeed = 30.0f;
        dashTime = 150.0f;
    }

    override protected void AlwaysCast(){

    }

    override protected void SequentialCast(){
        if(!GetGrounded(selfActor)){
            End();
            return;
        }

        Vector3 oldTarget = GetTarget(selfActor);
        Vector3 oldPosition = GetPosition(selfActor);
        Vector3 dashDirection = oldTarget - oldPosition;
        dashDirection.Normalize();

        SetSteerable(selfActor, false);

        SetTarget(selfActor, oldPosition + dashDirection * 100.0f);
        SetSpeed(selfActor, GetSpeed(selfActor) + dashSpeed);

        Delay(dashTime);

        SetSpeed(selfActor, GetSpeed(selfActor) - dashSpeed);
        SetTarget(selfActor, GetPosition(selfActor) + dashDirection * 5.0f);

        SetSteerable(selfActor, true);

        Delay(cooldown - dashTime);

        End();
    }
}
