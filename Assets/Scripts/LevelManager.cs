using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Ball> balls;
    [SerializeField] private BallProjection ballProjection;
    private void Start()
    {
        EventManager.OnBalFallenIntoHole += RemoveAndDestroyBall;
    }
    public bool CanContinuePlay()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            if (balls[i].BallStopped() == false)
            {
                return false;
            }
        }
        return true;
    }
    public void RemoveAndDestroyBall(Ball ballToRemove)
    {
        ballProjection.RemoveAndDestroyClonedBall(balls.IndexOf(ballToRemove));
        balls.Remove(ballToRemove);
        Destroy(ballToRemove.gameObject);
        if (balls.Count == 1)
        {
            LevelEnded();
        }
    }
    public Ball[] GetAllBalls()
    {
        return balls.ToArray();
    }
    [ContextMenu("EndLevel")]
    public void LevelEnded()
    {
        EventManager.OnBalFallenIntoHole -= RemoveAndDestroyBall;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
