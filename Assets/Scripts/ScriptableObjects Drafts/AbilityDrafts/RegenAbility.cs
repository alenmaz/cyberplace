using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Ability/Regen ability", order = 0)]
public class RegenAbility : ActiveAbility
{
    [SerializeField] private int healthPerSecond;

    public override void Activate(GameObject parent)
    {
        var playerStats = parent.GetComponent<Stats>();
        if(playerStats != null) playerStats.UpdateHealth(healthPerSecond);
    }
}