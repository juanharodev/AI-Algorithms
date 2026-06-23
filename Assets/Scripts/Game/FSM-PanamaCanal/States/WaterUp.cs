using UnityEngine;

public class WaterUp : PanamaState
{
    public WaterUp(PanamaStateMachine _stateMachine, PanamaStateController _controller) : base(_stateMachine, _controller)
    {
    }
    Vector3 rayDirection;
    Vector3 offset;
    float distance;

    public override void Enter()
    {
        rayDirection = controller.isGoingDown ? -controller.boat.transform.right : controller.boat.transform.right;
        distance = controller.currentPillar.transform.localScale.x;
        offset = new Vector3(((controller.water.transform.localScale.x / 2) + 0.05f) * (rayDirection.x), (controller.water.transform.localScale.y / 2) + 0.01f);
    }
    public override void Update()
    {

        //Find water
        RaycastHit2D findWater = Physics2D.Raycast(controller.water.transform.position + offset, rayDirection, distance, controller.waterMask);


        //Water has been found 
        if (!findWater)
        {
            stateMachine.ChangeState(controller.PillarDown);
        }
        else
        {
            controller.water.transform.Translate(Vector3.up * Time.deltaTime);
        }
    }

    public override void Exit()
    {
        if (controller.water.CompareTag(PanamaStateController.k_Down) || controller.water.CompareTag(PanamaStateController.k_Up))
        {
            controller.water.tag = PanamaStateController.k_Up;
        }
    }
}