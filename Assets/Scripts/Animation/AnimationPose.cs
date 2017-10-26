using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
[System.Serializable]
public class AnimationPose : ScriptableObject {
    public Quaternion[] joints;
}
