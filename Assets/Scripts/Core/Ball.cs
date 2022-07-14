using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] protected Transform thisTransform;
    [SerializeField] protected Rigidbody thisRigidbody;
    [SerializeField] private LineRenderer thisLineRenderer;
    [SerializeField] private MeshRenderer thisMeshRenderer;
    private bool WasCollided;
    protected bool PhysicsBall;

    public Vector3 GetPos()
    {
        return thisTransform.position;
    }
    public void ResetThisBall(Vector3 pos)
    {
        thisTransform.position = pos;
        thisRigidbody.velocity = Vector3.zero;
        thisRigidbody.angularVelocity = Vector3.zero;
        WasCollided = false;
    }
    public virtual void PrepareBallForPhysicsScene()
    {
        thisMeshRenderer.enabled = false;
        thisLineRenderer.enabled = false;
        PhysicsBall = true;
    }
    public void SetNewPointForLineRenderer(Vector3 pos)
    {
        thisLineRenderer.positionCount = thisLineRenderer.positionCount + 1;
        thisLineRenderer.SetPosition(thisLineRenderer.positionCount - 1, pos);
    }
    public void ResetLineRenderer()
    {
        thisLineRenderer.positionCount = 0;
    }
    public virtual bool WasCollidedByControllBall()
    {
        return WasCollided;
    }
    private void Update()
    {
        if (thisRigidbody.velocity.magnitude < 1f)
        {
            thisRigidbody.velocity = Vector3.zero;
            thisRigidbody.angularVelocity = Vector3.zero;
        }
    }
    public bool BallStopped()
    {
        if (thisRigidbody.velocity.magnitude < 0.01f)
        {
            return true;
        }
        return false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (PhysicsBall && collision.gameObject.CompareTag("ControlBall"))
        {
            WasCollided = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (PhysicsBall == false && other.CompareTag("Hole"))
        {
            EventManager.BalFallenIntoHole(this);
        }
    }
}
