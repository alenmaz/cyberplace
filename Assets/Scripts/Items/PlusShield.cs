using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusShield : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var playerStats = other.gameObject.GetComponent<Stats>();
            if (playerStats != null) playerStats.UpdateShields(10);
            Destroy(this.gameObject);
        }

    }
}
