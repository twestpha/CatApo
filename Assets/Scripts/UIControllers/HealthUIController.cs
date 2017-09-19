using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthUIController : MonoBehaviour {

    public GameObject target;
    public Actor targetActor;

    public GameObject heartContainerPrefab;

    public Sprite armoredHeartSprite;
    public Sprite emptyArmorSprite;
    public Sprite emptyHeartSprite;
    public Sprite heartSprite;

    private List<GameObject> heartContainers;

    private float heartContainerWidth = 20.0f;
    private float heartContainerSpacing = 2.0f;

    private int lastHealth;
    private int lastArmor;

    private Vector3 healthBarOffset = new Vector3(0.0f, 80.0f);

	public void CreateHearts(){
        targetActor = target.GetComponent<Actor>();

        heartContainers = new List<GameObject>();

        int maxHealth = targetActor.maxHealth;
        int currentArmor = targetActor.currentArmor;
        int currentHealth = targetActor.currentHealth;

        float width = (maxHealth * heartContainerWidth) + ((maxHealth - 1) * heartContainerSpacing);

        for(int i = 0; i < maxHealth; ++i){
            heartContainers.Add(Object.Instantiate(heartContainerPrefab));
            heartContainers[i].transform.SetParent(transform);

            heartContainers[i].transform.localPosition = new Vector2((heartContainerWidth * i) + (heartContainerWidth / 2.0f) + (heartContainerSpacing * i) - (width / 2.0f), 0.0f);

            SetHeartContainerSprite(i);
        }

        lastHealth = currentHealth;
        lastArmor = currentArmor;
	}

	public void UpdateUI(){
        int currentArmor = targetActor.currentArmor;
        int currentHealth = targetActor.currentHealth;

        if(currentArmor != lastArmor || currentHealth != lastHealth){
            for(int i = 0; i < targetActor.maxHealth; ++i){
                SetHeartContainerSprite(i);
            }
        }

        lastHealth = currentHealth;
        lastArmor = currentArmor;

        transform.position = Camera.main.WorldToScreenPoint(target.transform.position) + healthBarOffset;
	}

    private void SetHeartContainerSprite(int index){
        int currentArmor = targetActor.currentArmor;
        int currentHealth = targetActor.currentHealth;

        Image heartContainerImage = heartContainers[index].GetComponent<Image>();

        if(index < currentArmor && index < currentHealth){
            heartContainerImage.sprite = armoredHeartSprite;
        } else if(index < currentArmor){
            heartContainerImage.sprite = emptyArmorSprite;
        } else if(index < currentHealth){
            heartContainerImage.sprite = heartSprite;
        } else {
            heartContainerImage.sprite = emptyHeartSprite;
        }
    }
}
