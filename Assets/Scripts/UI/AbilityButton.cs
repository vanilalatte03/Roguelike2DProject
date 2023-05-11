using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum AbilityType
{
    Speed,
    Heal,
    Health,
    Attack
};

public class AbilityButton : MonoBehaviour
{
    public GameObject all;
    public AbilityType abilityType; 

    public GameObject Speed;
    public GameObject Heal;
    public GameObject Health;
    public GameObject Attack;

    public GameObject image;
    public GameObject text;

    private void Start()
    {
        switch (abilityType)
        {
            case AbilityType.Speed:
                image.GetComponent<Image>().sprite = Speed.GetComponent<SpriteRenderer>().sprite;
                text.GetComponent<TextMeshProUGUI>().text = "Speed +1";
                break;
            case AbilityType.Heal:
                image.GetComponent<Image>().sprite = Heal.GetComponent<SpriteRenderer>().sprite;
                text.GetComponent<TextMeshProUGUI>().text = "Heal +1";
                break;
            case AbilityType.Health:
                image.GetComponent<Image>().sprite = Health.GetComponent<SpriteRenderer>().sprite;
                text.GetComponent<TextMeshProUGUI>().text = "Max Health +1";
                break;
            case AbilityType.Attack:
                image.GetComponent<Image>().sprite = Attack.GetComponent<SpriteRenderer>().sprite;
                text.GetComponent<TextMeshProUGUI>().text = "Attack Speed +1";
                break;
        }
    }

    public void OnClickButton()
    {
        switch (abilityType)
        {
            case AbilityType.Speed:
                Speed.GetComponent<CollectionController>().GetAbility();
                Debug.Log("Speed Up");
                break;
            case AbilityType.Heal:
                Heal.GetComponent<CollectionController>().GetAbility();
                Debug.Log("Healing");
                break;
            case AbilityType.Health:
                Health.GetComponent<CollectionController>().GetAbility();
                Debug.Log("Healthy");
                break;
            case AbilityType.Attack:
                Attack.GetComponent<CollectionController>().GetAbility();
                Debug.Log("Fast Attack");
                break;
        }

        all.gameObject.SetActive(false);
    }
}
