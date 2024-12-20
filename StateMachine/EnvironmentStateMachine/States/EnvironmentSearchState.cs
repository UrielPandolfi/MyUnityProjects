using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSearchState : EnvironmentState
{
    private bool isCloseEnough;
    private float _distanceApproachThresHold = 1.5f;
    public EnvironmentSearchState(EnvironmentStateContext context, EnvironmentStateManager.EnvironmentStates stateKey) : base(context, stateKey)
    {
        _context = context;
    }

    public override void OnEnterState()
    {
        isCloseEnough = false;
    }
    public override void OnUpdateState()
    {
        isCloseEnough = Vector3.Distance(_context.currentClosestPoint, _context.currentIkConstraint.data.root.position) < _distanceApproachThresHold
        && _context.currentClosestPoint != Vector3.positiveInfinity;
    }
    public override void OnExitState() {}
    public override EnvironmentStateManager.EnvironmentStates GetNextState()    // devolvemos la key del siguiente state
    {
        if(isCloseEnough)
        {
            return EnvironmentStateManager.EnvironmentStates.Approach;
        }

        if(_context.shouldReset)
        {
            return EnvironmentStateManager.EnvironmentStates.Reset;
        }
        
        return key; // ahora gracias a la baseState tenemos una Key
    }

    public override void OnTriggerEnter(Collider other)
    {
        StartIkTargetTracking(other);
    }
    public override void OnTriggerStay(Collider other)
    {
        UpdateIkTargetTracking(other);
    }
    public override void OnTriggerExit(Collider other)
    {
        ResetIkTargetTracking(other);
    }
}
