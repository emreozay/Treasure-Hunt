using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private Transform textParent;
    [SerializeField]
    private Renderer nameRenderer;
    [SerializeField]
    private TextMeshPro nameText;

    private string[] names = { "Noah", "Theo", "Oliver", "George", "Leo", "Freddie", "Arthur", "Archie", "Alfie", "Charlie", "Oscar", "Henry", "Harry", "Jack", "Teddy", "Finley", "Arlo", "Luca", "Jacob", "Tommy", "Lucas", "Theodore", "Max", "Isaac", "Albie", "James", "Mason", "Rory", "Thomas", "Rueben", "Roman", "Logan", "Harrison", "William", "Elijah", "Ethan", "Joshua", "Hudson", "Jude", "Louie", "Jaxon", "Reggie", "Oakley", "Hunter", "Alexander", "Toby", "Adam", "Sebastian", "Daniel", "Ezra", "Rowan", "Alex" };
    private static Queue<string> nameQueue = new Queue<string>();

    private void Awake()
    {
        nameRenderer.sortingLayerName = "UI";
    }

    void Start()
    {
        if (name == "Player")
            return;

        CreateNameQueue();
        SetNameTexts();
    }

    public void SetScoreText(int score)
    {
        UIManager.Instance.SetScoreText(textParent, score);
    }

    private void CreateNameQueue()
    {
        for (int i = 0; i < names.Length; i++)
        {
            nameQueue.Enqueue(names[i]);
        }
    }

    private void SetNameTexts()
    {
        string newName = nameQueue.Dequeue();
        textParent.GetChild(1).GetComponent<TextMeshProUGUI>().text = newName;
        nameText.text = newName;
    }
}
