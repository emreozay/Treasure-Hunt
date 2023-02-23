using UnityEngine;

public class SpawnableObjectCollisionDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            if (gameObject.CompareTag("DugHole"))
                return;

            transform.position = new Vector3(Random.Range(-15f, 15f), Random.Range(-20f, 20f), 0);
        }
    }
}