using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Ability/New empty active ability", order = 0)]
public class ActiveAbility : Ability {
    [Header("Ability requirments statistics")]
    public bool needsTarget = false;
    
    [Header("Ability time settings")]
    public int TickTime;
    public float coolDownTime;
    public float ActiveTime;

    public virtual void Reset(GameObject parent) { }
}

