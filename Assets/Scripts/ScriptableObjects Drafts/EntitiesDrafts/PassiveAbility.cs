using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Ability/New empty passive ability", order = 0)]
public class PassiveAbility : Ability
{
    [SerializeField] private int HealthBonus;
    [SerializeField] private int ShieldBonus;
    [SerializeField] private float SpeedBonus;
    [SerializeField] private float RealoadSpeedBonus;
    [SerializeField] private float AidBonusPercent;

    public override void Activate(GameObject parent)
    {
        var stats = parent.GetComponent<Stats>();
        var player = parent.GetComponent<Player>();
        if(stats != null && player != null)
        {
            stats.IncreaseMaxHealth(HealthBonus);
            stats.IncreaseMaxShields(ShieldBonus);

            player.ReloadTime -= RealoadSpeedBonus;
            player.Speed += SpeedBonus;
            player.AidBonus *= AidBonusPercent;
        }
    }
}
