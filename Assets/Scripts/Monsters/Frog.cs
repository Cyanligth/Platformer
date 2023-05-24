using FrogState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    public State curState;
    public Transform target;
    public Vector3 returnPosition;
    public StateBase<Frog>[] states;
    public Rigidbody2D rb;

    [SerializeField] public float moveSpeed;
    [SerializeField] public float findRange;
    [SerializeField] public float attackRange;
    [SerializeField] public float runawayRange;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        states = new StateBase<Frog>[(int)State.Size];
        states[(int)State.Idle] = new IdleState(this);
        states[(int)State.Trace] = new TraceState(this);
        states[(int)State.Return] = new ReturnState(this);
        states[(int)State.Attack] = new AttackState(this);
        states[(int)State.Runaway] = new RunawayState(this);

    }

    private void Start()
    {
        curState = State.Idle;
        states[(int)curState].Enter();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        returnPosition = transform.position;
    }

    private void Update()
    {
        states[(int)curState].Update();
    }
    public void ChangeState(State state)
    {
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Enter();
    }
}

namespace FrogState
{
    public enum State { Idle, Trace, Return, Attack, Runaway, Size }
    public class IdleState : StateBase<Frog>
    {
        private float idleTime;
        public IdleState(Frog owner) : base(owner) { }

        public override void SetUp()
        {

        }
        public override void Enter()
        {
            Debug.Log("IdleEnter");
            idleTime = 0;
        }
        public override void Update()
        {
            idleTime += Time.deltaTime;
            owner.rb.velocity = Vector3.zero;
            if (Vector2.Distance(owner.target.position, owner.transform.position) < owner.findRange)
            {
                owner.ChangeState(State.Trace);
            }
        }
        public override void Exit()
        {
            Debug.Log("IdleExit");
        }
        public override void Transition() { }
    }

    public class TraceState : StateBase<Frog>
    {
        public TraceState(Frog owner) : base(owner) { }

        public override void SetUp()
        {

        }
        public override void Enter()
        {
            Debug.Log("TraceEnter");
        }
        public override void Update()
        {
            Vector2 dir = (owner.target.position - owner.transform.position).normalized;
            owner.rb.velocity = dir * owner.moveSpeed;

            if (Vector2.Distance(owner.target.position, owner.transform.position) > owner.findRange)
            {
                owner.ChangeState(State.Return);
            }
            else if (Vector2.Distance(owner.target.position, owner.transform.position) < owner.attackRange)
            {
                owner.ChangeState(State.Attack);
            }
        }
        public override void Exit()
        {
            Debug.Log("TraceExit");
        }
        public override void Transition() { }
    }

    public class ReturnState : StateBase<Frog>
    {
        public ReturnState(Frog owner) : base(owner) { }

        public override void SetUp()
        {

        }
        public override void Enter()
        {
            Debug.Log("ReturnEnter");
        }
        public override void Update()
        {
            Vector2 dir = (owner.returnPosition - owner.transform.position).normalized;
            owner.rb.velocity = dir * owner.moveSpeed;

            if (Vector2.Distance(owner.returnPosition, owner.transform.position) <= 0.1f)
            {
                owner.ChangeState(State.Idle);
            }
            else if (Vector2.Distance(owner.target.position, owner.transform.position) < owner.findRange)
            {
                owner.ChangeState(State.Trace);
            }
        }
        public override void Exit()
        {
            Debug.Log("RerurnExit");
        }
        public override void Transition() { }
    }

    public class AttackState : StateBase<Frog>
    {
        private float canAtk = 1;

        public AttackState(Frog owner) : base(owner) { }
        public override void SetUp()
        {

        }
        public override void Enter()
        {
            Debug.Log("AttackEnter");
        }
        public override void Update()
        {
            if (canAtk >= 1)
            {
                Debug.Log("개구리공격");
                canAtk = 0;
            }
            canAtk += Time.deltaTime;
            if (Vector2.Distance(owner.target.position, owner.transform.position) > owner.attackRange)
            {
                owner.ChangeState(State.Trace);
            }
            else if(Vector2.Distance(owner.target.position, owner.transform.position) < owner.runawayRange)
            {
                owner.ChangeState(State.Runaway);
            }
        }
        public override void Exit()
        {
            Debug.Log("AttackExit");
        }
        public override void Transition() { }
    }
    public class RunawayState : StateBase<Frog>
    {
        public RunawayState(Frog owner) : base(owner) { }
        public override void SetUp()
        {

        }
        public override void Enter()
        {
            Debug.Log("RunawayEnter");
        }
        public override void Update()
        {
            Vector2 dir = (owner.target.position - owner.transform.position).normalized;
            owner.rb.velocity = -dir * owner.moveSpeed;

            if (Vector2.Distance(owner.target.position, owner.transform.position) > owner.runawayRange)
            {
                owner.ChangeState(State.Attack);
            }
        }
        public override void Exit()
        {
            Debug.Log("RunawayExit");
        }
        public override void Transition() { }
    }
}
