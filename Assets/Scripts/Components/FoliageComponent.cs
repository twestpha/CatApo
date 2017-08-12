using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageComponent : MonoBehaviour {
    public float xIntensity;
    public float yIntensity;
    public float zIntensity;

    private float seed;

    public List<GameObject> branches;

    void Start(){
        seed = Random.Range(-5.0f, 5.0f);
    }

    void Update(){
        for(int i = 0; i < branches.Count; ++i){
            branches[i].transform.eulerAngles += new Vector3(xIntensity * Time.deltaTime * (Mathf.Sin(Time.time + seed) + Mathf.Sin(Time.time * 4.5f * seed)),
                                                             -yIntensity * Time.deltaTime * (Mathf.Sin(Time.time * 2.0f * seed) + Mathf.Sin(Time.time + 1.5f + seed)),
                                                             zIntensity * Time.deltaTime * (Mathf.Sin(Time.time * 3.0f * seed) + Mathf.Cos(Time.time + 5.0f + seed)));
        }
    }
}
