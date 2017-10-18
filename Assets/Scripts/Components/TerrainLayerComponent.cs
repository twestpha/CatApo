using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainLayerComponent : MonoBehaviour {

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
    private MeshRenderer mesh;
    private Terrain terrain;
    private int layermask;
    private bool prevenabled;

	void Start(){
        player = GameObject.FindWithTag("Player").GetComponent<PlayerComponent>();
        mesh = GetComponent<MeshRenderer>();
        terrain = GetComponent<Terrain>();

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
        if(enabled != prevenabled){
            if(mesh){
                mesh.enabled = enabled;
            }

            if(gameObject.layer == Interactable.InteractableCollisionMask &&
                gameObject.GetComponent<Interactable>()){
                gameObject.GetComponent<Interactable>().Toggle();
            }

            if(terrain){
                terrain.enabled = enabled;
            }
        }
        prevenabled = enabled;
	}
}
