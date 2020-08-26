using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    public int health;
    public int maxHealth;

    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Sprite halfHeart;

    // Update is called once per frame
    void Update()
    {
        HeartsUI();
    }

    void HeartsUI()
    {
        if (health > numOfHearts)
        {
            Debug.Log("Already at Full Health");
            health = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        health = Mathf.Min(maxHealth, health + healAmount);
    }

    private void Die()
    {
        //Destroy(gameObject);
        Debug.Log("You Die!");
    }

}
