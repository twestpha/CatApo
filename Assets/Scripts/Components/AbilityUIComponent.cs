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

    public GameObject healthBarPrefab;

    private List<HealthUIComponent> healthBars;

	void Start(){
        // abilities
        castComponent = player.GetComponent<AbilityCastComponent>();

        // health bars
        healthBars = new List<HealthUIComponent>();

        Actor[] actors = (Actor[]) GameObject.FindObjectsOfType (typeof(Actor));

        for(int i = 0; i < actors.Length; ++i){
            GameObject healthBar = Object.Instantiate(healthBarPrefab);
            healthBar.GetComponent<HealthUIComponent>().target = actors[i].gameObject;
            healthBar.GetComponent<HealthUIComponent>().CreateHearts();
            healthBar.transform.SetParent(GetComponent<Canvas>().transform);
            healthBars.Add(healthBar.GetComponent<HealthUIComponent>());
        }
	}

	void Update(){
        // abilities
        for(int i = 0; i < (int)AbilityCastComponent.AbilitySlot.OverflowAbilities; ++i){
            abilityIconCovers[i].GetComponent<Image>().fillAmount = 1.0f - castComponent.GetCooldownPercentage(i);
        }

        // health bars
        for(int i = 0; i < healthBars.Count; ++i){
            healthBars[i].UpdateUI();
        }
	}
}
