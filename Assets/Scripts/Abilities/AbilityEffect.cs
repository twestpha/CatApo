using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class AbilityEffect : ScriptableObject {
    // Ability Effect is the result of a cast ability. This is a sort of metaprogram

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
    }

    public enum Operation {
        None,
        Replace,
        Add,
        Subtract,
        Multiply,
        Divide,
    }

    public enum Behavior {
        Keep,
        Restore
    }

    private Actor targetActor;
    public Attribute attribute;
    public Operation operation;
    public Behavior behavior;
    public float value;
    public float duration;

    // Delayed coroutine
    private IEnumerator coroutine;

    // Implementation of the ability effects
    static public void ApplyWithSettings(ref Actor target_, Attribute attribute_, Operation operation_, Behavior behavior_, float value_, float duration_){
        // Get previous value
        float previousValue = 0.0f;
        AttributeAccessor(ref target_, attribute_, ref previousValue, AssignToRight);

        // setup the value using the given operation
        float finalValue;
        switch(operation_){
        case Operation.Subtract:
            finalValue = previousValue - value_; break;
        case Operation.Add:
            finalValue = previousValue + value_; break;
        case Operation.Multiply:
            finalValue = previousValue * value_; break;
        case Operation.Divide:
            finalValue = previousValue / value_; break;
        case Operation.Replace:
            finalValue = value_; break;
        default:
            return; // nop
        }

        // push that back to target value
        AttributeAccessor(ref target_, attribute_, ref finalValue, AssignToLeft);
    }

    static private void AttributeAccessor(ref Actor actor, Attribute attribute, ref float v, bool dir){
        switch(attribute){
        case Attribute.Health:
            Assign(ref actor.currentHealth, ref v, dir); return;
        default:
        return;
        }
    }

    static private void Assign(ref float left, ref float right, bool dir){
        if(dir){left = right;} else {right = left;}
    }

    // Non static
    public void Apply(Actor target){
        targetActor = target;

        ApplyWithSettings(ref target, attribute, operation, behavior, value, duration);

        if(behavior == Behavior.Restore){
            targetActor.StartCoroutine(RestoreAfterTime(duration));
        }
    }

    private IEnumerator RestoreAfterTime(float waitTime){
        yield return new WaitForSeconds(waitTime);

        // invert the operation
        Operation newop = Operation.Replace;
        switch(operation){
        case Operation.Subtract:
            newop = Operation.Add; break;
        case Operation.Add:
            newop = Operation.Subtract; break;
        case Operation.Multiply:
            newop = Operation.Divide; break;
        case Operation.Divide:
            newop = Operation.Multiply; break;
        default:
            Debug.LogError("Can't undo a \"Replace\" operation."); break;
        }

        ApplyWithSettings(ref targetActor, attribute, newop, Behavior.Keep, value, duration);
    }
}
