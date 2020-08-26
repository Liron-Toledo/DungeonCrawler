using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{

    private bool bootCollected = false;
    private bool gunCollected = false;

    public List<string> collectedItemNames = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCollectedItems(CollectionController item)
    {
        collectedItemNames.Add(item.item.name);

        foreach (string i in collectedItemNames)
        {
            switch(i)
            {
                case ("Boot"):
                    bootCollected = true;
                    break;
                case ("Gun"):
                    gunCollected = true;
                    break;

            }
        }

        if (bootCollected && gunCollected)
        {
            gameObject.GetComponent<AttackController>().changeFireRate(0.25f);
        }
    }
}
