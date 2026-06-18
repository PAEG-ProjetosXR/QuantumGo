using UnityEngine;
using Unity.VisualScripting;
using System;

public abstract class TargetState
{
    protected TargetController target;

    public TargetState(TargetController target)
    {
        this.target = target;
    }
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
