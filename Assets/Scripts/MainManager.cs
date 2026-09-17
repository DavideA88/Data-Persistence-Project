using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System.Runtime.InteropServices;
using System.Linq; // MIGRATED: New Input System namespace

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI HighScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    // MIGRATED: InputAction replaces Input.GetKeyDown(KeyCode.Space)
    private InputAction m_LaunchAction;
    private InputAction m_ReturnAction;

    private SessionManager inst = SessionManager.Instance;

    // MIGRATED: bind the Space key as a button action
    void Awake()
    {
        m_LaunchAction = new InputAction("Launch", InputActionType.Button, "<Keyboard>/space");
        m_ReturnAction = new InputAction("Return", InputActionType.Button, "<Keyboard>/escape");
    }

    // MIGRATED: enable the action while the component is active
    void OnEnable()
    {
        m_LaunchAction.Enable();
        m_ReturnAction.Enable();
    }

    // MIGRATED: disable the action when the component is inactive
    void OnDisable()
    {
        m_LaunchAction.Disable();
        m_ReturnAction.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        UpdateHighScore();
        AddPoint(inst.score);
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (m_LaunchAction.WasPressedThisFrame()) // MIGRATED: was Input.GetKeyDown(KeyCode.Space)
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (m_LaunchAction.WasPressedThisFrame()) // MIGRATED: was Input.GetKeyDown(KeyCode.Space)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            } else if (m_ReturnAction.WasPressedThisFrame())
            {
                SceneManager.LoadScene(0);
            }
        }

        if (FindObjectsByType<Brick>(FindObjectsSortMode.None).Length == 0)
        {
            inst.score += m_Points;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score of {inst.playerName} : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        inst.score = 0;

        CheckHighScore();
    }

    void CheckHighScore()
    {
        for (int i = 0; i < inst.highScores.Count; i++)
        {
            if (m_Points > inst.highScores[i].score)
            {
                inst.highScores.Insert(i, new SessionManager.HighScore());

                inst.highScores[i].playerName = inst.playerName;
                inst.highScores[i].score = m_Points;

                if (inst.highScores.Count > 5)
                {
                    inst.highScores.RemoveAt(5);
                }

                inst.SaveHighScore();

                UpdateHighScore();

                break;
            }
        }
    }

    void UpdateHighScore()
    {
        HighScoreText.text = $"Best Score : {inst.highScores[0].playerName} - {inst.highScores[0].score}";
    }
}