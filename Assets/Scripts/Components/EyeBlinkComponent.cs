using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBlinkComponent : MonoBehaviour {

    public Material eyesOpenMaterial;
    public Material eyesClosedMaterial;

    private Timer openTimer;
    private Timer closedTimer;
    private MeshRenderer mesh;

    private float minBlinkTime = 1.0f;
    private float maxBlinkTime = 4.0f;

    private bool open;

	void Start(){
        open = true;
        openTimer = new Timer(Random.Range(minBlinkTime, maxBlinkTime));
        closedTimer = new Timer(0.2f);
        mesh = GetComponent<MeshRenderer>();

        openTimer.Start();
	}

	void Update(){
        if(open && openTimer.Finished()){
            open = false;
            closedTimer.Start();
            mesh.material = eyesClosedMaterial;
        }

        if(!open && closedTimer.Finished()){
            open = true;
            openTimer.Start();
            openTimer.SetDuration(Random.Range(minBlinkTime, maxBlinkTime));
            mesh.material = eyesOpenMaterial;
        }
	}
}
