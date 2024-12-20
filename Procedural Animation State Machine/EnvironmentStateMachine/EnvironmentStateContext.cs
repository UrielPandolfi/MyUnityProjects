using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnvironmentStateContext
{
    public enum side
    {
        RIGHT,
        LEFT
    }
    // Private properties
    private TwoBoneIKConstraint _leftArmIkConstraint;
    private TwoBoneIKConstraint _rightArmIkConstraint;
    private MultiRotationConstraint _leftArmMultiRotationConstraint;
    private MultiRotationConstraint _rightArmMultiRotationConstraint;
    private CharacterController _characterController;
    private float _shoulderHeight;
    private float _playerRootCenterHeight;
    private Vector3 _rightTargetDefaultPosition;
    private Vector3 _leftTargetDefaultPosition;

    public EnvironmentStateContext(TwoBoneIKConstraint leftArmIkConstraint, TwoBoneIKConstraint rightArmIkConstraint,
    MultiRotationConstraint leftArmMultiRotationConstraint, MultiRotationConstraint rightArmMultiRotationConstraint,
    CharacterController characterController, float shoulderHeight, float playerRootCenterHeight)
    {
        // Ik constraints
        _leftArmIkConstraint = leftArmIkConstraint;
        _rightArmIkConstraint = rightArmIkConstraint;
        _leftArmMultiRotationConstraint = leftArmMultiRotationConstraint;
        _rightArmMultiRotationConstraint = rightArmMultiRotationConstraint;

        // Collider y medidas del colider
        _characterController = characterController;
        _shoulderHeight = shoulderHeight;
        _playerRootCenterHeight = playerRootCenterHeight;

        // Seteamos la posicion default de los target
        _rightTargetDefaultPosition = rightArmIkConstraint.data.target.localPosition;
        _leftTargetDefaultPosition = leftArmIkConstraint.data.target.localPosition;

        // Para que no comience en null seteamos un lado
        SetCurrentSide(Vector3.positiveInfinity);
    }

    // read-only properties
    public TwoBoneIKConstraint leftArmIkConstraint => _leftArmIkConstraint;
    public TwoBoneIKConstraint rightArmIkConstraint => _rightArmIkConstraint;
    public MultiRotationConstraint leftArmMultiRotationConstraint => _leftArmMultiRotationConstraint;
    public MultiRotationConstraint rightArmMultiRotationConstraint => _rightArmMultiRotationConstraint;
    public CharacterController characterController => _characterController;
    public Transform playerRootColliderTransform => _characterController.transform;
    public float shoulderHeight => _shoulderHeight;
    public float playerRootCenterHeight => _playerRootCenterHeight;

    public float ClosestDistance = Mathf.Infinity;
    public bool shouldReset = false;
    public Collider currentCollider;
    public TwoBoneIKConstraint currentIkConstraint;
    public MultiRotationConstraint currentMultiRotation;
    public side currentSide;
    public Vector3 currentClosestPoint = Vector3.positiveInfinity;
    public Vector3 currentTargetDefaultPosition;
    public quaternion currentTargetDefaultRotation;
    public float targetOffsetY {get; set;} = 0f;

    public void SetCurrentSide(Vector3 closestPoint)
    {
        // Comprobamos cual es el lado mas cercano a traves del centro del playerCapsuleCollider
        Vector3 rightShoulder = rightArmIkConstraint.data.root.position;
        Vector3 leftShoulder = leftArmIkConstraint.data.root.position;
        
        bool isRightSide = Vector3.Distance(rightShoulder, closestPoint) < Vector3.Distance(leftShoulder, closestPoint);

        if(isRightSide)
        {
            currentSide = side.RIGHT;
            currentIkConstraint = rightArmIkConstraint; // con esto ya podemos acceder a todo, al target, root, etc.
            currentMultiRotation = rightArmMultiRotationConstraint;
            currentTargetDefaultPosition = _rightTargetDefaultPosition;
        }
        else
        {
            currentSide = side.LEFT;
            currentIkConstraint = leftArmIkConstraint; // con esto ya podemos acceder a todo, al target, root, etc.
            currentMultiRotation = leftArmMultiRotationConstraint;
            currentTargetDefaultPosition = _leftTargetDefaultPosition;
        }
    }
}
