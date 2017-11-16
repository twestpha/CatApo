using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[CreateAssetMenu()]
[Serializable]
public class AnimationPose : ScriptableObject {
    public Vector3 zerojoint;
    public Quaternion[] joints;
}
