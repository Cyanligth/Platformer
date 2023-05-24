using BeeState;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bee : MonoBehaviour
{
    public State curState;
    public Transform target;
    public Vector3 returnPosition;
    public StateBase<Bee>[] states;
    public Rigidbody2D rb;

    [SerializeField] public float moveSpeed;
    [SerializeField] public float range;
    [SerializeField] public float attackRange;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        states = new StateBase<Bee>[(int)State.Size];
        states[(int)State.Idle] = new IdleState(this);
        states[(int)State.Trace] = new TraceState(this);
        states[(int)State.Return] = new ReturnState(this);
        states[(int)State.Attack] = new AttackState(this);

    }

    private void Start()
    {
        curState = State.Idle;
        states[(int)curState].Enter();
        rb.gravityScale = 0f;
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

namespace BeeState
{
    public enum State { Idle, Trace, Return, Attack, Die, Size }
    public class IdleState : StateBase<Bee>
    {
        private float idleTime;
        public IdleState(Bee owner) : base(owner) { }

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
            if (Vector2.Distance(owner.target.position, owner.transform.position) < owner.range)
            {
                owner.ChangeState(State.Trace);
            }
        }
        public override void Exit()
        {
            Debug.Log("IdleExit");
        }
        public override void Transition(){}
    }

    public class TraceState : StateBase<Bee>
    {
        public TraceState(Bee owner) : base(owner) { }

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

            if (Vector2.Distance(owner.target.position, owner.transform.position) > owner.range)
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

    public class ReturnState : StateBase<Bee>
    {
        public ReturnState(Bee owner) : base(owner) { }

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
            else if (Vector2.Distance(owner.target.position, owner.transform.position) < owner.range)
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

    public class AttackState : StateBase<Bee>
    {
        private float canAtk = 1; 

        public AttackState(Bee owner) : base(owner) { }
        public override void SetUp()
        {

        }
        public override void Enter()
        {
            Debug.Log("AttackEnter");
        }
        public override void Update()
        {
            if(canAtk >= 1)
            {
                Debug.Log("АјАн!");
                canAtk = 0;
            }
            canAtk += Time.deltaTime;
            if (Vector2.Distance(owner.target.position, owner.transform.position) > owner.attackRange)
            {
                owner.ChangeState(State.Trace);
            }
        }
        public override void Exit()
        {
            Debug.Log("AttackExit");
        }
        public override void Transition() { }
    }
}

