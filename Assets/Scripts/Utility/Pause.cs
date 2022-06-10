using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameManager manager;
    [SerializeField] private float mouseDelay;

    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
        manager.isPaused = true;
        StartCoroutine(DisableMouse());
    }

    IEnumerator DisableMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yield return new WaitForSecondsRealtime(mouseDelay);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
        manager.isPaused = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
