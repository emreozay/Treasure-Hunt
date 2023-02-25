using UnityEngine;

public class CountdownController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        UIManager.CountdownStartAction += StartCountdownAnimation;

        animator = GetComponent<Animator>();    
    }

    public void StartGame()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;

        UIManager.CountdownFinishAction?.Invoke();
    }

    private void StartCountdownAnimation()
    {
        animator.SetTrigger("StartCountdown");
    }

    private void OnDestroy()
    {
        UIManager.CountdownStartAction -= StartCountdownAnimation;
    }
}