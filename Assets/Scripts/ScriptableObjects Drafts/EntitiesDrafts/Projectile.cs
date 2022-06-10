using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New projectile", menuName = "Projectiles/New projectile", order = 0)]
public class Projectile : ScriptableObject {
    public string Name = "None";

    [Header("Bullet stats")]
    public float MaxDistance;
    public DamageType DamageType;
    public float Damage;
    public float Force;
}
