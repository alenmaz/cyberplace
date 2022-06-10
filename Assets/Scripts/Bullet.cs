using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private string Name = "None";
    public string homeTeamTag;
    public string enemyTeamTag;

    private Vector2 initialFirePoint;

    [Header("Bullet stats")]
    [SerializeField] private float MaxDistance;
    [SerializeField] private DamageType DamageType;
    [SerializeField] private int Damage;
    [SerializeField] private float Force;
    [SerializeField] private float PushStrength;
    [SerializeField] private float KnockDownTime;
    [SerializeField] private float StunTime;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialFirePoint = gameObject.transform.position;
    }

    void FixedUpdate()
    {
       if (Vector3.Distance(initialFirePoint, transform.position) > MaxDistance)
       {
            DestroyImmediate(gameObject);
            return;
       }
       rb.AddForce(gameObject.transform.up * Force, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(homeTeamTag)) return;
        if (other.gameObject.CompareTag(enemyTeamTag))
        {
            other.gameObject.GetComponent<Enemy>().DisableMove(KnockDownTime, StunTime);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * PushStrength,ForceMode2D.Impulse);
            var stats = other.gameObject.GetComponent<Stats>();
            if (stats != null)
            {
                Debug.Log($"Target of team {enemyTeamTag} is taking {Damage} points of dmg from bullet");
                stats.TakeDamage(Damage, DamageType);
            }
            Destroy(gameObject);
            return;
        }
        if (!other.isTrigger) Destroy(gameObject);
    }
}
