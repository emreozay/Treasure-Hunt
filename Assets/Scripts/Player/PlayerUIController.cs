using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private Transform textParent;
    [SerializeField]
    private TextMeshPro nameText;

    private string[] names = { "Noah", "Theo", "Oliver", "George", "Leo", "Freddie", "Arthur", "Archie", "Alfie", "Charlie", "Oscar", "Henry", "Harry", "Jack", "Teddy", "Finley", "Arlo", "Luca", "Jacob", "Tommy", "Lucas", "Theodore", "Max", "Isaac", "Albie", "James", "Mason", "Rory", "Thomas", "Rueben", "Roman", "Logan", "Harrison", "William", "Elijah", "Ethan", "Joshua", "Hudson", "Jude", "Louie", "Jaxon", "Reggie", "Oakley", "Hunter", "Alexander", "Toby", "Adam", "Sebastian", "Daniel", "Ezra", "Rowan", "Alex" };
    private static Queue<string> nameQueue = new Queue<string>();

    private static bool isQueueCreated;

    void Start()
    {
        CreateNameQueue();
        SetNameTexts();
    }

    public void SetNameColor(AILevel enemyAILevel)
    {
        switch (enemyAILevel)
        {
            case AILevel.Easy:
                nameText.color = Color.white;
                break;
            case AILevel.Medium:
                nameText.color = Color.red;
                break;
            case AILevel.Hard:
                nameText.color = Color.black;
                break;
            default:
                break;
        }
    }

    public Transform GetUIParent()
    {
        return textParent;
    }

    public void SetScoreText(int score)
    {
        UIManager.Instance.SetScoreText(textParent, score);
    }

    private void CreateNameQueue()
    {
        if (isQueueCreated)
            return;

        for (int i = 0; i < names.Length; i++)
        {
            nameQueue.Enqueue(names[i]);
        }

        isQueueCreated = true;
    }

    private void SetNameTexts()
    {
        if (name == "Player")
            return;

        string newName = nameQueue.Dequeue();
        textParent.GetChild(1).GetComponent<TextMeshProUGUI>().text = newName;
        nameText.text = newName;

        nameQueue.Enqueue(newName);
    }
}