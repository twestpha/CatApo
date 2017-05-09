using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class AbilityVolume : ScriptableObject {
    // Ability Volume is the area in which an ability can be cast
    public bool debug;

    public Vector2[] polygon;

    public bool ContainsPoint(Vector3 point){
        if(debug){
            for(int i = 0; i < polygon.Length; ++i){
                Vector3 a = new Vector3(polygon[i].x, 0.1f, polygon[i].y);
                Vector3 b = i == 0 ?
                            new Vector3(polygon[polygon.Length - 1].x, 0.1f, polygon[polygon.Length - 1].y) :
                            new Vector3(polygon[i - 1].x, 0.1f, polygon[i - 1].y);
                Debug.DrawLine(a, b, Color.red, 1.0f);
            }
        }

        // transform into ability space?

        int j = polygon.Length - 1;
        var result = false;

        for(int i = 0; i < polygon.Length; j = i++){
           if (((polygon[i].y <= point.z && point.z < polygon[j].y) || (polygon[j].y <= point.z && point.z < polygon[i].y)) &&
              (point.x < (polygon[j].x - polygon[i].x) * (point.z - polygon[i].y) / (polygon[j].y - polygon[i].y) + polygon[i].x))
              result = !result;
        }

        return result;
    }
}
