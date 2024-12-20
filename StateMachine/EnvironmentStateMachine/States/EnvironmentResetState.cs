using Unity.Mathematics;
using UnityEngine;

public class EnvironmentResetState : EnvironmentState
{
    private float _lerp = 10.0f;
    private float _targetWeight = 0f;
    private float _rotationWeight = 0f;
    private float _timeCounter = 0f;
    private float _transitionCooldown = 2f;
    private float _rotationSpeed = 500f;
    
    public EnvironmentResetState(EnvironmentStateContext context, EnvironmentStateManager.EnvironmentStates stateKey) : base(context, stateKey)
    {
        _context = context;
    }

    public override void OnEnterState()
    {
        Debug.Log("State: Reset");
        _timeCounter = 0f;
        _context.currentClosestPoint = Vector3.positiveInfinity;
        _context.currentCollider = null;
        _context.ClosestDistance = Mathf.Infinity;
        _context.shouldReset = false;
    }
    public override void OnUpdateState()
    {
        _timeCounter += Time.deltaTime;
        // hacer el movimiento suave de la mano a la altura de la cintura. La transicion dura el valor de lerp, osea 2 segundos en este caso
        _context.targetOffsetY = Mathf.Lerp(_context.targetOffsetY, _context.playerRootCenterHeight - _context.shoulderHeight, _timeCounter / _lerp);

        // Devolver el peso al valor default
        _context.currentIkConstraint.weight = Mathf.Lerp(_context.currentIkConstraint.weight, _targetWeight, _timeCounter / _lerp);
        _context.currentMultiRotation.weight = Mathf.Lerp(_context.currentMultiRotation.weight, _rotationWeight, _timeCounter / _lerp);

        // Devolver los target a la posicion inicial
        
        _context.currentIkConstraint.data.target.transform.localPosition = Vector3.Lerp(_context.currentIkConstraint.data.target.transform.localPosition, _context.currentTargetDefaultPosition,
        _timeCounter / _lerp);

        // Devolver los target a la rotación inicial
        _context.currentIkConstraint.data.target.transform.rotation = Quaternion.RotateTowards(_context.currentIkConstraint.data.target.transform.rotation, _context.currentTargetDefaultRotation,
        _rotationSpeed * Time.deltaTime);
    }
    public override void OnExitState() {}
    public override EnvironmentStateManager.EnvironmentStates GetNextState()    // devolvemos la key del siguiente state
    {
        bool isMoving = _context.characterController.velocity.magnitude > 0.01f;

        if(_timeCounter > _transitionCooldown){
            return EnvironmentStateManager.EnvironmentStates.Search;    // Si paso el contador, devolvemos search.
        }

        return key; // ahora gracias a la baseState tenemos una Key
    }

    public override void OnTriggerEnter(Collider other)
    {

    }
    public override void OnTriggerStay(Collider other)
    {

    }
    public override void OnTriggerExit(Collider other)
    {

    }
}
