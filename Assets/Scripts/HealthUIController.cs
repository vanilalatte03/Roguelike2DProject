using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour
{
    public GameObject heartContainer;

    private float fillValue;

    void Update()
    {
        fillValue = (float)GameController.instance.Health;
        fillValue = fillValue / GameController.instance.MaxHealth;
        heartContainer.GetComponent<Image>().fillAmount = fillValue;
    }
}
