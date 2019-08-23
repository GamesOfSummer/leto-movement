using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System.Collections;
using System;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerScriptStateMachine))]
[Serializable]
public partial class PlayerScript : MonoBehaviour
{
    GameSaveController _gameSaveController;

    private Player reWiredPlayer;

    [HideInInspector]
    public PlayerStats playerStats;
    [HideInInspector]
    public PlayerScriptStateMachine statemachine;

    [HideInInspector]
    public PlayerMovement playerMovement;

    private Vector2 startingPosition;

    [HideInInspector]
    public Animator animator;

    private Companion companionScript;
    private PlayerButtonPrompts _playerButtonPrompts;

    public GameObject Sprite;
    public GameObject trailRendererTop;
    public GameObject trailRendererBottom;

    public Animator StunLockButtonPromptAnimator;


    //************************
    // companions
    public List<GameObject> companionArray;
    public GameObject currentCompanionGameObject;
    [HideInInspector]
    public int currentCompanionArrayIndex = 0;
    private float switchCommpanionTimer = 0.0F;


    [HideInInspector]
    public float h;

    [HideInInspector]
    public float v;

    public GameObject ceilingLeftCheck;
    public GameObject ceilingCheck;
    public GameObject ceilingRightCheck;

    public GameObject ceilingExtraCheckForCeilingDashing;

    public GameObject groundCheckLeft;
    public GameObject groundCheckCenter;
    public GameObject groundCheckRight;

    public GameObject leftTopDiagonalCheck;
    public GameObject leftBottomDiagonalCheck;

    public GameObject rightTopDiagonalCheck;
    public GameObject rightBottomDiagonalCheck;

    public GameObject LeftTopCeilingDashingCheck;
    public GameObject RightTopCeilingDashingCheck;

    public GameObject leftTopCheck;
    public GameObject leftMiddleCheck;
    public GameObject leftBottomCheck;

    public GameObject rightTopCheck;
    public GameObject rightMiddleCheck;
    public GameObject rightBottomCheck;


    [HideInInspector]
    public bool ceilingNearby = false;

    [HideInInspector]
    public bool ceilingNearbyExtraCheckForCeilingDashing = false;

    [HideInInspector]
    public bool groundedCheckWideTrue = false;

    [HideInInspector]
    public bool groundedCheckOneSideOnlyForFallingAnimationFlagTrue = false;

    [HideInInspector]
    public bool groundedCheckAllowForEdgeJumpTrue = false;

    [HideInInspector]
    public bool groundedCenterOnly = false;

    [HideInInspector]
    public bool groundedCenterOnlyNoEnemyUnderneath = false;

    [HideInInspector]
    public bool justGroundedCheckForAudioClip = false;


    [HideInInspector]
    public bool standingOnEnemy = false;

    [HideInInspector]
    public bool standingOnFlyingDysaur = false;

    [HideInInspector]
    public bool LeftTopCeilingDashingNearby = false;

    [HideInInspector]
    public bool RightTopCeilingDashingNearby = false;


    [HideInInspector]
    public bool leftTopDiagonalNearby = false;
    [HideInInspector]
    public bool leftBottomDiagonalNearby = false;

    [HideInInspector]
    public bool rightTopDiagonalNearby = false;
    [HideInInspector]
    public bool rightBottomDiagonalNearby = false;

    [HideInInspector]
    public bool leftWallNearby = false;

    [HideInInspector]
    public bool leftWallTopNearby = false;

    [HideInInspector]
    public bool leftWallMiddleNearby = false;

    [HideInInspector]
    public bool leftWallBottomNearby = false;

    [HideInInspector]
    public bool rightWallNearby = false;

    [HideInInspector]
    public bool rightWallTopNearby = false;

    [HideInInspector]
    public bool rightWallMiddleNearby = false;

    [HideInInspector]
    public bool rightWallBottomNearby = false;

    [HideInInspector]
    public bool illegalCeilingNearby = false;

    [HideInInspector]
    public bool illegalLeftWallNearby = false;

    [HideInInspector]
    public bool illegalRightWallNearby = false;

    [HideInInspector]
    public bool holdingDownLeftInput = false;

    [HideInInspector]
    public bool holdingDownRightInput = false;

    [HideInInspector]
    public bool holdingDownUpInput = false;

    [HideInInspector]
    public bool holdingDownDownInput = false;

    [HideInInspector]
    public bool pressedDownJumpInput = false;

    [HideInInspector]
    public bool holdingDownJumpInput = false;

    [HideInInspector]
    public bool continuallyHoldingDownJumpInput = false;

    [HideInInspector]
    public bool hasReleasedJumpInputOnce = false;

    [HideInInspector]
    public bool hasReleasedJumpInputOnceForWallClimbing = false;

    [HideInInspector]
    public bool holdingDownDashInput = false;


    [HideInInspector]
    public bool hasPressedDownDashInput = false;

    [HideInInspector]
    public bool holdingDownFireInput = false;


    [HideInInspector]
    public bool hasPressedDownInteractInput = false;

    [HideInInspector]
    public bool holdingDownInteractInput = false;

    [HideInInspector]
    public bool IsWallClinging = false;

    enum JumpStateEnum
    {
        AbleToJump = 0,
        IsJumpingUpwardsButCanJumpAgain = 1,
        IsJumpingUpwardsButCanNOTJumpAgain = 2,
        CannotJump = 3
    };

    private float JumpButtonHeldDownBeforeFirstReleaseTimer = 0;
    private float jumpHeldDownTimer = 0;
    public int jumpsLeftCount = 1;

    [HideInInspector]
    public int JumpState = 0;

    private bool facingRight = true;
    private bool facingRightForCutsceneOnlyCheck = true;
    private bool crouchedDown = false;


    float joystickBufferThreshold = 0.8F;
    float joystickBufferThresholdForVertical = 0.8F;

    public GameObject NormalDamageCollider;


    private Enemy EnemyILastTouched;



    //************************
    public GameObject wallJumpingEffectObject;

    //************************
    // wallsliding down juice effect
    public GameObject wallSlidingDownJuiceObject;
    private float wallSlidingDownJuiceBufferTimer = 0.0F;
    public GameObject wallJumpingJuiceObject;

    public GameObject floorDashingJuiceObject;
    private float floorDashingJuiceBufferTimer = 0.0F;


    public GameObject ChargeJumpParticleEffect;

    //************************
    // wave dash juice
    public GameObject silloutte;
    //private float silloutteBufferTimer = 0.0F;



    public GameObject ButtonPromptImage;
    private Animator ButtonPromptAnimator;

    private bool IsAttempingToTalkToAnNPC = false;
    private bool buttonPressedAnimationFlag = false;




    //*****************************
    //*****************************
    // stun lock
    private float DamageEverySoMuchTimerDuringStunlock = 2.0F;
    private float DamageAmountDuringStunlock = 1.0F;
    private float stunLockedTimer = 0F;
    private int timesFlippedForStunLockState = 0;


    //*****************************

    [HideInInspector]
    public AudioSource audioSource;

    [Header("Audio")]
    public AudioClip LetoJumpSound;
    public AudioClip LetoJumpForcedSound;

    public List<AudioClip> LetoWallJumpingSounds;
    public List<AudioClip> LetoDashingSounds;

    public AudioClip LetoFallSound;
    public AudioClip LetoTakingDamageSound;
    public AudioClip LetoTakingNoDamageSound;

    //*****************************
    //*****************************
    // animation flags
    [HideInInspector]
    public bool JumpAnimationFlag = false;
    [HideInInspector]
    public bool FallingAnimationFlag = false;
    [HideInInspector]
    public bool WallClingingAnimationFlag = false;
    [HideInInspector]
    public bool WallJumpingAnimationFlag = false;
    [HideInInspector]
    public bool DashingFloorAnimationFlag = false;
    [HideInInspector]
    public bool FiringMainWeaponAnimationFlag = false;
    [HideInInspector]
    public bool HurtAnimationFlag = false;
    [HideInInspector]
    public bool DeadAnimationFlag = false;

