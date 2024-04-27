using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    [SerializeField] GameObject menuButton;
    [SerializeField] TextMeshProUGUI highscorePlayerNames;
    [SerializeField] TextMeshProUGUI highscoreScores;

    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
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

        UpdateHighscore();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                menuButton.SetActive(false);

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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        // store the new highscore table if this was a highscore
        // and update the displayed highscore
        int highscoreIndex = StorageManager.Instance.StoreHighScore(m_Points);

        if (highscoreIndex > -1)
        {
            StorageManager.Instance.SaveHighscoreData();
            UpdateHighscore(highscoreIndex);
        }

        m_GameOver = true;
        GameOverText.SetActive(true);
        menuButton.SetActive(true);
    }

    public void UpdateHighscore(int highlightIndex = -1) 
    {
        string playerList = "";
        string playerScores = "";
        for (int i = 0; i < StorageManager.Instance.highscore.Count; i++) 
        {
            bool isHighlight = (highlightIndex == i);
            playerList += (isHighlight) ? "<color=#ff0000>" + StorageManager.Instance.highscoreName[i] + "</color>" + "\n" : StorageManager.Instance.highscoreName[i] + "\n";
            playerScores += (isHighlight) ? "<color=#ff0000>" + StorageManager.Instance.highscore[i] + "</color>" + "\n" : StorageManager.Instance.highscore[i].ToString() + "\n";
        }

        if (playerList != "") {
            highscorePlayerNames.text = playerList;
            highscoreScores.text = playerScores;
        }
    }

}
