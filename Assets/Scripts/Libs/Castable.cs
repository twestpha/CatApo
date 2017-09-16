using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castable : ScriptableObject {
    // Any "item" able to be equipped and "cast" as an ability (even if it's passive)

    public enum Rarity {
        Common,
        Rare,
        Legendary,
        Singular,
    };

    [Header("Base Castable Information")]
    public string debugName;
    public Rarity rarity;
    public Sprite icon;

}
