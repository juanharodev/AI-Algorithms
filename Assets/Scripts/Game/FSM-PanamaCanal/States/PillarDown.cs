using UnityEngine;

public class PillarDown : PanamaState
{
    public PillarDown(PanamaStateMachine _stateMachine, PanamaStateController _controller) : base(_stateMachine, _controller)
    {
    }

    Vector3 rayDirection;
    Vector3 offset;
    float distance;

    public override void Enter()
    {
        rayDirection = controller.boat.transform.position.x < controller.currentPillar.transform.position.x ? Vector3.left : Vector3.right;
        offset = new Vector3(0, (controller.currentPillar.transform.localScale.y / 2) + 0.01f);
        distance = controller.currentPillar.transform.localScale.x + (0.01f * rayDirection.x);
    }
    public override void Update()
    {
        //Find water
        RaycastHit2D findWater = Physics2D.Raycast(controller.currentPillar.transform.position + offset, rayDirection, distance, controller.waterMask);


        //Water has been found
        if (findWater)
        {
            //Star moving the boat
            stateMachine.ChangeState(controller.MoveBoat);
        }
        //Move the pillar down
        else
        {
            controller.currentPillar.transform.Translate(Vector3.down * Time.deltaTime);
        }
    }

    public override void Exit()
    {
        //Set that we have a pillar down
        controller.isPillarDown = true;
    }

    
}