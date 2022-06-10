using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Ability/Dash", order = 1)]
public class DashAbility : ActiveAbility
{
    [SerializeField] private float acceleratedSpeed;

    public override void Activate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        player.Speed += acceleratedSpeed;
    }

    public override void Reset(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        player.Speed -= acceleratedSpeed;
    }
}