    [HideInInspector]
    public bool CrouchedDownAnimationFlag = false;


    [HideInInspector]
    public string DialogueNodeStartName = "";


    [HideInInspector]
    public bool _canSeeEnemyNames = false;


    [HideInInspector]
    public bool _canChangeCompanions = false;

    [HideInInspector]
    public bool _godModeEnabled = false;

    [HideInInspector]
    public string debugCurrentControllerString = "";


    private void Awake()
    {
        playerStats = gameObject.GetComponent<PlayerStats>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        statemachine = gameObject.GetComponent<PlayerScriptStateMachine>();

        reWiredPlayer = ReInput.players.GetPlayer(0);

        ButtonPromptAnimator = ButtonPromptImage.GetComponent<Animator>();
        ButtonPromptImage.GetComponent<SpriteRenderer>().enabled = false;
        _playerButtonPrompts = GetComponent<PlayerButtonPrompts>();


        StunLockButtonPromptAnimator.enabled = false;

        _gameSaveController = GameController.instance._gameSaveController;

        currentCompanionGameObject = new GameObject();
        currentCompanionGameObject.AddComponent<SpringJoint2D>();
    }

    private void Start()
    {
        StartCoroutine(InitializeCompanionArrayCoroutine());

        statemachine.InitilizeEngine();
        startingPosition = transform.position;
        playerStats.resetCurrentHPtoMax();

        trailRendererTop.SetActive(false);
        trailRendererBottom.SetActive(false);
        ChargeJumpParticleEffect.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 0;

        _canSeeEnemyNames = GameController.instance._gameSaveController.CanSeeEnemyName();
        _godModeEnabled = GameController.instance._gameSaveController.IsGodModeTurnedOn();
        RefreshAbilitiesAsProgressUnlockedSomething();
    }




    private void Update()
    {
        CheckForCollisionsAllDirections();


        if (statemachine.IsCurrentStateJustStartedLevel() && groundedCheckWideTrue == true)
        {
            statemachine.ChangeState_GravityOn();
        }


        if (statemachine.DoesCurrentStateAllowUserInput())
        {
            if (!statemachine.IsCurrentStateInCutscene())
            {
                HandleHorizontalInputFromUser();
            }
            else
            {
                holdingDownLeftInput = false;
                holdingDownRightInput = false;
            }


            HandleVerticalInputFromUser();



            //***********************************
            // --- jump button
            ////***********************************
            if (reWiredPlayer.GetButtonDown("Jump"))
            {
                pressedDownJumpInput = true;
            }
            else
            {
                pressedDownJumpInput = false;
            }


            if (reWiredPlayer.GetButton("Jump"))
            {
                holdingDownJumpInput = true;
            }
            else
            {
                holdingDownJumpInput = false;
            }



            if (reWiredPlayer.GetButtonUp("Jump"))
            {
                hasReleasedJumpInputOnce = true;
            }


            if (reWiredPlayer.GetButtonDown("Jump"))
            {
                hasReleasedJumpInputOnceForWallClimbing = true;
            }

            //***********************************
            // --- dash button
            ////***********************************
            if (reWiredPlayer.GetButton("Dash"))
            {
                holdingDownDashInput = true;
            }
            else
            {
                holdingDownDashInput = false;
            }

            if (reWiredPlayer.GetButtonDown("Dash"))
            {
                hasPressedDownDashInput = true;
            }
            else
            {
                hasPressedDownDashInput = false;
            }






            //***********************************
            // --- fire button
            ////***********************************
            if (reWiredPlayer.GetButton("Fire"))
            {
                holdingDownFireInput = true;
            }
            else
            {
                holdingDownFireInput = false;
            }



            //***********************************
            // --- interact button
            ////***********************************
            if (reWiredPlayer.GetButton("Interact"))
            {
                holdingDownInteractInput = true;
            }
            else
            {
                holdingDownInteractInput = false;
            }

            if (reWiredPlayer.GetButtonDown("Interact"))
            {
                hasPressedDownInteractInput = true;
            }
            else
            {
                hasPressedDownInteractInput = false;
            }



            //***********************************
            // --- companion switching
            ////***********************************
            if (_canChangeCompanions)
            {
                if (switchCommpanionTimer <= 0 && (reWiredPlayer.GetButton("LeftTrigger") || reWiredPlayer.GetButton("RightTrigger")))
                {
                    switchCommpanionTimer = .3F;
                    switchToNextCompanion();

                }
            }

            switchCommpanionTimer = switchCommpanionTimer - Time.deltaTime;

            //***********************************
            // --- start button
            ////***********************************

            //if (reWiredPlayer.GetButtonDown("Start"))
            //{
            //	GameController.instance.ToggleMiniMap();
            //}


            //***********************************
            // --- start npc dialog if needed....
            ////***********************************

            if (statemachine.IsCurrentStateNearNPC() && (DialogueNodeStartName != null && DialogueNodeStartName.Length > 0))
            {
                EnableButtonPrompt(PlayerButtonPrompts.PlayerActions.Interact);

                if (hasPressedDownInteractInput == true && GameController.instance.DoesDialogBufferAllowsInput())
                {
                    statemachine.ChangeStateInCutscene();
                    GameController.instance.DisableGUIStartDialogue(DialogueNodeStartName);
                }

            }

        }
        else if (statemachine.IsCurrentStateStunLockedAndTakingDamage() == true)
        {
            Update_HandleStunLock();
        }
        else
        {
            holdingDownRightInput = false;
            holdingDownLeftInput = false;
            holdingDownUpInput = false;
            holdingDownDownInput = false;

            holdingDownJumpInput = false;
            holdingDownDashInput = false;

            holdingDownFireInput = false;
            holdingDownInteractInput = false;

            h = 0;
            v = 0;
        }


    }


    private void Update_HandleStunLock()
    {
        HandleHorizontalInputFromUser();

        if (stunLockedTimer <= 0)
        {
            stunLockedTimer = DamageEverySoMuchTimerDuringStunlock;
            //Debug.Log("Damage Leto");

            DamageLeto(DamageAmountDuringStunlock);
        }

        stunLockedTimer = stunLockedTimer - Time.deltaTime;
    }


    public void StunLockAndDamageLeto(float timer, float damageAmountPerTimerRese)
    {
        if (!statemachine.IsCurrentStateDead())
        {
            stunLockedTimer = 0;
            DamageAmountDuringStunlock = damageAmountPerTimerRese;
            statemachine.ChangeStateToStunLockedAndTakingDamage(timer);
        }
    }


    // NOTE :: DO ALL PHYSICS UPDATES IN FIXED UPDATE
    // USER INPUT GOES IN UPDATE()
    private void FixedUpdate()
    {
        //Debug.Log("times of Flipped :: " + timesFlipped);

        ControllerChanged();
        if (statemachine.IsCurrentStateStunLockedAndTakingDamage())
        {
            if (timesFlippedForStunLockState > 3)
            {
                Debug.Log("Break out now...");
                statemachine.ChangeToImmuneToDamage();
            }
        }
        else if (statemachine.IsCurrentStateInCutscene())
        {
            MakeLetoFall();
        }
        else if (!statemachine.IsCurrentStateInCutscene())
        {
            timesFlippedForStunLockState = 0;

            FixedUpdate_Running();
            FixedUpdate_JumpingOrWallClimbingOrDashing();



            if (playerMovement.IsChargeJumpEnabled())
            {
                FixedUpdate_ChargeJumpAndChainJump();
            }

            FixedUpdate_FiringGun();
        }


        HandleAnimations();
    }

