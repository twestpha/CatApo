using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour {
    public const int InteractableCollisionMask = 1 << 9;

    public bool enabled = true;

    public virtual void NotifyClicked(){}

    public void Toggle(){
        enabled = !enabled;
        gameObject.layer = enabled ? InteractableCollisionMask : 0;
    }
}
