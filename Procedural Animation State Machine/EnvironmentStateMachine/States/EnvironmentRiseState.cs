using Unity.Mathematics;
using UnityEngine;

public class EnvironmentRiseState : EnvironmentState
{
    private float _lerp = 5f;
    private float _targetWeight = 0.95f;
    private float _rotationWeight = 0.95f;
    private float _timeCounter = 0f;
    private float _touchTime = 1f;
    private quaternion _handTargetRotation;
    private float _rotationSpeed = 500f;
    private LayerMask _interactableLayer = LayerMask.GetMask("Interactable");
    public EnvironmentRiseState(EnvironmentStateContext context, EnvironmentStateManager.EnvironmentStates stateKey) : base(context, stateKey)
    {
        _context = context;
    }

    public override void OnEnterState()
    {
        Debug.Log("State: Rise");
        _timeCounter = 0f;
    }
    public override void OnUpdateState()
    {
        // necesitamos levantar la mano y rotarla hacia la pared
        _timeCounter += Time.deltaTime;

        _context.targetOffsetY = Mathf.Lerp(_context.targetOffsetY, 0f, _timeCounter / _lerp);
        _context.currentIkConstraint.weight = Mathf.Lerp(_context.currentIkConstraint.weight, _targetWeight, _timeCounter / _lerp);

        CalculateLookRotation();
        _context.currentIkConstraint.data.target.transform.rotation = Quaternion.RotateTowards(_context.currentIkConstraint.data.target.transform.rotation,
        _handTargetRotation, _rotationSpeed * Time.deltaTime);
        _context.currentMultiRotation.weight = Mathf.Lerp(_context.currentMultiRotation.weight, _rotationWeight, _timeCounter / _lerp);
    }
    public override void OnExitState() {}
    public override EnvironmentStateManager.EnvironmentStates GetNextState()    // devolvemos la key del siguiente state
    {
        bool endTouch = _timeCounter > _touchTime;

        if(endTouch || resetCheck())
        {
            return EnvironmentStateManager.EnvironmentStates.Reset;
        }

        return key;
    }

    private void CalculateLookRotation()
    {
        // calcular el punto de rotacion de la mano
        Vector3 rayDirection =  _context.currentClosestPoint - _context.currentIkConstraint.data.root.position;
        Vector3 directionNormalized = rayDirection.normalized;

        Debug.Log("Ray direction: " + directionNormalized);
        RaycastHit hit;
        if(Physics.Raycast(_context.currentClosestPoint, directionNormalized, out hit, 5f, _interactableLayer))
        {
            Vector3 normal = hit.normal; // Esto devuelve el angulo hacia donde apunta la superficie
            Vector3 surfaceNormal = -normal;
            _handTargetRotation = quaternion.LookRotation(surfaceNormal, Vector3.up);
            Debug.Log("hand target: " + _handTargetRotation);
        }
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

