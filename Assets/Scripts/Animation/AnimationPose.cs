using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[CreateAssetMenu()]
[Serializable]
public class AnimationPose : ScriptableObject {
    public Quaternion[] joints;
}
