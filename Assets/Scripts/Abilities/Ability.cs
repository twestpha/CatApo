using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject {
    // Maybe placement should be a composition :D
    /*
    Target    - Must click on enemy hero or unit
    Skillshot - Click on terrain and spell is cast that direction from hero
    Passive   - Benefits player without activations
    Radius    - Ability affects things in a radius around the hero
    Direction - Ability is aimed where hero is already facing
    Autocast  - Toggleable to enhance or modify other attacks
    Self      - effects self only
    Location  - Place spell where effects occur around it
    Summon    - create creature or persistent actor
    */

    public enum AbilitySource {
        Talent,
        Equipment,
    }

    protected Actor selfActor;
    public string abilityName;
    public AbilitySource source;
    public float cooldown;
    public List<AbilityEffect> effects;

    // I think, only things that are commong to all abilities go here
    // splat
    // created gameobject
    // cooldown
    // equipment list

    // Default Methods
    virtual public void Start(){

    }

    virtual public void Update(){

    }

    virtual public void LateUpdate(){

    }

    virtual public void Validate(){

    }

    virtual public void Cast(){
        Debug.LogError("Ability not implemented");
    }

    // Non virtual methods
    public void SetSelfActor(Actor actor){
        selfActor = actor;
    }
}
