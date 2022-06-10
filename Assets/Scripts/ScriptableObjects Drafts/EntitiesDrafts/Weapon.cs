using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New weapon", menuName = "Weapons/New weapon", order = 0)]
public class Weapon : ScriptableObject {
    public string Name = "None";

    [Header("Weapon stats")]
    [SerializeField] private int maxAmmo;
    [SerializeField] private float attackRate;
    [SerializeField] private float reloadTime; 

    public int MaxAmmo { get => maxAmmo; }
    public float AttackRate { get => attackRate; }
    public float ReloadTime { get => reloadTime; }


    [Header("Bullet to spawn")]
    public GameObject BulletPrefab;

    public void SpawnBullet(Transform spawnPoint, int amount, float maxAngle)
    {
        GameObject bullet;
        float angle = 0f;
        for (int i = 1; i < amount + 1; i++)
        {
            bullet = Instantiate(BulletPrefab,
                spawnPoint.position,
                spawnPoint.rotation,
                null);
            if (amount == 1) angle = 0;
            else angle = (maxAngle/(amount - 1)) * Mathf.Sin(90 * i * Mathf.Deg2Rad);
            bullet.transform.Rotate(new Vector3(0.0f, 0.0f, angle), Space.Self);
        }
    }
}
