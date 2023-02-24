using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public void StartGame()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
