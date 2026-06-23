using UnityEngine; 

public class BoatUp : PanamaState
{
    public BoatUp(PanamaStateMachine _stateMachine, PanamaStateController _controller) : base(_stateMachine, _controller)
    {
    }

    Vector3 offset;
    float distance;

    public override void Enter()
    {
        //Cast from the top corner, to the boats direction
        offset = new Vector3((controller.water.transform.localScale.x / 2) + 0.01f, (controller.water.transform.localScale.y / 2) + 0.01f);
        offset.x *= controller.boat.transform.right.x;
        distance = controller.nextPillar.transform.localScale.x;
    }

    public override void Update()
    {
        
        //Find water
        RaycastHit2D findWater = Physics2D.Raycast(controller.water.transform.position + offset, controller.boat.transform.right, distance, controller.waterMask);

        //No water is found, we cant get much higher
        if (!findWater)
        {
            stateMachine.ChangeState(controller.MoveBoat);
        }
        //Can we get much higher?
        else
        {
            controller.boat.transform.Translate(Vector3.up * Time.deltaTime);
            controller.water.transform.Translate(Vector3.up * Time.deltaTime);
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