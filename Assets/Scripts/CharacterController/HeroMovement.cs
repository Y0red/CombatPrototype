using System.Collections;
using UnityEngine;

public class HeroMovement : MonoBehaviour
{
    #region Variables
    private Animator anim;
    private Camera cam;
    private CharacterController controller;

    private Vector3 desiredMoveDirection;

    private Vector3 velocity;

    public Transform groundCheck;

    public LayerMask groundMask;

    [Header("Settings")]
    [SerializeField] float movementSpeed = 5;
    [SerializeField] float rotationSpeed = 0.1f;

    [SerializeField] float jumpPower = 2f;
    [SerializeField] float runCycleLegOffset = 0.2f;
    [SerializeField] float moveSpeedMultiplier = 1f;
    [SerializeField] float animSpeedMultiplier = 1f;
    [SerializeField] float gravity = -10f;
    [SerializeField] private float groundDistance = 0.1f;

    float m_ForwardAmount;
    const float k_Half = 0.5f;

    [Header("Booleans")]
    [SerializeField] bool blockRotationPlayer;
    public bool isGrounded;
    [Header("Attack Slots")]
    public AttackDifination demoAttack, combatAttack;
    [Header("Character Stats Slot")]
    public CharacterStats stats;

    [Header("Target to be Attacked")]
    public GameObject attackTarget;
#endregion

    #region MONOBEHAVIOUR
    void Awake()
    {
        anim = this.GetComponentInChildren<Animator>();
        cam = Camera.main;
        controller = this.GetComponent<CharacterController>();

        stats = GetComponent<CharacterStats>();
       
    }
    private void Start()
    {
        stats.characterDefination.OnLevelUp.AddListener(GameManager.Instance.OnHeroLeveledUp);
        stats.characterDefination.OnHeroDamaged.AddListener(GameManager.Instance.OnHeroDamaged);
        stats.characterDefination.OnHeroGainedHealth.AddListener(GameManager.Instance.OnHeroGainedHealth);
        stats.characterDefination.OnHeroDeath.AddListener(GameManager.Instance.OnHeroDied);
        stats.characterDefination.OnHeroInitialized.AddListener(GameManager.Instance.OnHeroInit);

        stats.characterDefination.OnHeroInitialized.Invoke();
    }
    #endregion

    #region Movement Controller
    public void Move(Vector3 move, bool crouch, bool jump)
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
     
        m_ForwardAmount =  new Vector2(move.x, move.y).sqrMagnitude;

        // control and velocity handling is different when grounded and airborne:
        if (isGrounded)
        {
            HandleGroundedMovement(crouch, jump);
        }
        else
        {
            HandleAirborneMovement();
        }

        // send input and other state parameters to the animator
        UpdateAnimator(move);

        if (m_ForwardAmount > 0.1f)
        {
      
            PlayerMoveAndRotation(move);
            
        }
    }
    private void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        anim.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        //anim.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        //anim.SetBool("Crouch", IsCrouching);
        anim.SetBool("OnGround", isGrounded);
        // animator.SetBool("IsSword", IsSword);

        if (!isGrounded)
        {
            anim.SetFloat("Jump", controller.transform.position.y);
        }

        // calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // (This code is reliant on the specific run cycle offset in our animations,
        // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
        float runCycle = Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime + runCycleLegOffset, 1);
        float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
        if (isGrounded)
        {
            anim.SetFloat("JumpLeg", jumpLeg);
        }

        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (isGrounded && move.magnitude > 0)
        {
            anim.speed = animSpeedMultiplier;
        }
        else
        {
            /// don't use that while airborne
            anim.speed = 1;
        }
    }
    private void HandleAirborneMovement()
    {
        // apply extra gravity from multiplier:
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    private void HandleGroundedMovement(bool crouch, bool jump)
    {
        ///check whether conditions are right to allow a jump:
        if (jump && !crouch && anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            // jump
            velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }
    }
    void PlayerMoveAndRotation(Vector3 move)
    {
        var camera = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * move.y + right * move.x;

        if (blockRotationPlayer == false)
        {
            //Camera
           transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed * moveSpeedMultiplier);
            controller.Move(desiredMoveDirection * Time.deltaTime * (movementSpeed * moveSpeedMultiplier));
        }
        else
        {
            //Strafe
            controller.Move((transform.forward * move.y + transform.right * move.y) * Time.deltaTime * (movementSpeed * moveSpeedMultiplier));
        }

    }
    #endregion

    #region Attack Controll
    public void AttackTarget(GameObject target = default)
    {
        if(target == default)
        {
            int i = Random.Range(0, ProjectConstants.combatAnims.Length);
            anim.SetTrigger(ProjectConstants.combatAnims[i]);
            return;
        }
        var weapon = stats.GetCurrentWeapon();
        if(weapon != null)
        {
            StopAllCoroutines();
            //stop agent
            attackTarget = target;
            StartCoroutine(PursueAndAttackTarget());
        }
        else
        {
            StopAllCoroutines();
            attackTarget = target;
            StartCoroutine(PursueAndAttackTarget2());
        }
    }
    private IEnumerator PursueAndAttackTarget()
    {
        //stop agent
        var weapon = stats.GetCurrentWeapon();
        if(attackTarget != null){
            while (Vector3.Distance(transform.position, attackTarget.transform.position) > weapon.Range)
            {
                //move to target

                yield return null;
            }
        }
       

        //look at target
        transform.LookAt(attackTarget.transform);
        anim.SetTrigger(ProjectConstants.vars.SWORD_ATTACK);
    }
    private IEnumerator PursueAndAttackTarget2()
    {

        while (Vector3.Distance(transform.position, attackTarget.transform.position) > combatAttack.Range)
        {
            //move to target
            // controller.Move(attackTarget.transform.position * movementSpeed * Time.deltaTime);
           // Vector3 mm = attackTarget.transform.position.x * Vector3.right + attackTarget.transform.position.z * Vector3.forward;
           // controller.Move(mm);
            yield return null;
        }

        transform.LookAt(attackTarget.transform);
        int i = Random.Range(0, ProjectConstants.combatAnims.Length);
        anim.SetTrigger(ProjectConstants.combatAnims[i]);
    }
    public void OnHit()
    {
        if(attackTarget != null)
        {
            ((Weapon)combatAttack).ExecuteAttack(gameObject, attackTarget);
        }
    }
    #endregion

    #region Ui stat upadate

    public int GetCurrentHealth()
    {
        return stats.characterDefination.currentHealth;
    }
    public int GetMaxHealth()
    {
        return stats.characterDefination.maxHealth;
    }
    public int GetCurrentLevel()
    {
        return stats.characterDefination.charLevel;
    }
    #endregion

    #region Callbacks
    public void OnMobDeath(int pointVal)
    {
        stats.IncreaseXp(pointVal); 
    }

    public void OnWaveComplete(int pointVal)
    {
        stats.IncreaseXp(pointVal);
       
    }

    public void OnOutOfWaves()
    {
        Debug.LogWarning("No more waves you win");
    }
    #endregion

 
}
