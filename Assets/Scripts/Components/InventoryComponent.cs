using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryComponent : MonoBehaviour {
    // Storage component for the character, which the ui will read

    // body slots, for types of items
    // prevents player wearing 2 helmets, etc.
    // spells don't use a body slot, only equipment
    // head, chest, on- and off-hands
    public enum BodySlot {
        None,
        Head      = 1 << 0,
        Torso     = 1 << 1,
        RightHand = 1 << 2,
        LeftHand  = 1 << 3,
    }

    public BodySlot bodySlots;

    // fixed size array of equipment/magic slots in the backpack
    public Castable[] backpackCastables = new Castable[8];

    // fixed size for castables currently equipped.
    // The ability cast component will take care of the ability effects,
    // but this will track "equipped" on the ability bar, and move it back and
    // forth between the backpack and equipped
    public Castable[] equippedCastables = new Castable[4];

    void Start(){

    }

    void Update(){

    }
}
