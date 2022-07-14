using UnityEngine;
using UnityEngine.EventSystems;

public class TouchField : MonoBehaviour
{
    private bool isMobile, Pressed;
    Vector3 OldPos;
    [SerializeField] private Camera controlCamera;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private ControllBall controllBall;
    [SerializeField] private LineRenderer lineRenderer;
    private void Start()
    {
        isMobile = Application.isMobilePlatform;
    }

    private void Update()
    {
        if (isMobile)
        {
            ControllByMobile();
        }
        else
        {
            ControlByMouse();
        }
    }
    void ControllByMobile()
    {
        if (Input.touchCount != 0)
        {
            Pressed = true;
            OldPos = Input.GetTouch(0).position;
            if (levelManager.CanContinuePlay())
            {
                EventManager.ChangingPosOfPointer(CastRay(OldPos));
            }
        }
        else if(Pressed && Input.touchCount == 0)
        {
            Pressed = false;
            if (levelManager.CanContinuePlay())
            {
                lineRenderer.positionCount = 0;

                EventManager.PointerUp(CastRay(OldPos));
            }
        }
    }
    void ControlByMouse()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            Pressed = true;
            OldPos = Input.mousePosition;
            if (levelManager.CanContinuePlay())
            {
                EventManager.ChangingPosOfPointer(CastRay(Input.mousePosition));
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Pressed = false;
            if (levelManager.CanContinuePlay())
            {
                lineRenderer.positionCount = 0;
                EventManager.PointerUp(CastRay(Input.mousePosition));
            }
            
        }
        else if( Pressed && Input.mousePosition!=OldPos)
        {
            OldPos = Input.mousePosition;
            if (levelManager.CanContinuePlay())
            {
                EventManager.ChangingPosOfPointer(CastRay(Input.mousePosition));
            }
        }
    }
    private Vector3 CastRay(Vector3 posOfPointer)
    {
        Ray rayFromCam = controlCamera.ScreenPointToRay(posOfPointer);
        RaycastHit raycastHit;
        Physics.Raycast(rayFromCam, out raycastHit);

        SetLineRenderer(raycastHit.point);

        return raycastHit.point;
    }
    private void SetLineRenderer(Vector3 pos)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(1, pos);
        lineRenderer.SetPosition(0, controllBall.GetPos()) ;

    }
}
