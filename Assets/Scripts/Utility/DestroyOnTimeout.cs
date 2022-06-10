using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTimeout : MonoBehaviour
{
    [Header("Lifetime")]
    [SerializeField] private float destroyTimeout;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > destroyTimeout) Destroy(this.gameObject);
    }
}
