using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManagerMenu : MonoBehaviour
{
    [SerializeField] public GameObject MainMenuPage;

    [Header("Dependencies")]
    [SerializeField] private SaveManager SaveManager;

    [Header("UI dependencies")]
    [SerializeField] private TextMeshProUGUI recordText;

    private void Start()
    {
        if(MainMenuPage != null) MainMenuPage.SetActive(true);
        SaveManager = FindObjectOfType<SaveManager>();
        if (SaveManager != null) recordText.text = SaveManager.KillsRecord.ToString();
    }

    public void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