    // ANIMATIONS
    private void HandleAnimations()
    {
        animator.SetBool("Jumping", false);
        animator.SetBool("Falling", false);
        animator.SetBool("Running", false);
        animator.SetBool("Wallclimbing", false);
        animator.SetBool("Walljumping", false);
        animator.SetBool("IdleFlipped", false);
        animator.SetBool("Dashing_Floor", false);
        animator.SetBool("Dashing_Wall", false);
        animator.SetBool("Dashing_Ceiling", false);
        animator.SetBool("Grounded", false);
        animator.SetBool("Hurt", false);
        animator.SetBool("Dead", false);
        animator.SetBool("CrouchedDown", false);



        if (playerStats.isDead() || HurtAnimationFlag == true)
        {
            animator.SetBool("MovingHorizontally", false);
            animator.SetBool("FiringMainWeapon", false);
        }

        if (playerStats.isDead() == true)
        {
            animator.SetBool("Dead", true);
        }
        else if (HurtAnimationFlag == true)
        {
            animator.SetBool("Hurt", true);
        }
        else if (!statemachine.IsCurrentStateInCutscene())
        {

            if (FiringMainWeaponAnimationFlag)
            {
                animator.SetBool("FiringMainWeapon", true);
            }
            else
            {
                animator.SetBool("FiringMainWeapon", false);
            }


            ButtonPromptAnimator.SetBool("ButtonPressed", false);

            if (buttonPressedAnimationFlag == true)
            {
                ButtonPromptAnimator.SetBool("ButtonPressed", true);
            }


            if (crouchedDown == true)
            {
                animator.SetBool("CrouchedDown", true);
            }
            else
            {
                if (FiringMainWeaponAnimationFlag)
                {
                    animator.SetBool("FiringMainWeapon", true);
                }


                if (!WallClingingAnimationFlag && !DashingFloorAnimationFlag)
                {
                    HandleDirectionAnimationFlags();
                }



                if (DashingFloorAnimationFlag)
                {
                    if (ceilingNearby || ceilingNearbyExtraCheckForCeilingDashing)
                    {
                        animator.SetBool("Dashing_Ceiling", true);
                    }
                    else if ((leftWallNearby || rightWallNearby) && !groundedCheckWideTrue)
                    {
                        animator.SetBool("Dashing_Wall", true);
                    }
                    else
                    {
                        animator.SetBool("Dashing_Floor", true);
                    }


                }
                else if (WallClingingAnimationFlag == true)
                {
                    animator.SetBool("Wallclimbing", true);
                }
                else if (groundedCheckOneSideOnlyForFallingAnimationFlagTrue == false && FallingAnimationFlag == true)
                {
                    animator.SetBool("Falling", true);
                }
                else if (WallJumpingAnimationFlag == true)
                {
                    animator.SetBool("Walljumping", true);
                }
                else if (JumpAnimationFlag == true)
                {
                    animator.SetBool("Jumping", true);
                }
                else if (WallClingingAnimationFlag == false && groundedCheckWideTrue == true)
                {
                    animator.SetBool("Grounded", true);

                    if (Math.Abs(h) > joystickBufferThreshold && !holdingDownUpInput)
                    {
                        animator.SetBool("Running", true);
                    }
                    else
                    {
                        if (!facingRight || !facingRightForCutsceneOnlyCheck)
                        {
                            animator.SetBool("IdleFlipped", true);
                        }
                    }

                }


            }
        }
        else
        {
            animator.SetBool("MovingHorizontally", false);
        }


    }

    //this is for trigger enter or trigger on stay
    public void TriggerEnter(Collider2D other)
    {
        //Debug.Log("*** Leto :: TriggerEnter :: " + other.name + " -- " + other.tag + " ***");

        HandleIfNPC(other);
        HandeIfButtonPromptZone(other);
        string enemyName = HandleIfEnemy(other);

        if (_godModeEnabled == false && statemachine.AbleToTakeDamage() && (IsBulletOrEnemy(other)))
        {
            //Debug.Log("*** Leto :: TriggerEnter - Enemy Touch ***");

            int damage = 0;
            float stunTimeOnLeto = 0;
            float damageThrowBackOnLeto = 0;

            bool goIntoStunLockStateDoNotTakeDamage = false;

            if (other.GetComponent<Enemy>() != null)
            {
                damage = other.GetComponent<Enemy>().returnAttackDamage();
                stunTimeOnLeto = other.GetComponent<Enemy>().stunTimeOnLeto;
                damageThrowBackOnLeto = other.GetComponent<Enemy>().damageThrowbackOnLeto;
            }
            else if (other.GetComponent<EnemyHitbox>() != null)
            {
                damage = other.GetComponent<EnemyHitbox>().ReturnEnemyScript().returnAttackDamage();
                stunTimeOnLeto = other.GetComponent<EnemyHitbox>().ReturnEnemyScript().stunTimeOnLeto;
                damageThrowBackOnLeto = other.GetComponent<EnemyHitbox>().ReturnEnemyScript().damageThrowbackOnLeto;
                goIntoStunLockStateDoNotTakeDamage = other.GetComponent<EnemyHitbox>().DoNotDamageLetoInsteadStunLock;
            }
            else if (other.GetComponent<BulletScript>() != null)
            {
                damage = other.GetComponent<BulletScript>().ReturnBulletDamage();
                stunTimeOnLeto = other.GetComponent<BulletScript>().returnStunTimeOnLeto();
                damageThrowBackOnLeto = other.GetComponent<BulletScript>().returnDamageThrowBackOnLeto();
            }
            else
            {
                Debug.LogWarning("Error in Leto taking damage class");
            }


            // I may not need to do damage, ie, a monster to ride on (Dysaur)
            RaycastHit2D hit2 = Physics2D.Linecast(transform.position, other.gameObject.transform.position);
            Vector2 normal2 = hit2.normal;


            if (statemachine.IsCurrentStateStunLockedAndTakingDamage() == false &&
                goIntoStunLockStateDoNotTakeDamage == true &&
                !statemachine.IsCurrentStateDead())
            {
                if (other.GetComponent<EnemyHitbox>() != null)
                {
                    //Debug.Log("working");

                    var hitbox = other.GetComponent<EnemyHitbox>();

                    StunLockAndDamageLeto(hitbox.stunTimerTimerReset, hitbox.stunDamageToLeto);
                }
            }
            else if (statemachine.IsCurrentStateStunLockedAndTakingDamage() == true && !statemachine.IsCurrentStateDead())
            {
                //something else....
            }
            else if (enemyName.Contains("Dysaur") &&
                (Vector2.Dot(Vector2.up, normal2) > 0.30) &&
                (Vector2.Dot(Vector2.left, normal2) < 0.85F && Vector2.Dot(Vector2.right, normal2) < 0.85F))
            {

                //Debug.Log("No damage");
                //Debug.Log(Vector2.Dot(Vector2.up, normal2) + " Left " +  + " Right " + Vector2.Dot(Vector2.right, normal2));

            }
            else
            {
                //Debug.Log("Doing damage! :: " + enemyName + " - " + Vector2.Dot(Vector2.up, normal2));
                DamageLeto(damage);

                if (playerStats.isDead())
                {
                    KillLeto();
                }
                else
                {
                    RaycastHit2D hit = Physics2D.Linecast(transform.position, other.gameObject.transform.position);
                    Vector2 normal = hit.normal;

                    ThrowLetoFromDamageAndChangeStateToHurt(normal, stunTimeOnLeto, damageThrowBackOnLeto);
                }
            }
        }


    }

