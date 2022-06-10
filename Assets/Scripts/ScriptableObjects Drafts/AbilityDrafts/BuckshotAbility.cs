using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ability/Buckshot", order = 3)]
public class BuckshotAbility : ActiveAbility 
{
    [SerializeField] private int NumberofBullets;
    public override void Activate(GameObject parent)
    {
        parent.GetComponent<Player>().AmountOfBullets = NumberofBullets;
    }

    public override void Reset(GameObject parent)
    {
        parent.GetComponent<Player>().AmountOfBullets = 1;
    }
}
