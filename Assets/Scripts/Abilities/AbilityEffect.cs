using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class AbilityEffect : ScriptableObject { // Maybe generalize this for other operations...?
    // Locking a value, making sure it doesn't change
    // maybe create an actor/gameobject... so ability doesn't pick up that slack mostly because I don't like double lists

    // Ability Effect is the result of a cast ability. This is a sort of metaprogram

    public bool debug;

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
        default:
        return;
        }
    }

    static private void Assign(ref float left, ref float right, bool dir){
        if(dir){left = right;} else {right = left;}
    }

    // Non static
    public void Apply(Actor target){
        if(debug){
            Debug.Log("Casting [" + operation + " " + value + " " + attribute + "] on " + target);
        }

        // apply operation
        ApplyWithSettings(ref target, attribute, operation, value);
    }
}
