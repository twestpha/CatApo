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
        point.y = 0.0f;

        Vector2[] newPolygon = new Vector2[polygon.Length];

        for(int i = 0; i < newPolygon.Length; ++i){
            float rotx = (polygon[i].x * Mathf.Cos(rotation)) - (polygon[i].y * Mathf.Sin(rotation));
            float roty = (polygon[i].y * Mathf.Cos(rotation)) + (polygon[i].x * Mathf.Sin(rotation));
            newPolygon[i] = new Vector2(rotx, roty) + new Vector2(position.x, position.z);
        }

        if(debug){
            // If you're not seeing anything draw, you forgot to turn on Gizmos, dumbass
            for(int i = 0; i < newPolygon.Length; ++i){
                Vector3 a = new Vector3(newPolygon[i].x, 0.1f, newPolygon[i].y);
                Vector3 b = i == 0 ?
                            new Vector3(newPolygon[newPolygon.Length - 1].x, 0.1f, newPolygon[newPolygon.Length - 1].y) :
                            new Vector3(newPolygon[i - 1].x, 0.1f, newPolygon[i - 1].y);
                Debug.DrawLine(a, b, Color.red, 1.0f);
            }
        }

        int j = newPolygon.Length - 1;
        var result = false;

        for(int i = 0; i < newPolygon.Length; j = i++){
           if (((newPolygon[i].y <= point.z && point.z < newPolygon[j].y) || (newPolygon[j].y <= point.z && point.z < newPolygon[i].y)) &&
              (point.x < (newPolygon[j].x - newPolygon[i].x) * (point.z - newPolygon[i].y) / (newPolygon[j].y - newPolygon[i].y) + newPolygon[i].x))
              result = !result;
        }

        return result;
    }
}
