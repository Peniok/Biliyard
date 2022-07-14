using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallProjection : MonoBehaviour
{
    private Scene simulationScene;
    private PhysicsScene physicsScene;
    [SerializeField] private int physicsFramesToCalculate;

    [SerializeField] private Transform obstaclesParent;
    private ControllBall controlBallCloned;
    [SerializeField] private GameObject ballGameObject;

    private List<Ball> clonedBalls;
    [SerializeField] private LevelManager levelManager;

    void Start()
    {
        EventManager.OnChangingPosOfPointer += CalculateProjection;
        EventManager.OnPointerUp += ResetAllLinerenderers;
        clonedBalls = new List<Ball>();
        CreatePhysicsScen();
    }

    void CreatePhysicsScen()
    {
        simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        physicsScene = simulationScene.GetPhysicsScene();

        SceneManager.MoveGameObjectToScene(Instantiate(obstaclesParent.gameObject), simulationScene);
        for (int i = 0; i < levelManager.GetAllBalls().Length; i++)
        {
            clonedBalls.Add(Instantiate(levelManager.GetAllBalls()[i].gameObject, levelManager.GetAllBalls()[i].GetPos(), levelManager.GetAllBalls()[i].transform.rotation).GetComponent<Ball>());
            clonedBalls[i].PrepareBallForPhysicsScene();
            if (controlBallCloned==null && clonedBalls[i].TryGetComponent(out ControllBall controllball ))
            {
                controlBallCloned = controllball;
            }
            SceneManager.MoveGameObjectToScene(clonedBalls[i].gameObject, simulationScene);
        }
    }
    public void CalculateProjection(Vector3 posOfFinger)
    {
        for (int i = 0; i < levelManager.GetAllBalls().Length; i++)
        {
            clonedBalls[i].ResetThisBall(levelManager.GetAllBalls()[i].GetPos());
            levelManager.GetAllBalls()[i].ResetLineRenderer();
            levelManager.GetAllBalls()[i].SetNewPointForLineRenderer(clonedBalls[i].GetPos());
        }
        controlBallCloned.GetForce(posOfFinger);
        for (int i = 0; i < physicsFramesToCalculate; i++)
        {
            physicsScene.Simulate(Time.fixedDeltaTime);
            for (int o = 0; o < levelManager.GetAllBalls().Length; o++)
            {
                if (clonedBalls[o].WasCollidedByControllBall())
                {
                    levelManager.GetAllBalls()[o].SetNewPointForLineRenderer(clonedBalls[o].GetPos());
                }
            }
        }
    }
    public void ResetAllLinerenderers(Vector3 pos)
    {
        for (int i = 0; i < levelManager.GetAllBalls().Length; i++)
        {
            levelManager.GetAllBalls()[i].ResetLineRenderer();
        }
    }
    public void RemoveAndDestroyClonedBall(int indexToRemove)
    {
        Destroy(clonedBalls[indexToRemove].gameObject);
        clonedBalls.Remove(clonedBalls[indexToRemove]);
    }
    private void OnDisable()
    {
        EventManager.OnChangingPosOfPointer -= CalculateProjection;
        EventManager.OnPointerUp -= ResetAllLinerenderers;
    }
}
