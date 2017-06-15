using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUIComponent : MonoBehaviour {

    public GameObject player;

    [Header("Icons")]
    public List<GameObject> abilityIcons;
    public List<GameObject> abilityIconCovers;

    private AbilityCastComponent castComponent;

    public GameObject healthBar;
    private Vector3 healthBarOffset = new Vector3(0.0f, -80.0f);

	void Start(){
        castComponent = player.GetComponent<AbilityCastComponent>();
	}

	void Update(){
        for(int i = 0; i < (int)AbilityCastComponent.AbilitySlot.OverflowAbilities; ++i){
            abilityIconCovers[i].GetComponent<Image>().fillAmount = 1.0f - castComponent.GetCooldownPercentage(i);
        }

        healthBar.transform.position = Camera.main.WorldToScreenPoint (player.transform.position) + healthBarOffset;
	}
}
