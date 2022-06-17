using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAnimation : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float amplitudeX = 0.5f;
    [SerializeField] float amplitudeY = 0.5f;

    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float newY = Mathf.Sin(Time.time * speed) * amplitudeX + pos.y;
        float newX = Mathf.Sin(Time.time * speed) * amplitudeY + pos.x;
        transform.localPosition = new Vector3(newX, newY, transform.localPosition.z);
    }
}
