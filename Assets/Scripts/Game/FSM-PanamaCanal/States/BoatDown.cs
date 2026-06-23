
using UnityEngine;

public class BoatDown : PanamaState
{
    public BoatDown(PanamaStateMachine _stateMachine, PanamaStateController _controller) : base(_stateMachine, _controller)
    {
    }

    
    float distance;

    public override void Enter()
    {
        //Set a fixed distance to detect when boat is near ground
        distance = controller.boat.transform.localScale.y;
    }

    public override void Update()
    {

        //Find water
        RaycastHit2D findLand = Physics2D.Raycast(controller.boat.transform.position,Vector3.down, distance, controller.landMask);

        //Land was found, keep moving the boat
        if (findLand)
        {
            stateMachine.ChangeState(controller.MoveBoat);
        }
        //Boat and water go down down
        else
        {
            controller.boat.transform.Translate(Vector3.down * Time.deltaTime);
            controller.water.transform.Translate(Vector3.down * Time.deltaTime);
        }
    }

    public override void Exit()
    {
        //Clear all cathys
        controller.isPillarDown = false;
        controller.currentPillar = null;
        controller.nextPillar = null;
        controller.water = null;

    }
}