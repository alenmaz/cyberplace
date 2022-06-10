using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var playerStats = other.gameObject.GetComponent<Stats>();
            var player = other.gameObject.GetComponent<Player>();
            var shield = player.shield;
            var shieldTimer = player.shieldTimer;
            if (!player.shield.activeInHierarchy)
            {
                playerStats.UpdateShields(0);
                playerStats.CanBeDamaged = false;
                shield.SetActive(true);
                shieldTimer.gameObject.SetActive(true);
                shieldTimer.isCooldown = true;
                Destroy(this.gameObject);
            }
            else
            {
                shieldTimer.ResetTime();
                Destroy(this.gameObject);
            }            
        }

    }
}
