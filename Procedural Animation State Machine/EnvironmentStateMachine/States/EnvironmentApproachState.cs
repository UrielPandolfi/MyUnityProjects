using Unity.Mathematics;
using UnityEngine;

public class EnvironmentApproachState : EnvironmentState
{   
    private float _lerp = 6f;
    private float _targetWeight = .5f;
    private float _rotationWeight = .8f;
    private float _timeCounter = 0f;
    private float _stateThreshold = 3f; // Si pasa este tiempo y no se cambio de animacion, se llama a state reset
    private float _rotationSpeed = 500f; // Esto luego se multiplica por time.deltaTime que es un valor muy chiquito
    private float _armReach = 0.6f; // Alcance del brazo
    public EnvironmentApproachState(EnvironmentStateContext context, EnvironmentStateManager.EnvironmentStates stateKey) : base(context, stateKey)
    {
        _context = context;
    }

    public override void OnEnterState()
    {
        Debug.Log("State: Approach");
        _timeCounter = 0f;
    }
    public override void OnUpdateState()
    {
        _timeCounter += Time.deltaTime;
        _context.currentIkConstraint.weight = Mathf.Lerp(_context.currentIkConstraint.weight, _targetWeight, _timeCounter / _lerp);
        // Hacemos lo mismo para la rotacion
        quaternion targetRotation = Quaternion.LookRotation(-Vector3.up, _context.playerRootColliderTransform.transform.forward);
        _context.currentIkConstraint.data.target.transform.rotation = Quaternion.RotateTowards(_context.currentIkConstraint.data.target.transform.rotation,
        targetRotation, _rotationSpeed * Time.deltaTime);
        _context.currentMultiRotation.weight = Mathf.Lerp(_context.currentMultiRotation.weight, _rotationWeight, _timeCounter / _lerp);
    }
    public override void OnExitState() {}
    public override EnvironmentStateManager.EnvironmentStates GetNextState()    // devolvemos la key del siguiente state
    {
        // esta dentro del rango del brazo
        bool isWithinArmsReach = Vector3.Distance(_context.currentIkConstraint.data.root.position, _context.currentClosestPoint) < _armReach;
        bool isTimeOver = _timeCounter > _stateThreshold;
        if(isTimeOver || resetCheck())
        {
            return EnvironmentStateManager.EnvironmentStates.Reset;
        }

        if(isWithinArmsReach)
        {
            return EnvironmentStateManager.EnvironmentStates.Rise;
        }
        
        return key;
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
