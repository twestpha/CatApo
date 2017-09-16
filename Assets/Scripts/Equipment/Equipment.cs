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
        Armor,
    };

    [Header("Equipment Type")]
    public EquipmentType type;

    [Header("Item Models")]
    public GameObject modelPrefab;
    public string attachJoint;
    public Vector3 translateoffset;
    public Vector3 rotationoffset;
    public Material characterMaterial;
    public bool hidesHair;

    [Header("Casting Objects")]
    public GameObject spawnablePrefab;
    public GameObject effectsPrefab;

    // [Header("Equipment Ability Effects")]
    // public List<AbilityEffect> effects;
    // public List<float> effectsTiming;
    // private int effectsIndex;
}
