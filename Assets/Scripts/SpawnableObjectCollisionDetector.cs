using UnityEngine;

public class SpawnableObjectCollisionDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            if (gameObject.CompareTag("DugHole"))
                return;

            transform.position = LevelManager.Instance.GetRandomPosition();
        }
    }
}