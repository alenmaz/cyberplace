using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingUI : MonoBehaviour
{
    public GameObject target;
    public Vector2 offset;

    void Update()
    {
        ((RectTransform)transform).position = Camera.main.WorldToScreenPoint(target.transform.position) + (Vector3)offset;
    }
}
