using UnityEngine;

public class HoleCollisionDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.CompareTag("Hole") && (collision.CompareTag("Obstacle") || collision.CompareTag("Hole")))
        {
            transform.position = new Vector3(Random.Range(-15f, 15f), Random.Range(-20f, 20f), 0);
        }
    }
}