using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEffect : ScriptableObject {
    // Ability Effect is the result of a cast ability. This is a sort of metaprogram
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

public class AbilityEffectAttribute : AbilityEffect {

    public enum Attribute {
        Health,
        Speed,
        Ability0Cooldown,
        Ability1Cooldown,
        Ability2Cooldown,
        PositionX,
        PositionZ,
        PositionY,
        VelocityY,
        Grounded,
    }

    static protected void WriteAttribute(ref Actor actor, Attribute attribute, ref float v){
        switch(attribute){
        case Attribute.Health:
            actor.currentHealth = v;
        return;
        case Attribute.Ability0Cooldown:
            actor.GetComponent<AbilityCastComponent>().abilities[0].SetCooldownElapsed(v);
        return;
        case Attribute.VelocityY:
            actor.velocity.y = v;
        return;
        default:
            Debug.LogWarning("Can't write attribute " + attribute);
        return;
        }
    }

    static protected void ReadAttribute(ref Actor actor, Attribute attribute, ref float v){
        switch(attribute){
        case Attribute.Health:
            v = actor.currentHealth;
        return;
        case Attribute.Ability0Cooldown:
            v = actor.GetComponent<AbilityCastComponent>().abilities[0].CooldownElapsed();
        return;
        case Attribute.VelocityY:
            v = actor.velocity.y;
        return;
        case Attribute.Grounded:
            v = actor.GetComponent<CharacterController>().isGrounded ? 1.0f : 0.0f;
        return;
        default:
            Debug.LogWarning("Can't read attribute " + attribute);
        return;
        }
    }

    static protected void Assign(ref float left, ref float right, bool dir){
        if(dir){left = right;} else {right = left;}
    }
}

[CreateAssetMenu()]
[System.Serializable]
public class AbilityEffectConditional : AbilityEffectAttribute {
    public enum Operation {
        Equal,
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        And,
        Or,
    }

    public Attribute attribute;
    public Operation operation;
    public float value;

    public AbilityEffect trueEffect;
    public AbilityEffect falseEffect;

    override public void Apply(Actor target){
        float attributeValue = 0.0f;
        ReadAttribute(ref target, attribute, ref attributeValue);

        bool condition = false;

        switch(operation){
        case Operation.Equal:
            condition = attributeValue == value;
        break;
        case Operation.NotEqual:
            condition = attributeValue != value;
        break;
        case Operation.GreaterThan:
            condition = attributeValue > value;
        break;
        case Operation.GreaterThanOrEqual:
            condition = attributeValue >= value;
        break;
        case Operation.LessThan:
            condition = attributeValue < value;
        break;
        case Operation.LessThanOrEqual:
            condition = attributeValue <= value;
        break;
        case Operation.And:
            condition = (attributeValue * value) > 0.5f;
        break;
        case Operation.Or:
            condition = (attributeValue + value) > 0.5f;
        break;
        default:
        break;
        }

        if(condition){
            trueEffect.Apply(target);
        } else {
            if(falseEffect){
                falseEffect.Apply(target);
            }
        }
    }
}

[CreateAssetMenu()]
[System.Serializable]
public class AbilityEffectInputConditional : AbilityEffect {
    public enum Operation {
        Key,
        KeyUp,
        KeyDown,
    }

    public KeyCode keycode;
    public Operation operation;

    public AbilityEffect trueEffect;
    public AbilityEffect falseEffect;

    override public void Apply(Actor target){
        bool condition = false;

        switch(operation){
        case Operation.Key:
            condition = Input.GetKey(keycode);
        break;
        case Operation.KeyUp:
            condition = Input.GetKeyUp(keycode);
        break;
        case Operation.KeyDown:
            condition = Input.GetKeyDown(keycode);
        break;
        default:
        break;
        }

        if(condition){
            trueEffect.Apply(target);
        } else {
            if(falseEffect){
                falseEffect.Apply(target);
            }
        }
    }
}

[CreateAssetMenu()]
[System.Serializable]
public class AbilityEffectModifyAttribute : AbilityEffectAttribute {
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
        ReadAttribute(ref target_, attribute_, ref previousValue);

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
        WriteAttribute(ref target_, attribute_, ref finalValue);
    }

    override public void Apply(Actor target){
        if(debug){
            Debug.Log(operation + " " + value + " " + attribute + " to " + target);
        }

        // apply operation
        ApplyWithSettings(ref target, attribute, operation, value);
    }
}
