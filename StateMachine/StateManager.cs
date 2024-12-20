using System;
using System.Collections.Generic;
using Photon.Pun.Demo.Cockpit;
using Unity.VisualScripting;
using UnityEngine;

public class StateManager<EState> : MonoBehaviour where EState : Enum
{
    //diccionario que guardara todas las instancias de los states
    protected Dictionary<EState, BaseState<EState>> states = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> currentState;

    void Start()
    {
        currentState.OnEnterState();
    }

    void Update()
    {
        EState nextStateKey = currentState.GetNextState();

        if(nextStateKey.Equals(currentState.key)) // si el que sigue es el que esta ahora
        {
            currentState.OnUpdateState();
        }
        else
        {
            Transition(currentState);
        }
    }

    private void Transition(BaseState<EState> state)
    {
        // hacemos la transicion
        currentState.OnExitState();
        currentState = states[currentState.GetNextState()]; // Cambiamos al siguiente state
        currentState.OnEnterState();
    }

    void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }

    void OnTriggerStay(Collider other)
    {
        currentState.OnTriggerStay(other);
    }

    void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(other);
    }
}
