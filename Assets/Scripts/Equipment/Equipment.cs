using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class Equipment : Castable {
    // Equipment is the collection of model, icon, effects, summonables, and stats
    // for a piece of equipment in the game

    public bool debug;

    public enum EquipmentType {
        Bow, // If we're certain types (bow, etc) we should have a useConsumable accessor where the ability will then search for a consumable and cast it as well (on cast)
        Arrow,
        Sword,
        Shield,
    };


    public EquipmentType type;

    [Header("Game Objects")]
    public GameObject weaponModel;
    public string attachJoint;
    public GameObject spawnableModel;

    // probably some references to icons

    [Header("Equipment Ability Effects")]
    // public List<AbilityEffect> effects;
    // public List<float> effectsTiming;
    private int effectsIndex;
}
