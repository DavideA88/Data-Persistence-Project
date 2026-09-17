using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public GameObject highScoresScreen;
    public TextMeshProUGUI[] highScoresElements;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (SessionManager.Instance.playerName != "")
        {
            nameInputField.text = SessionManager.Instance.playerName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        
    }

    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void SetName(string playerName)
    {
        SessionManager.Instance.playerName = playerName;
    }

    public void ShowHighScores()
    {
        highScoresScreen.SetActive(true);

        for (int i = 0; i < highScoresElements.Length; i++)
        {
            if (SessionManager.Instance.highScores[i] != null)
            {
                highScoresElements[i].text = $"{i + 1}. {SessionManager.Instance.highScores[i].playerName} - {SessionManager.Instance.highScores[i].score}";
            }
        }
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
