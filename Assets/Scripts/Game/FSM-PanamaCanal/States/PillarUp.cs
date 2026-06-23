using UnityEngine;

public class PillarUp : PanamaState
{
    public PillarUp(PanamaStateMachine _stateMachine, PanamaStateController _controller) : base(_stateMachine, _controller)
    {
    }

    Vector3 direction;
    GameObject pillarToMove;
    Vector3 offset;
    float distance;
    bool hasFlipped = false;


    public override void Enter()
    {
        direction = controller.isGoingDown? Vector3.left : Vector3.right;
        pillarToMove = controller.isPillarDown ? controller.currentPillar : controller.nextPillar;
        offset = new Vector3(((pillarToMove.transform.localScale.x/2) + 0.01f) * direction.x, 0);
        //RaycastHit2D nextWate = Physics2D.Raycast(pillarToMove.transform.position + offset, direction, 0.5f, controller.waterMask);
        RaycastHit2D nextWater = Physics2D.BoxCast(pillarToMove.transform.position + offset,pillarToMove.transform.localScale,0,direction,1,controller.waterMask);
        Debug.DrawRay(pillarToMove.transform.position + offset, direction * 0.5f,Color.magenta,6);
        //Physics2D.BoxCast(startPoint,controller.currentPillar.transform.localScale,0,controller.boat.transform.right,1,controller.waterMask);
        distance = nextWater.collider.gameObject.transform.localScale.x+0.1f;
        offset.y = (pillarToMove.transform.localScale.y / 2) + 0.01f;
    }
    
    public override void Update()
    {
        RaycastHit2D findPillar = Physics2D.Raycast(pillarToMove.transform.position + offset, direction, distance, controller.pillarMask);
        Debug.DrawRay(pillarToMove.transform.position + offset, direction * distance,Color.green);

        if (!findPillar)
        {
            //When the player is located between two already moved pillars, move water down
            if (controller.isPillarDown)
            {
                controller.isPillarDown = false;
                RaycastHit2D waterTag = Physics2D.Raycast(controller.boat.transform.position, Vector3.down, controller.boat.transform.localScale.y, controller.waterMask);
                //If water is down move it up
                if (waterTag.collider.gameObject.CompareTag(PanamaStateController.k_Down))
                {
                    stateMachine.ChangeState(controller.BoatUp);
                }
                //If water is up, move it down
                else if (waterTag.collider.gameObject.CompareTag(PanamaStateController.k_Up))
                {
                    stateMachine.ChangeState(controller.BoatDown);
                }
                //Water shall not ve moved
                else
                {
                    hasFlipped = !hasFlipped;
                    //First time after flip, put the up pillar down
                    if (hasFlipped)
                    {
                        controller.isPillarDown = true;
                        stateMachine.ChangeState(controller.PillarUp);
                    }
                    //Second time after flip, restart the logic, flip the movement tipy
                    else
                    {
                        hasFlipped = !hasFlipped;
                        controller.isGoingDown = !controller.isGoingDown;

                        //Clear all cathys
                        controller.currentPillar = null;
                        controller.nextPillar = null;
                        controller.water = null;

                        stateMachine.ChangeState(controller.MoveBoat);
                    }
                        
                }
            }
            //Adjust the water
            else
            {
                if (controller.isGoingDown)
                {
                    stateMachine.ChangeState(controller.WaterUp);
                }
                else
                {
                    stateMachine.ChangeState(controller.WaterDown);
                }
            }
            
        }
        else
        {
           pillarToMove.transform.Translate(Vector3.up * Time.deltaTime);
        }
    }

    public override void Exit()
    {
    }

   
}