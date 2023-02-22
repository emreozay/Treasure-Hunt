using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField]
    private Image timeBoxImage;
    [SerializeField]
    private Sprite redTimeBox;

    [SerializeField]
    private Transform[] scoreParents;

    private int timeLeft = 60;

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

    private IEnumerator Timer()
    {
        int totalTime = timeLeft;
        for (int i = 0; i < totalTime; i++)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
            timeText.text = timeLeft.ToString();

            if (timeLeft == 10)
                timeBoxImage.sprite = redTimeBox;
        }
    }

    public void SetScoreText(Transform scoreTextParent, int newScore)
    {
        TextMeshProUGUI scoreText = scoreTextParent.GetChild(0).GetComponent<TextMeshProUGUI>();
        scoreText.text = newScore.ToString();

        SetLeaderboard(scoreTextParent);
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
}