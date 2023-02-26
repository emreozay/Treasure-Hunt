using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform player; 
    [SerializeField]
    Vector3 positionOffset;

    [SerializeField]
    float smoothCameraFollowStrength = 5.0f;

    void LateUpdate()
    {
        SetCameraPosition();
    }

    private void SetCameraPosition()
    {
        Vector3 offset = player.position + positionOffset;
        float lerpAmount = Time.deltaTime * smoothCameraFollowStrength;

        transform.position = Vector3.Lerp(transform.position, offset, lerpAmount);
        transform.position = new Vector3(transform.position.x, transform.position.y, offset.z);
    }
}