using UnityEngine;

public class NameText : MonoBehaviour
{
    private Renderer nameRenderer;

    private void Awake()
    {
        nameRenderer = GetComponent<Renderer>();
        nameRenderer.sortingLayerName = "UI";
    }
}
