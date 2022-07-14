using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllBall : Ball
{
    Vector3 StartPos;
    private void Start()
    {
        EventManager.OnPointerUp+=GetForce;
        StartPos = thisTransform.position;
    }
    public void GetForce(Vector3 posOfFinger)
    {
        posOfFinger = new Vector3(posOfFinger.x,0, posOfFinger.z);
        Vector3 posOfBallWithY0 = new Vector3(thisTransform.position.x,0, thisTransform.position.z);
        thisRigidbody.velocity = (posOfBallWithY0 - posOfFinger)*2;
    }
    public override void PrepareBallForPhysicsScene()
    {
        base.PrepareBallForPhysicsScene();
        EventManager.OnPointerUp -= GetForce;
    }
    public override bool WasCollidedByControllBall()
    {
        return true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (PhysicsBall == false && other.CompareTag("Hole"))
        {
            ResetThisBall(StartPos);
        }
    }
    private void OnDisable()
    {
        EventManager.OnPointerUp -= GetForce;
    }
}
