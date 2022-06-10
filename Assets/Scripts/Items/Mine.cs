using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [Header("Mine")]
    public GameObject MineDmgCircle;
    public float Radius;
    public int Damage;
    public float SpriteOffset = 1f;

    private void Start()
    {
        if(MineDmgCircle != null && SpriteOffset > 0) MineDmgCircle.transform.localScale = new Vector3(Radius / SpriteOffset, Radius / SpriteOffset, Radius / SpriteOffset);
    }

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Vector2 position = new Vector2(this.transform.position.x, this.transform.position.y);
            if(MineDmgCircle != null) Instantiate(MineDmgCircle, position, Quaternion.identity);
            foreach (Collider2D hero in Physics2D.OverlapCircleAll(this.transform.position, Radius))
            {
                if (hero.gameObject.CompareTag("Enemy"))
                {
                    var stats = hero.gameObject.GetComponent<Stats>();
                    if (stats != null)
                    {
                        Debug.Log($"Enemy is taking {Damage} points of dmg from mine");
                        stats.TakeDamage(Damage, DamageType.Fire);
                    }
                }
            }
            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, Radius);
    }
}
