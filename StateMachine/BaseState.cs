using System;
using UnityEngine;

public abstract class BaseState<EState> where EState : Enum
{
    public EState key {get; private set;}
    public BaseState(EState enumKey) // siempre que se cree un BaseState se le debe dar una key
    {
        key = enumKey;
    }
    public abstract void OnEnterState();
    public abstract void OnUpdateState();
    public abstract void OnExitState();
    public abstract EState GetNextState(); // devolvemos la key del siguiente state
    public abstract void OnTriggerEnter(Collider other);
    public abstract void OnTriggerStay(Collider other);
    public abstract void OnTriggerExit(Collider other);
}
