using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMultipleLayerComponent : MonoBehaviour {

    [Header("Show on Layer")]
    public bool layer7;
    public bool layer6;
    public bool layer5;
    public bool layer4;
    public bool layer3;
    public bool layer2;
    public bool layer1;
    public bool layer0;

    private PlayerComponent player;
    private int layermask;

    public List<GameObject> models;

    private bool prevenabled;

	void Start(){
        player = GameObject.FindWithTag("Player").GetComponent<PlayerComponent>();

        prevenabled = (player.GetPlayerTerrainLayer() & layermask) > 0;
        for(int i = 0; i < models.Count; ++i){
            models[i].GetComponent<MeshRenderer>().enabled = prevenabled;
        }

        layermask  = ((layer7 ? 1 : 0) << 7);
        layermask += ((layer6 ? 1 : 0) << 6);
        layermask += ((layer5 ? 1 : 0) << 5);
        layermask += ((layer4 ? 1 : 0) << 4);
        layermask += ((layer3 ? 1 : 0) << 3);
        layermask += ((layer2 ? 1 : 0) << 2);
        layermask += ((layer1 ? 1 : 0) << 1);
        layermask += ((layer0 ? 1 : 0) << 0);
	}

	void Update(){
        bool enabled = (player.GetPlayerTerrainLayer() & layermask) > 0;
        if(prevenabled != enabled){
            for(int i = 0; i < models.Count; ++i){
                models[i].GetComponent<MeshRenderer>().enabled = enabled;
            }
        }
        prevenabled = enabled;
	}
}
