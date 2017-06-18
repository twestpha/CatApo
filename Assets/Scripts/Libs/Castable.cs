using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castable : ScriptableObject {

    public enum Rarity {
        Common,
        Rare,
        Legendary,
        Singular,
    };

    [Header("Equipment Information")]
    public string title;
    public string description;
    public Rarity rarity;
    public Sprite icon;

}
