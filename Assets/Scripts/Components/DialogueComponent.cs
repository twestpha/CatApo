using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueComponent : MonoBehaviour {

    public const int DialogueCollisionMask = 1 << 9;

    public Strings.LocalizedString text;

	public string GetString(){
        return Strings.GetString(text);
	}
}
