using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerScript : MonoBehaviour
{
    [Header("Player Health")]
    private float playerHealth = 100f;
    public float presentHealth;

    // Start is called before the first frame update
    void Start()
    {
        presentHealth = playerHealth;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playerHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        Debug.Log("player health " + presentHealth);

        if(presentHealth <= 0)
        {
            PlayerDie();
        }
    }
    private void PlayerDie()
    {
        // Object.Destroy(gameObject, 1.0f);
        Debug.Log("dead");
    }
    
}
