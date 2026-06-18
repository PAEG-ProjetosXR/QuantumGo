using UnityEngine;
using System;

public enum State
{
    idle = 0,
    walk,
    run,
    spin,
    jump
}
public class TargetController : MonoBehaviour
{
    public State currentStateVisual = State.idle;
    private TargetState currentState;

    [Header("Moveset")]
    public float walkspeed;
    public float runspeed;
    public float spinspeed;
    public float jumpspeed;

    public Rigidbody rb;
    public bool isGround = true;

    public TargetIdleState IdleState { get; private set; }
    public TargetWalkState WalkState { get; private set; }
    public TargetRunState RunState { get; private set; }
    public TargetSpinState SpinState { get; private set; }

    public TargetJumpState JumpState { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Instancia os estados passando a referência deste script (this)
        IdleState = new TargetIdleState(this);
        WalkState = new TargetWalkState(this);
        RunState = new TargetRunState(this);
        SpinState = new TargetSpinState(this);
        JumpState = new TargetJumpState(this);
    }

    private void Start()
    {
        ChangeState(IdleState);
    }

    private void Update()
    {
        currentState?.Update(); //Roda o Update do estado atual
    }
    
    public void ChangeState(TargetState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void ExecuteIdle()
    {
        if (rb != null)
        {
            Vector3 speed = rb.linearVelocity;
            speed.x = 0; speed.z = 0;
            rb.linearVelocity = speed;
        }
    }

    public void ExecuteWalk()
    {
        if (rb != null)
        {
            Vector3 forwardDirection = transform.forward * walkspeed;
            rb.linearVelocity = new Vector3(forwardDirection.x, rb.linearVelocity.y, forwardDirection.z);
        }
    }

    public void ExecuteRun()
    {
        if (rb != null)
        {
            Vector3 forwardDirection = transform.forward * runspeed;
            rb.linearVelocity = new Vector3(forwardDirection.x, rb.linearVelocity.y, forwardDirection.z);
        }
    }

    public void ExecuteSpin()
    {
        if(rb != null)
        {
            //Gira ele mantendo ele parado no lugar
            ExecuteIdle();
            transform.Rotate(Vector3.up * spinspeed * Time.deltaTime);
        }
    }

    public void ExecuteJump()
    {
        if (rb != null && isGround)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpspeed, ForceMode.Impulse);
            isGround = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.6f)
        {
            isGround = true;

            if (currentStateVisual == State.jump)
            {
                ChangeState(IdleState);
            }
        }
    }
}
