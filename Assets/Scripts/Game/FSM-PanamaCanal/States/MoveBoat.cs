using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveBoat : PanamaState
{
    public MoveBoat(PanamaStateMachine _stateMachine, PanamaStateController _controller) : base(_stateMachine, _controller)
    {
    }


    public override void Enter()
    {
    }

    public override void Update()
    {
        //Find pillar
        RaycastHit2D findPillar = Physics2D.Raycast(controller.boat.transform.position, controller.boat.transform.right, (controller.boat.transform.localScale.x/2) + 0.2f,controller.pillarMask);

        //Pillar finded
        if (findPillar)
        {
            

            //Get next zone info and adjust it
            if (controller.nextPillar == null)
            {
                //Save current pillar
                controller.currentPillar = findPillar.collider.gameObject;

                //Save start point for detecting water and next pillar
                Vector3 startPoint = controller.currentPillar.transform.position;
                startPoint.x += ((controller.currentPillar.transform.localScale.x / 2) + 0.05f) * controller.boat.transform.right.x;
                float distance = (controller.currentPillar.transform.localScale.x / 2);

                //Get and save current water
                RaycastHit2D waterDetection = Physics2D.BoxCast(startPoint,controller.currentPillar.transform.localScale,0,controller.boat.transform.right,1,controller.waterMask);
                Debug.DrawRay(startPoint, controller.currentPillar.transform.right * distance, Color.green, 2.5f);
                controller.water = waterDetection.collider.gameObject;

                //Get next pillar
                distance = (controller.water.transform.localScale.x);
                RaycastHit2D nextPillarDetection = Physics2D.Raycast(startPoint, controller.boat.transform.right, distance, controller.pillarMask);
                Debug.DrawRay(startPoint, controller.boat.transform.right * distance, Color.magenta, 2.5f);
                controller.nextPillar = nextPillarDetection.collider.gameObject;

                stateMachine.ChangeState(controller.PillarUp);
            }
            else
            {
                //Rotate at end points
                if (findPillar.collider.gameObject.CompareTag("End"))
                {
                    controller.boat.transform.Rotate(Vector3.up, 180);
                }
                stateMachine.ChangeState(controller.PillarUp);
            }
        }
        //Move rigth until finds something
        else 
        {
            controller.boat.transform.Translate(Vector3.right * Time.deltaTime);
        }
    }

    public override void Exit ()
    {
        
    }
}
