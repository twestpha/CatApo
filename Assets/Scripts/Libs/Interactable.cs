using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour {
    public const int InteractableCollisionMask = 1 << 9;

    public virtual void NotifyClicked(){}
}
