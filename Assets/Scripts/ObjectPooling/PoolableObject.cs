using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Enemy,
    Pack,
    BigEnemy,
    Boss,
    Aid,
    Shield,
    MinePickUp,
    Mine,
    Bullet
}

public class PoolableObject : MonoBehaviour
{
    [SerializeField] private ObjectType objectType;

    public ObjectType ObjectType { get { return objectType; } }
}
