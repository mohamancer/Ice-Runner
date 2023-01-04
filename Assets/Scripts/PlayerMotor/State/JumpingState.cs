using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : BaseState
{
    public float jumpForce = 7.0f;
    public override void Construct()
    {
        motor.verticalVeclocity = jumpForce;
        motor.anim?.SetTrigger("Jump");
    }

    public override Vector3 ProcessMotion()
    {
        //Apply gravity

        motor.ApplyGravity();

        Vector3 m = Vector3.zero;

        m.x = motor.SnapToLane();
        m.y = motor.verticalVeclocity;
        m.z = motor.baseRunSpeed;

        return m;
    }

    public override void Transition()
    {
        if (InputManager.Instance.SwipeLeft)
            motor.ChangeLane(-1);

        if (InputManager.Instance.SwipeRight)
            motor.ChangeLane(1);
        if (motor.verticalVeclocity < 0)
            motor.ChangeState(GetComponent<FallingState>());
    }
}
