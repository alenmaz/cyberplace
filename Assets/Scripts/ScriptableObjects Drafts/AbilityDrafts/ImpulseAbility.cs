using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Ability/Impulse", order = 2)]
public class ImpulseAbility : ActiveAbility
{
    [SerializeField] private float Power;
    [SerializeField] private float KnockDownTime;
    [SerializeField] private float StunTime;
    [SerializeField] private float Radius;

    public override void Activate(GameObject parent)
    {
        foreach (Collider2D hit in Physics2D.OverlapCircleAll(parent.transform.position, Radius))
        {
            if (hit.gameObject.CompareTag("Enemy") && hit.attachedRigidbody != null)
            {
                hit.gameObject.GetComponent<Enemy>().DisableMove(KnockDownTime, StunTime);
                var direction = hit.transform.position - parent.transform.position;
                hit.attachedRigidbody.AddForce(direction.normalized * Power, ForceMode2D.Impulse);
            }
        }
    }

    public override void Reset(GameObject parent)
    {

    }
}
