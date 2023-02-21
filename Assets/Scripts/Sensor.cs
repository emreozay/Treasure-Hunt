using Unity.VisualScripting;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    [SerializeField]
    private Transform holeParent;
    [SerializeField]
    private Sprite[] sensorHalos;

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

            if(distance < minDistance)
            {
                minDistance = distance;
                closestHole = hole;
            }
        }

        if (minDistance < 10f)
        {
            spriteRenderer.enabled = true;

            if (minDistance < 4f)
                spriteRenderer.sprite = sensorHalos[2];
            else if (minDistance < 7f)
                spriteRenderer.sprite = sensorHalos[1];
            else
                spriteRenderer.sprite = sensorHalos[0];
        }
        else
        {
            spriteRenderer.enabled = false;
        }

        Debug.Log(closestHole.name + " - " + minDistance, closestHole.gameObject);
    }
}