    public void TriggerExit(Collider2D other)
    {
        if (statemachine.IsCurrentStateNearNPC())
        {
            statemachine.ChangeState_GravityOn();
        }

        DisableButtonPrompt();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "ground")
        {
            companionScript.Companion_OnCollisionEnter2D(collision);
            statemachine.ChangeState_GravityOn();

        }
        else
        {
            if (IsEnemy(collision.collider) || collision.collider.GetComponent<EnemyHitbox>() != null)
            {
                if (collision.collider.GetComponent<EnemyHitbox>() == null)
                {
                    EnemyILastTouched = collision.collider.GetComponent<Enemy>();
                }
                else
                {
                    EnemyILastTouched = collision.collider.GetComponent<EnemyHitbox>().ReturnEnemyScript();
                }
            }
        }
    }

    //*****************************
    //*****************************
    public void DamageLeto(float damage)
    {
        DamageLeto((int)damage);
    }

    public void DamageLeto(int damage)
    {
        Useful.instance.GetShakeCamera().ActuallyShakeCameraOnLetoDamage();

        playerStats.takeDamage(damage);
        audioSource.clip = LetoTakingDamageSound;
        audioSource.Play();


        if (playerStats.isDead() && !statemachine.IsCurrentStateDead())
        {
            KillLeto();
        }

    }

    private void ThrowLetoFromDamageAndChangeStateToHurt(Vector2 normal, float stunTimeOnLeto, float damageThrowBackOnLeto)
    {
        if (Vector2.Dot(Vector2.up, normal) > 0.70)
        {
            statemachine.ChangeStateToHurt(stunTimeOnLeto, new Vector2(0, damageThrowBackOnLeto));
        }
        else if (Vector2.Dot(Vector2.down, normal) > 0.70)
        {
            statemachine.ChangeStateToHurt(stunTimeOnLeto, new Vector2(0, -damageThrowBackOnLeto));
        }
        else if (Vector2.Dot(Vector2.left, normal) > 0.70)
        {
            statemachine.ChangeStateToHurt(stunTimeOnLeto, new Vector2(-damageThrowBackOnLeto, 0));
        }
        else if (Vector2.Dot(Vector2.right, normal) > 0.70)
        {
            statemachine.ChangeStateToHurt(stunTimeOnLeto, new Vector2(damageThrowBackOnLeto, 0));
        }
        else
        {
            statemachine.ChangeStateToHurt(stunTimeOnLeto, new Vector2(0, 0));
        }
    }





    public float CalculateHorizontalDistance(float oldSpeed)
    {
        var newVector = transform.position + new Vector3(oldSpeed * Time.deltaTime, 0, 0);
        var hit = ReturnRayCast(newVector);

        float distance = (hit.point - (Vector2)transform.position).magnitude;

        if ((hit.fraction != 0) && (distance < Mathf.Abs(oldSpeed)))
        {
            return (hit.fraction * oldSpeed * Time.deltaTime);
        }
        else if (distance == 0)
        {
            return 0;
        }

        return (oldSpeed * Time.deltaTime);
    }



    public Vector2 FinalAdjustHorizontalSpeed(float directionLetoIsGoing)
    {
        var newVector = Vector2.zero;
        float distanceToRayCast = 0.2F;

        if (directionLetoIsGoing > 1)
        {
            newVector = transform.position + new Vector3((distanceToRayCast), 0, 0);
        }
        else
        {
            newVector = transform.position + new Vector3((-1 * distanceToRayCast), 0, 0);
        }



        RaycastHit2D hit = ReturnRayCast(newVector);
        float distance = (hit.point - (Vector2)transform.position).magnitude;


        if (hit.collider != null)
        {
            //Debug.Log("hit point::" + hit.point + " distance:: " + distance + " fraction:" + hit.fraction);

            if (directionLetoIsGoing > 1)
            {
                return new Vector3(hit.point.x - 0.09F, transform.position.y, 0);
            }
            else
            {
                return new Vector3(hit.point.x + 0.15F, transform.position.y, 0);
            }

        }
        else
        {
            return transform.position;
        }
    }


    public float CalculateVerticalDistance(float oldSpeed)
    {
        var newVector = transform.position + new Vector3(0, oldSpeed * Time.deltaTime, 0);
        var hit = ReturnRayCast(newVector);
        float distance = (hit.point - (Vector2)transform.position).magnitude;

        if ((hit.fraction != 0) && (distance < Mathf.Abs(oldSpeed)))
        {
            return (hit.fraction * oldSpeed * Time.deltaTime);
        }
        else if (distance == 0)
        {
            return 0;
        }

        return (oldSpeed * Time.deltaTime);
    }


    public Vector2 FinalAdjustVerticalSpeed(float oldSpeed)
    {
        var newVector = Vector2.zero;
        float distanceToRayCast = 0.4F;

        if (oldSpeed > 1)
        {
            newVector = transform.position + new Vector3(0, (distanceToRayCast), 0);
        }
        else
        {
            newVector = transform.position + new Vector3(0, (-1 * distanceToRayCast), 0);
        }


        RaycastHit2D hit = ReturnRayCast(newVector);
        float distance = (hit.point - (Vector2)transform.position).magnitude;


        if (hit.collider != null)
        {
            //Debug.Log("hit point::" + hit.point + " distance:: " + distance + " fraction:" + hit.fraction);

            if (oldSpeed > 1)
            {
                return new Vector3(transform.position.x, hit.point.y - 0.25F, 0);
            }
            else
            {
                return new Vector3(transform.position.x, hit.point.y + 0.30F, 0);
            }

        }
        else
        {
            return transform.position;
        }
    }





    private RaycastHit2D ReturnRayCast(Vector3 newVector)
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, newVector, 1 << LayerMask.NameToLayer("Ground"));

        RaycastHit2D monsterHit = Physics2D.Linecast((Vector2)transform.position, newVector, 1 << LayerMask.NameToLayer("Enemy"));
        if (monsterHit.fraction != 0)
        {
            return monsterHit;
        }

        return hit;
    }


    public void KillLeto()
    {
        statemachine.ChangeStateToDead(2.0F);
        GameController.instance.GameControllerShoutOutPlayerDeathEvent();
    }

    private bool IsBulletOrEnemy(Collider2D other)
    {
        return (IsBullet(other) || IsEnemy(other));
    }

    private bool IsBullet(Collider2D other)
    {
        if (other.GetComponent<BulletScript>() != null)
        {
            if (other.GetComponent<BulletScript>().damagePlayerFlag == true)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsEnemy(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            if (other.GetComponentInParent<Enemy>() != null)
            {
                if (other.GetComponentInParent<Enemy>().canDoDamageViaTouch)
                {
                    return true;
                }
            }
            else if (other.GetComponent<Enemy>() != null)
            {
                if (other.GetComponent<Enemy>().canDoDamageViaTouch)
                {
                    return true;
                }
            }
            else if (other.GetComponent<EnemyHitbox>() != null && other.GetComponent<EnemyHitbox>().EnemyScriptLocation == EnemyHitbox.EnemyScriptLocationEnum.ReferenceInEditor)
            {
                //Debug.Log(other.GetComponent<EnemyHitbox>().ReturnEnemyScript().canDoDamageViaTouch);

                if (other.GetComponent<EnemyHitbox>().ReturnEnemyScript().canDoDamageViaTouch)
                {
                    return true;
                }
            }
        }


        return false;
    }

    private void HandleIfNPC(Collider2D other)
    {
        if (other.GetComponent<NPCDialogStart>() != null && other.GetComponent<NPCDialogStart>().NPCDisabled == false)
        {
            if (statemachine.IsCurrentStateGravityOn())
            {
                statemachine.ChangeStateNearNPC();
            }

        }
    }

    private void HandeIfButtonPromptZone(Collider2D other)
    {
        if (other.GetComponent<ButtonPromptTrigger>() != null)
        {
            EnableButtonPrompt(other.GetComponent<ButtonPromptTrigger>().playerAction);
        }
    }

    private string HandleIfEnemy(Collider2D other)
    {
        string enemyName = "";
        if (IsEnemy(other) || other.GetComponent<EnemyHitbox>() != null)
        {
            if (other.GetComponent<EnemyHitbox>() == null)
            {
                EnemyILastTouched = other.GetComponent<Enemy>();
            }
            else
            {
                EnemyILastTouched = other.GetComponent<EnemyHitbox>().ReturnEnemyScript();
            }

            if (EnemyILastTouched != null)
            {
                enemyName = EnemyILastTouched.ReturnFriendlyEnemyName();
                GameController.instance.UpdateEnemyNameGUI(enemyName);
            }
        }

        return enemyName;
    }

    private void FixedUpdate_Running()
    {
        if(!holdingDownDownInput && !holdingDownUpInput)
        {
            if (holdingDownRightInput && !rightWallNearby && !crouchedDown)
            {
                playerMovement.MoveRight();

            }
            else if (holdingDownLeftInput && !leftWallNearby && !crouchedDown)
            {
                playerMovement.MoveLeft();
            }

        }

    }

    //called by bounce dysaurs and Letos bed
    public void ForceJumpState(float time)
    {

        if(!statemachine.IsCurrentStateForcedJumpingUpward())
        {
            GroundLeto();

            if (holdingDownJumpInput == true)
            {
                Useful.instance.Play2DAudioClipOnNewGameObject(LetoJumpForcedSound, transform.position);
                statemachine.ChangeState_ForcedJump(time, true);
            }
            else
            {
                Useful.instance.Play2DAudioClipOnNewGameObject(LetoJumpSound, transform.position);
                statemachine.ChangeState_ForcedJump(time);
            }
        }

    }


    private bool CanSingleJump()
    {
        if (playerMovement.IsDoubleJumpEnabled())
        {
            return IsAttempingToStartJump();
        }
        else
        {
            return
           (
           (OnJumpableTerrain() || JumpState == (int)JumpStateEnum.AbleToJump) &&
           jumpsLeftCount > 0 &&
           !ceilingNearby &&
           OnJumpableTerrain() &&
           !statemachine.IsCurrentStateForcedJumpingUpward() &&
           IsAttempingToStartJump()
           );
        }

    }


    private void SetJumpStateAfterFirstJump()
    {
        if (playerMovement.IsDoubleJumpEnabled() && JumpState == 0)
        {
            JumpState = (int)JumpStateEnum.IsJumpingUpwardsButCanJumpAgain;
        }
        else
        {
            JumpState = (int)JumpStateEnum.IsJumpingUpwardsButCanNOTJumpAgain;
        }
    }





    private void FixedUpdate_JumpingOrWallClimbingOrDashing()
    {
        CanIGroundLetoIfOnGround();
        HandleWallClinging();

        DashingFloorAnimationFlag = false;

        if (IsAttemptingToDash())
        {
            //for hitting the ceiling while trying to ceiling dash
            if (((IsCurrentlyJumpingUpward()) || statemachine.IsCurrentStateForcedJumpingUpward()) && !ceilingNearby)
            {
                JumpButtonHeldDownBeforeFirstRelease();
            }


            FixedUpdate_Dashing();
        }
        else if (CanSingleJump())
        {

            if (playerMovement.IsDoubleJumpEnabled())
            {
                ForceGroundLeto();
            }


            JumpButtonHeldDownBeforeFirstReleaseTimer = 0;

            jumpsLeftCount--;
            if (jumpsLeftCount < 0)
            {
                jumpsLeftCount = 0;
            }

            SetJumpStateAfterFirstJump();

            if (IsWallClimbing())
            {          
                GameObject particle = Instantiate(wallJumpingEffectObject) as GameObject;

                if (!facingRight)
                {
                    particle.transform.position = gameObject.transform.position + new Vector3(-.2F, -0.25F, 0);
                }
                else
                {
                    particle.transform.position = gameObject.transform.position + new Vector3(0.2F, -0.25F, 0);                
                }

                Destroy(particle, 0.15F);


                PlaySoundFromAudioArray(LetoWallJumpingSounds);
                statemachine.ChangeState_ForcedJump();
                WallJumpingAnimationFlag = true;
            }
            else if (!statemachine.IsCurrentStateForcedJumpingUpward())
            {
                Useful.instance.Play2DAudioClipOnNewGameObject(LetoJumpSound, transform.position);
            }

            JumpButtonPressed();

        }
        else if ((IsCurrentlyJumpingUpward()) || statemachine.IsCurrentStateForcedJumpingUpward())
        {
            if (ceilingNearby)
            {
                CancelJump();
            }
            else
            {
                JumpButtonHeldDownBeforeFirstRelease();
            }
        }
        else if (!statemachine.IsCurrentStateForcedJumpingUpward() &&
            ((rightWallNearby == true) && (holdingDownRightInput == true) && !illegalRightWallNearby) ||
            (leftWallNearby == true) && (holdingDownLeftInput == true) && !illegalLeftWallNearby)
        {
            playerMovement.WallStick();
            CanIGroundLetoWhileOnWall();
            WallJumpingAnimationFlag = false;
        }
        else if (groundedCheckWideTrue == false)
        {
            MakeLetoFall();
        }



        if (!statemachine.IsCurrentStateDashing())
        {
            ShutOffBothDashingTrails();
        }
    }


    private void MakeLetoFall()
    {
        DashingFloorAnimationFlag = false;
        FallingAnimationFlag = true;
        WallJumpingAnimationFlag = false;

        playerMovement.ApplyGravity();
    }

    private bool IsAttemptingToDash()
    {
        return (playerMovement.IsDashEnabled() &&
            holdingDownDashInput &&
            (WallNearby() || ceilingNearbyExtraCheckForCeilingDashing) &&
            (IsAttempingToTalkToAnNPC == false));
    }

    public bool IsAttempingToStartJump()
    {
        if (playerMovement.IsDoubleJumpEnabled())
        {
            return (pressedDownJumpInput);
        }
        else
        {
            return (pressedDownJumpInput);
        }

    }

    private void JumpButtonPressed()
    {
        JumpAnimationFlag = true;
        playerMovement.Jump();
    }

    private bool OnJumpableTerrain()
    {
        return (
            groundedCenterOnly ||
            groundedCheckWideTrue ||
            groundedCheckAllowForEdgeJumpTrue ||
            ((leftWallNearby && !illegalLeftWallNearby) || (rightWallNearby && !illegalRightWallNearby))
            );
    }

    private bool IsWallClimbing()
    {
        return (((leftWallNearby == true) && (holdingDownLeftInput == true)) ||
                ((rightWallNearby == true) && (holdingDownRightInput == true)));
    }
    private void CancelJump()
    {
        JumpButtonHeldDownBeforeFirstReleaseTimer = 1000;
        jumpHeldDownTimer = 1000;
        JumpState = 3;
    }
    private void JumpButtonHeldDownBeforeFirstRelease(bool overrideFasterJump = false)
    {
        JumpButtonHeldDownBeforeFirstReleaseTimer += Time.deltaTime;

        if (AllowedToJumpTimerReturnsTrue() || statemachine.IsCurrentStateForcedJumpingUpward())
        {
            JumpAnimationFlag = true;
            playerMovement.Jump();
        }
        else
        {
            JumpState = 3;
        }
    }

    private bool AllowedToJumpTimerReturnsTrue()
    {
        return (JumpButtonHeldDownBeforeFirstReleaseTimer <= playerMovement.GetJumpForceTimeToReset());
    }

    public bool IsCurrentlyJumpingUpward()
    {
        if (playerMovement.IsDoubleJumpEnabled())
        {
            return ((pressedDownJumpInput || holdingDownJumpInput));
        }
        else
        {
            return (AllowedToJumpTimerReturnsTrue() &&
             (pressedDownJumpInput || holdingDownJumpInput) &&
             (JumpState == 2 || (JumpState == 1 && playerMovement.IsDoubleJumpEnabled())) &&
            (hasReleasedJumpInputOnce == false));
        }



    }
    private void CanIGroundLetoIfOnGround()
    {
        if (groundedCheckWideTrue && !IsCurrentlyJumpingUpward())
        {
            GroundLeto();
        }
        else
        {
            justGroundedCheckForAudioClip = false;
        }

    }

    private void CanIGroundLetoWhileOnWall()
    {
        if (hasReleasedJumpInputOnce)
        {
            GroundLeto();
        }

    }
    private void FixedUpdate_Dashing()
    {
        PlaySoundFromAudioArrayWithDelay(LetoDashingSounds, 0.2F);
        SpawnDashingEffects();


        DashingFloorAnimationFlag = true;

        var letoDirection = 1;
        if (facingRight == false)
        {
            letoDirection = -1;
        }

        VerticalFlipUp();

        //Debug.Log(ceilingNearby + " - " + grounded + " --- " + (leftWallTopNearby || leftWallBottomNearby || rightWallTopNearby || rightWallBottomNearby) + " - " + WallNearby());


        if (!statemachine.IsCurrentStateDashing())
        {
            statemachine.ChangeState_Dashing();
        }

        

        //ZOOM UP WALLS
        if (
        (!ceilingNearby || !groundedCenterOnly) &&
        ((leftWallNearby && !illegalLeftWallNearby) || (rightWallNearby && !illegalRightWallNearby))
        )
        {
         
            ShutOffBothDashingTrails();

            if (holdingDownDownInput && !groundedCenterOnly)
            {
                VerticalFlipDown();
                trailRendererTop.SetActive(true);
                playerMovement.DashingOnWall(-1);
            }
            else if (!ceilingNearby)
            {
                trailRendererBottom.SetActive(true);
                playerMovement.DashingOnWall(1);
            }

        }
        //CEILING
        else if (ceilingNearby || ceilingNearbyExtraCheckForCeilingDashing)
        {

            trailRendererBottom.SetActive(false);
            trailRendererTop.SetActive(true);
            SpawnDashingEffects();

            if (!leftWallNearby && !rightWallNearby)
            {
                playerMovement.DashingOnCeiling(letoDirection);
            }
            else if (leftWallNearby || LeftTopCeilingDashingNearby)
            {
                playerMovement.DashingOnCeiling(1);
            }
            else if (rightWallNearby || RightTopCeilingDashingNearby)
            {
                playerMovement.DashingOnCeiling(-1);
            }


        }
        //ON GROUND ONLY
        else if (groundedCenterOnlyNoEnemyUnderneath && (!leftWallNearby && !illegalLeftWallNearby && !rightWallNearby && !illegalRightWallNearby))
        {          
            trailRendererTop.SetActive(false);
            trailRendererBottom.SetActive(true);
            SpawnDashingEffects();

            playerMovement.DashingOnGround(letoDirection);
        }

    }

    private void ShutOffBothDashingTrails()
    {
        trailRendererTop.SetActive(false);
        trailRendererBottom.SetActive(false);
    }

    private void FixedUpdate_FiringGun()
    {
        if (companionScript != null)
        {
            if (holdingDownFireInput && companionScript.fireBufferTimer <= 0)
            {
                companionScript.fireBufferTimer = companionScript.fireBufferReset;
                companionScript.FireButtonPressed(crouchedDown);
                FiringMainWeaponAnimationFlag = true;
            }

            companionScript.fireBufferTimer -= Time.deltaTime;

            if (holdingDownFireInput)
            {
                FiringMainWeaponAnimationFlag = true;
            }
            else
            {
                FiringMainWeaponAnimationFlag = false;
            }
        }

    }

    private void HandleWallClinging()
    {
        if (AbleToWallClingAndAttemptingTo())
        {
            WallClingingAnimationFlag = true;
            WallJumpingAnimationFlag = false;
            IsWallClinging = true;
            
            if(statemachine.IsCurrentStateForcedJumpingUpward())
            {
                statemachine.ChangeState_GravityOn();
            }
            
            GroundLeto();
        }
        else
        {
            WallClingingAnimationFlag = false;

            IsWallClinging = false;
        }
    }

    private float _jumpChargeTimer = 0.0F;

    private bool _hasChargeJumpedOnced = false;

    private void FixedUpdate_ChargeJumpAndChainJump()
    {
        if ((playerMovement.IsChainJumpEnabled() == true) || (_hasChargeJumpedOnced == false))
        {
            //JumpAnimationFlag = false;
            if (!statemachine.IsCurrentStateForcedJumpingUpward() && !groundedCheckWideTrue && hasReleasedJumpInputOnce == true && (pressedDownJumpInput || holdingDownJumpInput))
            {

                // playerMovement.ChangeVerticalVelocity(0.0F);
                //playerMovement.ResetGravityToZero();


                statemachine.ChangeState_HoldingAChargeJump();
                ChargeJumpParticleEffect.SetActive(true);
                trailRendererBottom.SetActive(true);

                _jumpChargeTimer += Time.deltaTime;

            }
            else if (_jumpChargeTimer > 0.1F)
            {
                _hasChargeJumpedOnced = true;

                //Debug.Log("released -> " + _jumpChargeTimer);
                _jumpChargeTimer = 0;

                if (holdingDownLeftInput)
                {
                    statemachine.SetChainJumpingLeft();
                }
                else if (holdingDownRightInput)
                {
                    statemachine.SetChainJumpingRight();
                }


                statemachine.ChangeState_IsChainJumping();
                ChargeJumpParticleEffect.SetActive(false);
            }
            else if (_jumpChargeTimer != 0)
            {
                //Debug.Log("released -> " + _jumpChargeTimer);
                _jumpChargeTimer = 0;

                // playerMovement.ResetGravityToOne();
                //playerMovement.ChangeVerticalVelocity(15.0F);
                ChargeJumpParticleEffect.SetActive(false);
                trailRendererBottom.SetActive(false);
            }


            if (groundedCheckWideTrue || statemachine.IsCurrentStateForcedJumpingUpward())
            {
                _jumpChargeTimer = 0;
                _hasChargeJumpedOnced = false;
                //playerMovement.ResetGravityToOne();
            }

        }




    }

    //called by dialog runner sometimes
    public void RefreshAbilitiesAsProgressUnlockedSomething()
    {
        playerMovement.SetIsDashEnabled(GameController.instance._gameSaveController.HasDashBoots());
        playerMovement.SetChargeJumpEnabled(GameController.instance._gameSaveController.HasChargeJump());
        playerMovement.SetChainJumpEnabled(GameController.instance._gameSaveController.HasChainJump());
        playerMovement.SetDoubleJump(GameController.instance._gameSaveController.HasDoubleJump());

        if (GameController.instance._timeManager.GetComponent<TimeManager>().GetCurrentDayInt() > 2)
        {
            _canChangeCompanions = true;
        }
    }

    private void HandleVerticalInputFromUser()
    {
        v = reWiredPlayer.GetAxis("Move Vertically");

        //*************************
        //*************************

        //Debug.Log("v : " + v);

        crouchedDown = false;
        SetCollidersWhenNotCrouched();

        if (v > joystickBufferThresholdForVertical)
        {
            holdingDownUpInput = true;
            holdingDownDownInput = false;

        }
        else if (v < (-1 * joystickBufferThresholdForVertical))
        {
            holdingDownUpInput = false;
            holdingDownDownInput = true;

            if (groundedCenterOnly)
            {
                crouchedDown = true;
                SetCollidersWhenCrouched();
            }
        }
        else
        {
            holdingDownUpInput = false;
            holdingDownDownInput = false;
        }

    }

    private void HandleHorizontalInputFromUser()
    {
        h = reWiredPlayer.GetAxis("Move Horizontally");

        //Debug.Log("--" + h);

        if (h > joystickBufferThreshold)
        {
            holdingDownLeftInput = false;
            holdingDownRightInput = true;
        }
        else if (h < (-1 * joystickBufferThreshold))
        {
            holdingDownLeftInput = true;
            holdingDownRightInput = false;
        }
        else
        {
            holdingDownLeftInput = false;
            holdingDownRightInput = false;
        }


        //*************************
        //*************************

        if (h > 0 && !facingRight)
        {
            Flip();
        }
        else if (h < 0 && facingRight)
        {
            Flip();
        }


    }

    private void HandleDirectionAnimationFlags()
    {
        h = reWiredPlayer.GetAxis("Move Horizontally");

        if ((Math.Abs(h) > joystickBufferThreshold) && !holdingDownUpInput && !holdingDownDownInput)
        {
            animator.SetBool("MovingHorizontally", true);
        }
        else
        {
            animator.SetBool("MovingHorizontally", false);
        }

        if (holdingDownUpInput)
        {
            animator.SetBool("HoldingUpInput", true);
        }
        else
        {
            animator.SetBool("HoldingUpInput", false);
        }

        if (holdingDownDownInput && !groundedCheckWideTrue)
        {
            animator.SetBool("HoldingDownInput", true);
        }
        else
        {
            animator.SetBool("HoldingDownInput", false);
        }
    }
    //*****************************
    //*****************************


    private void ControllerChanged()
    {
        switch (Useful.instance.ReturnCurrentController())
        {
            case ControllerType.Keyboard:
                _playerButtonPrompts.setCurrentController(PlayerButtonPrompts.ControllerType.Keyboard);
                debugCurrentControllerString = "Keyboard";
                break;

            default:
                _playerButtonPrompts.setCurrentController(PlayerButtonPrompts.ControllerType.Xbox);
                debugCurrentControllerString = "Xbox";
                break;
        }
    }

    private bool AbleToWallClingAndAttemptingTo()
    {
        return (
            (((leftWallNearby == true) && (holdingDownLeftInput == true) && (!illegalLeftWallNearby) && (!MonsterOnMyLeft())) ||
            ((rightWallNearby == true) && (holdingDownRightInput == true) && (!illegalRightWallNearby) && (!MonsterOnMyRight()))) &&
            !groundedCenterOnly &&
            statemachine.AbleToTakeDamage() &&
            (!pressedDownJumpInput && !holdingDownJumpInput)
            );
    }





    private void GroundLeto()
    {
        if (statemachine.AllowGrounding() && !statemachine.IsCurrentStateForcedJumpingUpward())
        {
            ForceGroundLeto();

            if (justGroundedCheckForAudioClip == false && (GetRigidBody2D().velocity.y < 0))
            {
                justGroundedCheckForAudioClip = true;
                Useful.instance.Play2DAudioClipOnNewGameObject(LetoFallSound, transform.position);
            }
        }

    }

    private void ForceGroundLeto()
    {
        FallingAnimationFlag = false;
        WallJumpingAnimationFlag = false;

        JumpState = 0;
        playerMovement.Ground();

        if (!statemachine.IsCurrentStateImmuneToDamage())
        {
            statemachine.ChangeState_GravityOn();
        }


        JumpButtonHeldDownBeforeFirstReleaseTimer = 0;
        jumpHeldDownTimer = 0;
        JumpAnimationFlag = false;
        jumpsLeftCount = playerMovement.GetMaxJumpCount();

        hasReleasedJumpInputOnce = false;
    }


    private void SpawnWallClimbingParticles()
    {
        //***********************************
        // --- spawn dust particle effects off wall
        ////***********************************
        if (wallSlidingDownJuiceBufferTimer <= 0)
        {
            GameObject particle = Instantiate(wallSlidingDownJuiceObject) as GameObject;
            particle.transform.position = gameObject.transform.position + new Vector3(0, -0.6F, 0);
            Destroy(particle, 0.5F);

            if (facingRight)
            {
                particle.transform.Rotate(0, 180, 0);
            }

            wallSlidingDownJuiceBufferTimer = 0.2F;
        }
        else if (wallSlidingDownJuiceBufferTimer > 0)
        {
            wallSlidingDownJuiceBufferTimer -= Time.deltaTime;
        }
        //***********************************
        // --- end spawn dust particle effects off wall
        ////***********************************
    }


    public bool WallNearby()
    {
        if (ceilingNearby && !illegalCeilingNearby)
        {
            return true;
        }
        else if (groundedCenterOnlyNoEnemyUnderneath)
        {
            return true;
        }
        else if (leftWallNearby && !illegalLeftWallNearby)
        {
            return true;
        }
        else if (rightWallNearby && !illegalRightWallNearby)
        {
            return true;
        }


        return false;
    }


    //*****************************
    //*****************************

    private void EnableButtonPrompt(PlayerButtonPrompts.PlayerActions playerAction)
    {
        ButtonPromptImage.GetComponent<SpriteRenderer>().enabled = true;
        ButtonPromptImage.GetComponent<SpriteRenderer>().sprite = _playerButtonPrompts.ReturnCorrectButtonSprite(playerAction);
    }


    public void EnableButtonPromptAfterDialogEnds()
    {
        ButtonPromptImage.GetComponent<SpriteRenderer>().enabled = true;
        ButtonPromptImage.GetComponent<SpriteRenderer>().sprite = _playerButtonPrompts.ReturnCorrectButtonSprite(PlayerButtonPrompts.PlayerActions.Interact);
    }


    public void DisableButtonPrompt()
    {
        ButtonPromptImage.GetComponent<SpriteRenderer>().enabled = false;
    }



    private void SpawnDashingEffects()
    {

        floorDashingJuiceBufferTimer -= Time.deltaTime;

        if (floorDashingJuiceBufferTimer <= 0)
        {
            GameObject particle = Instantiate(floorDashingJuiceObject) as GameObject;
            particle.name = "Floor Dashing Juice";


            if (ceilingNearby || ceilingNearbyExtraCheckForCeilingDashing)
            {
                particle.GetComponent<Rigidbody2D>().gravityScale = -1;
                particle.GetComponent<SpriteRenderer>().flipY = true;

                if (facingRight)
                {
                    particle.transform.position = gameObject.transform.position + new Vector3(0F, 0.1F, 0);
                }
                else
                {
                    particle.transform.position = gameObject.transform.position + new Vector3(0F, 0.1F, 0);
                    particle.GetComponent<SpriteRenderer>().flipX = true;
                }
            }
            else
            {
                if (!facingRight)
                {
                    particle.GetComponent<SpriteRenderer>().flipX = true;
                }

                particle.transform.position = gameObject.transform.position + new Vector3(0, -0.1F, 0);
            }

                
 

          
            Destroy(particle, 0.3F);

            floorDashingJuiceBufferTimer = 0.06F;
        }

    }
    public PlayerStats GetPlayerStats()
    {
        return playerStats;
    }

    public void SetPlayerStats(PlayerStats newPlayerStats)
    {
        playerStats = newPlayerStats;
    }

    public Rigidbody2D GetRigidBody2D()
    {
        return playerMovement.GetPlayerRigidBody2D();
    }

    public bool FacingRight()
    {
        return facingRight;
    }

    public bool FacingLeft()
    {
        return (!facingRight);
    }

    public bool FacingRightForCutsceneOnlyCheck()
    {
        return facingRightForCutsceneOnlyCheck;
    }
    

    public bool HoldingLeftOrRightInput()
    {
        return (holdingDownLeftInput || holdingDownRightInput);
    }



    public bool HoldingDownDownInput()
    {
        return holdingDownDownInput;
    }

    public void WarpToCurrentStartingLocation()
    {
        transform.position = startingPosition;
    }


    //*****************************
    //*****************************


    private void CheckForCollisionsAllDirections()
    {
        ceilingNearby = checkCollision(ceilingCheck.transform.position) ||
            checkCollision(ceilingLeftCheck.transform.position) ||
            checkCollision(ceilingRightCheck.transform.position);


        ceilingNearbyExtraCheckForCeilingDashing = checkCollision(ceilingExtraCheckForCeilingDashing.transform.position);

        standingOnEnemy = (checkLocalCollisions(groundCheckCenter.transform.position, 1 << LayerMask.NameToLayer("Enemy")));

        groundedCheckWideTrue = (checkCollision(groundCheckCenter.transform.position) || standingOnEnemy ||
            checkCollision(groundCheckLeft.transform.position) || checkCollision(groundCheckRight.transform.position));


        groundedCheckOneSideOnlyForFallingAnimationFlagTrue = (checkCollision(groundCheckLeft.transform.position) || checkCollision(groundCheckRight.transform.position));
        groundedCenterOnly = (checkCollision(groundCheckCenter.transform.position) || standingOnEnemy);
        groundedCenterOnlyNoEnemyUnderneath = checkCollisionGroundOnly(groundCheckCenter.transform.position);

        groundedCheckAllowForEdgeJumpTrue = (leftBottomDiagonalNearby || rightBottomDiagonalNearby);


        if (standingOnEnemy && EnemyILastTouched != null && EnemyILastTouched.enemyName == Enemy.EnemyNameEnum.Dysaur)
        {
            standingOnFlyingDysaur = true;
        }
        else
        {
            standingOnFlyingDysaur = false;
        }

        LeftTopCeilingDashingNearby = checkCollision(LeftTopCeilingDashingCheck.transform.position);
        RightTopCeilingDashingNearby = checkCollision(RightTopCeilingDashingCheck.transform.position);

        leftTopDiagonalNearby = checkCollision(leftTopDiagonalCheck.transform.position);
        leftBottomDiagonalNearby = checkCollision(leftBottomDiagonalCheck.transform.position);

        rightTopDiagonalNearby = checkCollision(rightTopDiagonalCheck.transform.position);
        rightBottomDiagonalNearby = checkCollision(rightBottomDiagonalCheck.transform.position);

        leftWallTopNearby = checkCollision(leftTopCheck.transform.position);
        leftWallMiddleNearby = checkCollision(leftMiddleCheck.transform.position);
        leftWallBottomNearby = checkCollision(leftBottomCheck.transform.position);
        leftWallNearby = (leftWallTopNearby || leftWallMiddleNearby || leftWallBottomNearby);

        rightWallTopNearby = checkCollision(rightTopCheck.transform.position);
        rightWallMiddleNearby = checkCollision(rightMiddleCheck.transform.position);
        rightWallBottomNearby = checkCollision(rightBottomCheck.transform.position);
        rightWallNearby = (rightWallTopNearby || rightWallMiddleNearby || rightWallBottomNearby);


        illegalCeilingNearby = checkIllegalWallCollision(LeftTopCeilingDashingCheck.transform.position) ||
            checkIllegalWallCollision(RightTopCeilingDashingCheck.transform.position);

        illegalLeftWallNearby = checkIllegalWallCollision(leftMiddleCheck.transform.position);
        illegalRightWallNearby = checkIllegalWallCollision(rightMiddleCheck.transform.position);


    }


    private bool MonsterOnMyLeft()
    {
        var leftWallTopNearbyMonster = checkMonsterCollisionOnly(leftTopCheck.transform.position);
        var leftWallMiddleNearbyMonster = checkMonsterCollisionOnly(leftMiddleCheck.transform.position);
        var leftWallBottomNearbyMonster = checkMonsterCollisionOnly(leftBottomCheck.transform.position);
        return (leftWallTopNearbyMonster || leftWallMiddleNearbyMonster || leftWallBottomNearbyMonster);
    }

    private bool MonsterOnMyRight()
    {
        var rightWallTopNearbyMonster = checkMonsterCollisionOnly(rightTopCheck.transform.position);
        var rightWallMiddleNearbyMonster = checkMonsterCollisionOnly(rightMiddleCheck.transform.position);
        var rightWallBottomNearbyMonster = checkMonsterCollisionOnly(rightBottomCheck.transform.position);
        return (rightWallTopNearbyMonster || rightWallMiddleNearbyMonster || rightWallBottomNearbyMonster);
    }

    private bool checkCollision(Vector2 endPoint)
    {
        return (
            Physics2D.Linecast(transform.position, endPoint, 1 << LayerMask.NameToLayer("Enemy")) ||
            Physics2D.Linecast(transform.position, endPoint, 1 << LayerMask.NameToLayer("Ground"))
            );
    }

    private bool checkCollisionGroundOnly(Vector2 endPoint)
    {
        return (          
            Physics2D.Linecast(transform.position, endPoint, 1 << LayerMask.NameToLayer("Ground"))
            );
    }

    private bool checkMonsterCollisionOnly(Vector2 endPoint)
    {
        RaycastHit2D hit;
        var cast = Physics2D.Linecast(transform.position, endPoint, 1 << LayerMask.NameToLayer("Enemy"));

        if (cast == true)
        {
            if (cast.collider.gameObject.GetComponent<Enemy>() != null)
            {
                var enemyScript = cast.collider.gameObject.GetComponent<Enemy>();
                return !enemyScript.IsAFriendlyMonster();
            }
        }


        return (Physics2D.Linecast(transform.position, endPoint, 1 << LayerMask.NameToLayer("Enemy")));
    }

    private bool checkIllegalWallCollision(Vector2 endPoint)
    {
        var returnObject = Physics2D.Linecast(transform.position, endPoint, 1 << LayerMask.NameToLayer("Ground"));

        if (returnObject.collider != null && returnObject.collider.tag == "ILLEGAL")
        {
            return true;
        }

        return false;
    }


    private bool checkLocalCollisions(Vector2 endPoint, int layerMask)
    {
        return (Physics2D.Linecast(transform.position, endPoint, layerMask));
    }


    //*****************************
    //*****************************

    public void Flip()
    {
        timesFlippedForStunLockState++;

        facingRight = !facingRight;

        Vector3 theScale = Sprite.transform.localScale;
        theScale.x *= -1;
        Sprite.transform.localScale = theScale;
    }



    public void FlipForCutsceneOnly()
    {
        facingRightForCutsceneOnlyCheck = !facingRightForCutsceneOnlyCheck;

        Vector3 theScale = Sprite.transform.localScale;
        theScale.x *= -1;
        Sprite.transform.localScale = theScale;
    }

    public void FlipLeftForCutsceneOnly()
    {
        if(facingRight == true)
        {
            FlipForCutsceneOnly();
        }
       
    }

    public void ResetFlip()
    {
        Sprite.transform.localScale = new Vector3(1, 1, 1);
        facingRightForCutsceneOnlyCheck = true;
        facingRight = true;
        Sprite.GetComponent<SpriteRenderer>().flipX = false;
    }







    private void VerticalFlipUp()
    {
        Vector3 theScale = Sprite.transform.localScale;
        theScale.y = 1;
        Sprite.transform.localScale = theScale;
    }

    private void VerticalFlipDown()
    {
        Vector3 theScale = Sprite.transform.localScale;
        theScale.y = -1;
        Sprite.transform.localScale = theScale;
    }


    public void ChangeSpawnPoint(float x, float y)
    {
        GameController.instance.SaveCameraSize();
        startingPosition = new Vector2(x, y);
    }

    public void ToggleGodModeEnabled()
    {
        _godModeEnabled = GameController.instance._gameSaveController.IsGodModeTurnedOn();
    }

    public void FullHeal()
    {
        playerStats.FullHeal();
    }


    //******************************



    private void PlaySoundFromAudioArray(List<AudioClip> list)
    {
        var clip = UnityEngine.Random.Range(0, list.Count);
        Useful.instance.Play2DAudioClipOnNewGameObject(list[clip], transform.position);
    }


    private bool soundLock = false;
    private void PlaySoundFromAudioArrayWithDelay(List<AudioClip> list, float delay)
    {
        if (soundLock == false)
        {
            soundLock = true;
            var clip = UnityEngine.Random.Range(0, list.Count);
            StartCoroutine(YieldToSound(list[clip], delay));
        }
    }


    private IEnumerator YieldToSound(AudioClip clip, float delay)
    {
        audioSource.clip = clip;
        audioSource.Play();

        yield return new WaitForSeconds(delay);
        soundLock = false;
    }



    private void SetCollidersWhenCrouched()
    {
        NormalDamageCollider.GetComponent<BoxCollider2D>().size = new Vector2(0.3F, 0.3F);
        NormalDamageCollider.GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.15F);

    }

    private void SetCollidersWhenNotCrouched()
    {
        NormalDamageCollider.GetComponent<BoxCollider2D>().size = new Vector2(0.3F, 0.6F);
        NormalDamageCollider.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
    }



  


}//end of class