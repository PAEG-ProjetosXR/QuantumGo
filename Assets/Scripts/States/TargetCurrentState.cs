using UnityEngine;
using System;

public class TargetIdleState : TargetState
{
    public TargetIdleState(TargetController target) : base(target) { }

    public override void Enter()
    {
        target.currentStateVisual = State.idle;
    }

    public override void Update()
    {
        target.ExecuteIdle();
    }

    public override void Exit() { }
}

public class TargetWalkState : TargetState
{
    public TargetWalkState(TargetController target) : base(target) { }

    public override void Enter()
    {
        target.currentStateVisual = State.walk;
    }

    public override void Update()
    {
        target.ExecuteWalk();
    }

    public override void Exit() { }
}

public class TargetRunState : TargetState
{
    public TargetRunState(TargetController target) : base(target) { }

    public override void Enter()
    {
        target.currentStateVisual = State.run;
    }

    public override void Update()
    {
        target.ExecuteRun();
    }

    public override void Exit() { }
}

public class TargetSpinState : TargetState
{
    public TargetSpinState(TargetController target) : base(target) { }

    public override void Enter()
    {
        target.currentStateVisual = State.spin;
    }

    public override void Update()
    {
        target.ExecuteSpin();
    }

    public override void Exit() { }
}

public class TargetJumpState : TargetState
{
    public TargetJumpState(TargetController target) : base(target) { }

    public override void Enter()
    {
        target.currentStateVisual = State.jump;
        target.ExecuteJump();
    }

    public override void Update()
    {
        if (!target.isGround && target.rb != null)
        {
            Vector3 speed = target.rb.linearVelocity;
            speed.x = 0; speed.z = 0;
            target.rb.linearVelocity = speed;
        }
    }

    public override void Exit() { }
}
