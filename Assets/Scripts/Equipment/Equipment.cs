using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class Equipment : ScriptableObject {
    // Equipment is the collection of model, icon, effects, summonables, and stats
    // for a piece of equipment in the game

    public bool debug;

    public enum EquipmentRarity {
        Common,
        Rare,
        Legendary,
        Singular,
    };

    public enum EquipmentType {
        Bow,
        Arrow,
        Sword,
        Shield,
    };

    [Header("Equipment Information")]
    public string equipmentName;
    public string description;
    public EquipmentRarity rarity;
    public EquipmentType type;

    [Header("Game Objects")]
    public GameObject weaponModel;
    public string attachJoint;
    public GameObject spawnableModel;

    // probably some references to icons

    [Header("Equipment Ability Effects")]
    public List<AbilityEffect> effects;
    public List<float> effectsTiming;
    private int effectsIndex;
}
