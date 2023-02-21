using UnityEngine;

public class Sensor : MonoBehaviour
{
    [SerializeField]
    private Transform holeParent;
    [SerializeField]
    private Sprite[] sensorHalos;
    [SerializeField]
    private PlayerController playerController;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        FindHoleDistance();
    }

    private void FindHoleDistance()
    {
        Transform closestHole = holeParent.GetChild(0);
        float minDistance = 100f;

        foreach (Transform hole in holeParent)
        {
            if (!hole.CompareTag("Hole"))
                continue;

            float distance = Vector2.Distance(transform.position, hole.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestHole = hole;
            }
        }

        if (minDistance < 10f)
        {
            spriteRenderer.enabled = true;

            if (minDistance < 4f)
            {
                spriteRenderer.sprite = sensorHalos[2];
                playerController.MovementSpeed = 2;
            }
            else if (minDistance < 7f)
            {
                spriteRenderer.sprite = sensorHalos[1];
                playerController.MovementSpeed = 3;
            }
            else
            {
                spriteRenderer.sprite = sensorHalos[0];
                playerController.MovementSpeed = 4;
            }
        }
        else
        {
            spriteRenderer.enabled = false;
            playerController.MovementSpeed = 5;
        }
    }
}