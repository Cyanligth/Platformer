using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase<TOwner> where TOwner : MonoBehaviour
{
    protected TOwner owner;

    public StateBase(TOwner owner)
    {
        this.owner = owner;
    }
    public abstract void SetUp();
    public abstract void Enter();
    public abstract void Update();
    public abstract void Transition();
    public abstract void Exit();
}
