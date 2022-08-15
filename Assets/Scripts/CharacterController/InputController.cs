using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    HeroMovement heroController;
    PlayerAction inputActions;

    public Vector2 moveAxis;
    public Vector3 mousePos;

    public LayerMask layerMask;

    public bool jump;
    public bool click;

    public bool alph1;
    public bool alph2;
    public bool alph3;
    public bool alph4;

    public EventGameObject eventGameObject;

    Camera camera;
    Vector3 forward;
    Vector3 right;

    Vector3 mmoveDirection;
    Vector3 inputMoveDirection;

    [Header("used for dibuging agrro radius")]
    public bool showAggro = true;
    public float aggroRange = .5f;
    public float maxRayDistance = 5f;
    public NPCController _target;

    bool isAttackable = false;
    void Awake()
    {
        inputActions = new PlayerAction();
    }
    
    void Start()
    {
        camera = Camera.main;
        forward = camera.transform.forward;
        right = camera.transform.right;
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        heroController = GetComponent<HeroMovement>();

        #region Read Input Event
        inputActions.Player.Move.performed += ctx => moveAxis.x = ctx.ReadValue<Vector2>().x;
        inputActions.Player.Move.canceled += ctx => moveAxis.x = ctx.ReadValue<Vector2>().x;

        inputActions.Player.Move.performed += ctx => moveAxis.y = ctx.ReadValue<Vector2>().y;
        inputActions.Player.Move.canceled += ctx => moveAxis.y = ctx.ReadValue<Vector2>().y;

        inputActions.Player.Jump.performed += ctx => jump = ctx.ReadValueAsButton();
        inputActions.Player.Jump.canceled += ctx => jump = ctx.ReadValueAsButton();

        inputActions.Player.Attack.performed += Onclick;// = ctx.ReadValueAsButton();
        inputActions.Player.Attack.canceled -= Onclick;//ctx => click = ctx.ReadValueAsButton();

        inputActions.Player.MousePosition.performed += ctx => mousePos = ctx.ReadValue<Vector2>();

        inputActions.Player.Alpha1.performed += ctx => alph1 = ctx.ReadValueAsButton();
        inputActions.Player.Alpha1.canceled += ctx => alph1 = ctx.ReadValueAsButton();

        inputActions.Player.Alpha2.performed += ctx => alph2 = ctx.ReadValueAsButton();
        inputActions.Player.Alpha2.canceled += ctx => alph2 = ctx.ReadValueAsButton();

        inputActions.Player.Alpha3.performed += ctx => alph3 = ctx.ReadValueAsButton();
        inputActions.Player.Alpha3.canceled += ctx => alph3 = ctx.ReadValueAsButton();

        inputActions.Player.Alpha4.performed += ctx => alph4 = ctx.ReadValueAsButton();
        inputActions.Player.Alpha4.canceled += ctx => alph4 = ctx.ReadValueAsButton();
        #endregion
    }

    private void Onclick(InputAction.CallbackContext obj)
    {
        if ( !isAttackable)
        {
            isAttackable = true;
            Debug.Log("1 click");
            DoAttack();
        }
    }

    void LateUpdate()
    {
       
            

        mmoveDirection = moveAxis;//forward * moveAxis.y + right * moveAxis.x;
         heroController.Move(mmoveDirection, false, jump);

        inputMoveDirection = forward * moveAxis.y + right * moveAxis.x;
        inputMoveDirection = inputMoveDirection.normalized;


        RaycastHit info;
        if (Physics.SphereCast(transform.position, aggroRange, inputMoveDirection, out info, maxRayDistance, layerMask))
        {
            if (info.collider.tag == "Attackable")
            {
                SetCurrentTarget(info.collider.GetComponent<NPCController>());
            }      
        }
    }
    void DoAttack()
    {
        if(_target != null)
        {
            heroController.AttackTarget(_target.gameObject);
        }else
        {
            heroController.AttackTarget(default);
        }
        isAttackable = false;
    }
    public NPCController CurrentTarget()
    {
        return _target;
    }
    public void SetCurrentTarget(NPCController target)
    {
        _target = target;
    }

    #region Input ENABLED/DESABLED
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
    #endregion
    private void OnDrawGizmos()
    {
        if (showAggro)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawRay(transform.position, inputMoveDirection);
            Gizmos.DrawWireSphere(transform.position, aggroRange);
            if (CurrentTarget() != null)
                Gizmos.DrawSphere(CurrentTarget().transform.position, aggroRange);
        }
    }
}
[System.Serializable]
public class EventGameObject : UnityEvent<GameObject> { }
