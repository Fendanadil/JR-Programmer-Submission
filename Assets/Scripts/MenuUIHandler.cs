using System.Collections;
using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] private GameObject playerNameInputGO;
    private TMP_InputField playerNameInput;

    // Start is called before the first frame update
    void Start()
    {
        playerNameInput = playerNameInputGO.GetComponent<TMP_InputField>();
        playerNameInput.characterLimit = 12;

        // update the player name
        playerNameInput.text = StorageManager.Instance.playerName;

        // update the highscore
        ArrayList highscore = StorageManager.Instance.GetHighScore();
        string hsText = "0";
        if (highscore.Count > 0) { 
            hsText = highscore[1] + " by " + highscore[0];
        }

        highscoreText.text = "Highscore: " + hsText;
    }

    public void StartGame()
    {
        StorageManager.Instance.playerName = playerNameInput.text;
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        StorageManager.Instance.playerName = playerNameInput.text;
        StorageManager.Instance.SaveConfigData();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

}
