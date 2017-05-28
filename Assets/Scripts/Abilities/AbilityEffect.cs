using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEffect : ScriptableObject {
    // Ability Effect is the result of a cast ability. This is a sort of metaprogram
    // Locking a value, making sure it doesn't change
    // conditional ability effect, based on some state
    // ^ these two seem related to AbilityEffectModifyAttribute, maybe create some common base class there too

    public bool debug;

    virtual public void Apply(Actor target){}
}

[CreateAssetMenu()]
[System.Serializable]
public class AbilityEffectCreateObject : AbilityEffect {
    public GameObject createObject;
    public Vector3 translationOffset;
    public float rotationOffset;

    override public void Apply(Actor target){
        // bug... not shooting in direction of skillshot, but in direction of target facing
        if(debug){
            Debug.Log("Creating " + createObject + "at offset " + translationOffset + " and " + rotationOffset);
        }

        Object.Instantiate(
            createObject,
            target.transform.TransformPoint(translationOffset),
            target.transform.rotation * Quaternion.Euler(0.0f, rotationOffset, 0.0f)
        );
    }
}

[CreateAssetMenu()]
[System.Serializable]
public class AbilityEffectActorStatus : AbilityEffect {
    public enum ActorStatus {
        Stop,
        Root,
        Stun,
        Silence,
    };

    public ActorStatus status;
    public float duration;

    override public void Apply(Actor target){
        if(debug){
            Debug.Log(status + " " +  target + " for " + duration);
        }

        switch(status){
        case ActorStatus.Stop:
            target.Root(0.1f);
        break;
        }
    }
}

[CreateAssetMenu()]
[System.Serializable]
public class AbilityEffectModifyAttribute : AbilityEffect {
    // Readability replacements for accessor
    protected const bool AssignToLeft = true;
    protected const bool AssignToRight = false;

    // This have to match the entries in Actor, or at least have an get/set
    public enum Attribute {
        Health,
        Speed,
        Cooldown,
        State,
        PositionX,
        PositionZ,
        PositionY,
        VelocityY,
    }

    public enum Operation {
        None,
        Replace,
        Add,
        Subtract,
        Multiply,
        Divide,
    }

    public Attribute attribute;
    public Operation operation;
    public float value;

    // Implementation of the ability effects
    static public void ApplyWithSettings(ref Actor target_, Attribute attribute_, Operation operation_, float value_){

        // Get previous value
        float previousValue = 0.0f;
        AttributeAccessor(ref target_, attribute_, ref previousValue, AssignToRight);

        // setup the value using the given operation
        float finalValue;
        switch(operation_){
        case Operation.Subtract:
            finalValue = previousValue - value_;
        break;
        case Operation.Add:
            finalValue = previousValue + value_;
        break;
        case Operation.Multiply:
            finalValue = previousValue * value_;
        break;
        case Operation.Divide:
            finalValue = previousValue / value_;
        break;
        case Operation.Replace:
            finalValue = value_;
        break;
        default:
        return; // nop
        }

        // push that back to target value
        AttributeAccessor(ref target_, attribute_, ref finalValue, AssignToLeft);
    }

    static private void AttributeAccessor(ref Actor actor, Attribute attribute, ref float v, bool dir){
        switch(attribute){
        case Attribute.Health:
            Assign(ref actor.currentHealth, ref v, dir);
        return;
        case Attribute.VelocityY:
            Assign(ref actor.velocity.y, ref v, dir);
        return;
        default:
        return;
        }
    }

    static private void Assign(ref float left, ref float right, bool dir){
        if(dir){left = right;} else {right = left;}
    }

    override public void Apply(Actor target){
        if(debug){
            Debug.Log(operation + " " + value + " " + attribute + " to " + target);
        }

        // apply operation
        ApplyWithSettings(ref target, attribute, operation, value);
    }
}
