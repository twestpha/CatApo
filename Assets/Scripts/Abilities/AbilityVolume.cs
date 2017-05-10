using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityVolume : ScriptableObject {
    // Ability Volume is the area in which an ability can be cast
    public bool debug;

    protected Vector3 position;
    protected float rotation;

    public void SetPosition(Vector3 position_){
        position = position_;
        position.y = 0.0f;
    }

    public void SetRotation(float rotation_){
        rotation = rotation_;
    }

    virtual public bool ContainsPoint(Vector3 point){
        return false;
    }
}

[CreateAssetMenu()]
[System.Serializable]
public class AbilityVolumeRadius : AbilityVolume{
    public float radius;

    override public bool ContainsPoint(Vector3 point){
        if(debug){
            Debug.DrawLine(position, position + (Vector3.right * radius), Color.red, 1.0f);
            Debug.DrawLine(position, position + (-Vector3.right * radius), Color.red, 1.0f);
            Debug.DrawLine(position, position + (Vector3.forward * radius), Color.red, 1.0f);
            Debug.DrawLine(position, position + (-Vector3.forward * radius), Color.red, 1.0f);
        }

        point.y = 0.0f;
        point -= position;

        return point.magnitude <= radius;
    }
}

[CreateAssetMenu()]
[System.Serializable]
public class AbilityVolumePolygon : AbilityVolume{
    public Vector2[] polygon;

    override public bool ContainsPoint(Vector3 point){
        if(debug){
            for(int i = 0; i < polygon.Length; ++i){
                Vector3 a = new Vector3(polygon[i].x + position.x, 0.1f, polygon[i].y + position.z);
                Vector3 b = i == 0 ?
                            new Vector3(polygon[polygon.Length - 1].x + position.x, 0.1f, polygon[polygon.Length - 1].y + position.z) :
                            new Vector3(polygon[i - 1].x + position.x, 0.1f, polygon[i - 1].y + position.z);
                Debug.DrawLine(a, b, Color.red, 1.0f);
            }
        }

        point.y = 0.0f;

        // transform point into ability space
        point -= position;
        // then some rotation around y

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
