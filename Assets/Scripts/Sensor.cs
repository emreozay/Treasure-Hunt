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
        foreach (Transform hole in holeParent)
        {
            if (!hole.CompareTag("Hole"))
                return;

            float distance = Vector2.Distance(transform.position, hole.position);

            if(distance < 10f)
            {
                spriteRenderer.enabled = true;

                if (distance < 4f)
                    spriteRenderer.sprite = sensorHalos[2];
                else if (distance < 7f)
                    spriteRenderer.sprite = sensorHalos[1];
                else
                    spriteRenderer.sprite = sensorHalos[0];
            }
            else
            {
                spriteRenderer.enabled = false;
            }

            Debug.Log(hole.name + " - " + distance, hole.gameObject);
        }
    }
}