using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    public GameObject Speed;
    public GameObject Heal;
    public GameObject Health;
    public GameObject Attack;

    public void OnClickButton()
    {
        int random = Random.Range(0, 4);

        switch (random)
        {
            case 0:
                Speed.GetComponent<CollectionController>().GetAbility();
                Debug.Log("Speed Up");
                break;
            case 1:
                Heal.GetComponent<CollectionController>().GetAbility();
                Debug.Log("Healing");
                break;
            case 2:
                Health.GetComponent<CollectionController>().GetAbility();
                Debug.Log("Healthy");
                break;
            case 3:
                Attack.GetComponent<CollectionController>().GetAbility();
                Debug.Log("Fast Attack");
                break;
        }

        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
