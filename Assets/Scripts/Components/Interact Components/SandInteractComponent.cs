using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandInteractComponent : Interactable {

    public float dropDistance;
    public float dropDuration;

    private Timer dropTimer;

    private bool dropping = false;
    private float previousHeight;

    private float scrollSpeed = -0.27f;
    private float scroll = 0.0f;

    private AudioSource audioSource;
    private MeshRenderer mesh;

    public void Start(){
        dropTimer = new Timer(dropDuration);
        audioSource = GetComponent<AudioSource>();
        mesh = GetComponent<MeshRenderer>();
    }

    public void Update(){

        if(dropping){
            transform.position = new Vector3(transform.position.x, previousHeight - dropTimer.Parameterized() * dropDistance, transform.position.z);
            float timespeed = 1.0f - Mathf.Pow((dropTimer.Parameterized()), 3.0f);

            scroll += scrollSpeed * Time.deltaTime * (timespeed + 0.1f);
            mesh.material.SetTextureOffset("_BumpMap", new Vector2(0.0f, scroll));

            audioSource.volume = timespeed;

            if(dropTimer.Finished()){
                dropping = false;
                audioSource.Stop();
            }

        }
    }

    override public void NotifyClicked(){
        if(!interactenabled){
            return;
        }

        dropping = true;
        previousHeight = transform.position.y;
        dropTimer.Start();
        audioSource.Play();
    }
}
