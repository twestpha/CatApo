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

    public int bodySlots = 0;

    // fixed size array of equipment/magic slots in the backpack
    public Castable[] backpackCastables = new Castable[8];

    // fixed size for castables currently equipped.
    // The ability cast component will take care of the ability effects,
    // but this will track "equipped" on the ability bar, and move it back and
    // forth between the backpack and equipped
    public Castable[] equippedCastables = new Castable[4];
    private GameObject[] castableModels = new GameObject[4];

    private GameObject playerModel;

    private int materialSlot;

    [Header("Default Material")]
    public Material defaultMaterial;

    private Timer debugTimer;

    void Start(){
        playerModel = GameObject.FindWithTag("PlayerModel");
        if(!playerModel){
            Debug.LogError("CANNOT FIND PLAYER MODEL");
        }

        for(int i = 0; i < backpackCastables.Length; ++i){
            if(backpackCastables[i]){
                backpackCastables[i] = Instantiate(backpackCastables[i]);
            }
        }

        // debug shit for me :D
        // Equip(backpackCastables[0], 0);
        // debugTimer = new Timer(5.0f);
        // debugTimer.Start();
    }

    void Update(){
        // if(debugTimer.Finished()){
        //     Unequip(0);
        // }
    }

    public void Equip(Castable castable, int slot){
        if(castable.GetType() == typeof(Equipment)){
            Equip((Equipment) castable, slot);
        }
    }

    public void Unequip(int slot){
        if(equippedCastables[slot].GetType() == typeof(Equipment)){
            UnequipEquipment(slot);
        }
    }

    private void Equip(Equipment equipment, int slot){
        equippedCastables[slot] = equipment;

        // create the model, attach to player
        castableModels[slot] = Instantiate(equipment.modelPrefab);
        castableModels[slot].transform.position = playerModel.transform.position + equipment.translateoffset;
        castableModels[slot].transform.rotation = playerModel.transform.rotation * Quaternion.Euler(equipment.rotationoffset);
        castableModels[slot].transform.parent = playerModel.transform;

        // TODO attach model to bones

        // create material and attach to player
        SkinnedMeshRenderer playermesh = playerModel.GetComponent<SkinnedMeshRenderer>();
        if(equipment.characterMaterial){
            playermesh.material = equipment.characterMaterial;
            materialSlot = slot;
        }
    }

    private void UnequipEquipment(int slot){
        Destroy(castableModels[slot]);

        // remove player material if we're using it
        if(materialSlot == slot){
            SkinnedMeshRenderer playermesh = playerModel.GetComponent<SkinnedMeshRenderer>();
            playermesh.material = defaultMaterial;
        }
    }
}
