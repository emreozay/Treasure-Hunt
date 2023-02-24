using UnityEngine;

public class Sensor : MonoBehaviour
{
    [SerializeField]
    private Transform holeParent;
    [SerializeField]
    private Sprite[] sensorHalos;
    [SerializeField]
    private Movement movement;

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

        if (closestHole == null)
            return;

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
                movement.MovementSpeed = movement.MaxMovementSpeed / 2.5f;
            }
            else if (minDistance < 7f)
            {
                spriteRenderer.sprite = sensorHalos[1];
                movement.MovementSpeed = movement.MaxMovementSpeed / 1.5f;
            }
            else
            {
                spriteRenderer.sprite = sensorHalos[0];
                movement.MovementSpeed = movement.MaxMovementSpeed / 1.25f;
            }
        }
        else
        {
            spriteRenderer.enabled = false;
            movement.MovementSpeed = movement.MaxMovementSpeed;
        }
    }
}