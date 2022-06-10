using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAnimation : MonoBehaviour
{
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    float maxScale = 0.5f;

    private Vector3 pos;

    private void Start()
    {
        pos = transform.localScale;
    }
    void Update()
    {
        float newY = Mathf.Abs(Mathf.Sin(Time.time * speed) * maxScale) + pos.y;
        float newX = Mathf.Abs(Mathf.Sin(Time.time * speed) * maxScale) + pos.x;
        transform.localScale = new Vector3(newX, newY, transform.localScale.z);
    }

}
