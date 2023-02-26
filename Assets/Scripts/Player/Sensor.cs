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

    private bool isPlayer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (GetComponentInParent<PlayerController>() != null)
            isPlayer = true;
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

        if (minDistance < 6f)
        {
            spriteRenderer.enabled = true;

            if (minDistance < 2.5f)
            {
                if (isPlayer)
                    Handheld.Vibrate();

                spriteRenderer.sprite = sensorHalos[2];
                movement.MovementSpeed = movement.MaxMovementSpeed / 2.5f;
            }
            else if (minDistance < 4.5f)
            {
                spriteRenderer.sprite = sensorHalos[1];
                movement.MovementSpeed = movement.MaxMovementSpeed / 1.75f;
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