using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour {
    public const int InteractableCollisionMask = 1 << 9;

    public bool interactenabled = true;

    public virtual void NotifyClicked(){}

    public void Toggle(){
        interactenabled = !interactenabled;
        gameObject.layer = interactenabled ? InteractableCollisionMask : 0;
    }
}
