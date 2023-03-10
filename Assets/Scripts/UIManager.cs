using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject gameWinPanel;
    [SerializeField]
    private GameObject tapToStartPanel;

    [SerializeField]
    private Transform scoreboard;

    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private TextMeshProUGUI eliminatedText;

    [SerializeField]
    private Image timeBoxImage;
    [SerializeField]
    private Sprite redTimeBox;

    [SerializeField]
    private Transform[] scoreParents;
    [SerializeField]
    private PlayerUIController[] playerUIControllers;

    public static Action CountdownStartAction;
    public static Action CountdownFinishAction;

    private int timeLeft = 40;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        Time.timeScale = 0;

        StartCoroutine(Timer());
    }

    public void SetScoreText(Transform scoreTextParent, int newScore)
    {
        TextMeshProUGUI scoreText = scoreTextParent.GetChild(0).GetComponent<TextMeshProUGUI>();
        scoreText.text = newScore.ToString();

        SetLeaderboard(scoreTextParent);
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void GameWin()
    {
        LevelManager.Instance.NextLevel();
        gameWinPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        print("Restart the level!");
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        print("Load next level!");
    }

    public void TapToStart()
    {
        tapToStartPanel.SetActive(false);
        CountdownStartAction?.Invoke();
    }

    private IEnumerator Timer()
    {
        int totalTime = timeLeft;
        for (int i = 0; i < totalTime; i++)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
            timeText.text = timeLeft.ToString();

            if (timeLeft == 10)
            {
                timeBoxImage.sprite = redTimeBox;
                EliminateLastPlayer(2);
            }
            else if (timeLeft == 0)
                EndGame();

            if (timeLeft == 20)
                EliminateLastPlayer(3);
        }
    }

    private void EliminateLastPlayer(int childIndex)
    {
        Transform lastPlayer = scoreboard.GetChild(childIndex);

        for (int i = 0; i < playerUIControllers.Length; i++)
        {
            if (playerUIControllers[i] == null)
                continue;

            if (lastPlayer == playerUIControllers[i].GetUIParent())
            {
                string lastPlayerName = lastPlayer.GetChild(1).GetComponent<TextMeshProUGUI>().text;
                StartCoroutine(SetEliminatedText(lastPlayerName));

                if (playerUIControllers[i].gameObject.name == "Player")
                {
                    EndGame();
                    return;
                }

                playerUIControllers[i].gameObject.SetActive(false);
                return;
            }
        }
    }

    private IEnumerator SetEliminatedText(string lastPlayerName)
    {
        eliminatedText.text = lastPlayerName + " eliminated!";
        eliminatedText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        eliminatedText.gameObject.SetActive(false);
    }

    private void SetLeaderboard(Transform scoreTextParent)
    {
        TextMeshProUGUI scoreText = scoreTextParent.GetChild(0).GetComponent<TextMeshProUGUI>();
        int currentScore = int.Parse(scoreText.text);

        for (int i = 0; i < scoreParents.Length; i++)
        {
            TextMeshProUGUI childScoreText = scoreParents[i].GetChild(0).GetComponent<TextMeshProUGUI>();
            int childScore = int.Parse(childScoreText.text);

            if (currentScore > childScore)
            {
                if (scoreTextParent.GetSiblingIndex() > scoreParents[i].GetSiblingIndex())
                {
                    scoreTextParent.SetSiblingIndex(scoreParents[i].GetSiblingIndex());
                }
            }
        };
    }

    private void EndGame()
    {
        Time.timeScale = 0;

        if (scoreboard.GetChild(0) == scoreParents[0])
        {
            GameWin();
        }
        else
        {
            GameOver();
        }
    }
}