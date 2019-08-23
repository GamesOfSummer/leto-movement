using MonsterLove.StateMachine;
using System.Collections;
using UnityEngine;

public class PlayerScriptStateMachine : MonoBehaviour
{
	private PlayerScript playerScript;
	private PlayerMovement playerMovement;
	private Vector2 hurtThrowDirection;

    private StateMachine<States> fsm;
    float originalTimeInForcedWallJumpState;

    private bool _overrideFasterJump = false;

    private bool chainJumpingToTheLeft = false;
    private bool chainJumpingToTheRight = false;

    public enum States
	{
		JustStartedLevel, // no control when you first start the level, only until you hit the ground
		GravityOn,
		NoControlNoGravity,
		Dashing, 
		ForcedJumping,
        HoldingAChargeJump,
        IsChainJumping,
        Hurt,
		ImmuneToDamage,
		Dead,
		NearNPC,
		InCutscene,
        StunLockedAndTakingDamage
	}

	
    public void InitilizeEngine()
	{
		playerScript = gameObject.GetComponent<PlayerScript>();
		playerMovement = gameObject.GetComponent<PlayerMovement>();
		
		fsm = StateMachine<States>.Initialize(this);

        originalTimeInForcedWallJumpState = playerMovement.timeInForcedWallJumpState;

        fsm.ChangeState(States.JustStartedLevel);
    }


	public States GetCurrentState()
	{
		return fsm.State;
	}



    public bool IsCurrentStateGravityOn()
    {
        return (fsm.State.Equals(States.GravityOn));
    }

    public bool IsCurrentStateJustStartedLevel()
	{
		return (fsm.State.Equals(States.JustStartedLevel));
	}

	public bool IsCurrentStateDead()
	{
		return (fsm.State.Equals(States.Dead));
	}

	public bool AbleToTakeDamage()
	{
		return 
			(
				!fsm.State.Equals(States.Hurt)
				&& !fsm.State.Equals(States.ImmuneToDamage)
				&& !fsm.State.Equals(States.Dead)
			);
	}


    public bool AllowGrounding()
    {
        return
            (
                !fsm.State.Equals(States.Hurt)
                && !fsm.State.Equals(States.Dead)
            );
    }

    public bool IsCurrentStateHurt()
	{
		return (fsm.State.Equals(States.Hurt));
	}

	public bool IsCurrentStateImmuneToDamage()
	{
		return (fsm.State.Equals(States.ImmuneToDamage));
	}

	public bool IsCurrentStateDashing()
	{
		return (fsm.State.Equals(States.Dashing));
	}

	public bool IsCurrentStateForcedJumpingUpward()
	{
		return (fsm.State.Equals(States.ForcedJumping));
	}


	public bool IsCurrentStateNearNPC()
	{
		return (fsm.State.Equals(States.NearNPC));
	}

	public bool IsCurrentStateInCutscene()
	{
		return (fsm.State.Equals(States.InCutscene));
	}

 

    public bool IsTalkingToAnNPC()
	{
		return
		(
			 fsm.State.Equals(States.InCutscene)
		);
	}

    public bool IsCurrentStateStunLockedAndTakingDamage()
    {
        return
        (
             fsm.State.Equals(States.StunLockedAndTakingDamage)
        );
    }


    public bool LimitPlayerVelocity()
	{
		return
			(
				!fsm.State.Equals(States.Hurt) &&
                !fsm.State.Equals(States.IsChainJumping) &&
                !fsm.State.Equals(States.NoControlNoGravity)
			);
	}

	public bool DoesCurrentStateAllowUserInput()
	{
		return
			(
				!fsm.State.Equals(States.JustStartedLevel) &&
				!fsm.State.Equals(States.Dead) &&
				!fsm.State.Equals(States.Hurt) &&
				!fsm.State.Equals(States.NoControlNoGravity) &&
				!fsm.State.Equals(States.InCutscene) &&
                !fsm.State.Equals(States.StunLockedAndTakingDamage)
			);
	}




	public void ChangeState_GravityOn()
	{
        if(!IsCurrentStateStunLockedAndTakingDamage())
        {
            fsm.ChangeState(States.GravityOn);
        }	
	}

	public void ChangeState_GravityOn(float timer)
	{
        if (!IsCurrentStateStunLockedAndTakingDamage())
        {
            StartCoroutine(TurnGravityBackOn(timer));
        }      
	}

	private void GravityOn_Enter()
	{
		//Debug.Log("*** Leto :: GravityOn_Enter() ***");
		//playerMovement.ResetGravityToOne();
	}

	private IEnumerator TurnGravityBackOn(float timer = .1F)
	{
		yield return new WaitForSeconds(timer);
		fsm.ChangeState(States.GravityOn);
	}




    public void ChangeState_HoldingAChargeJump()
    {
        if (!IsCurrentStateStunLockedAndTakingDamage())
        {
            fsm.ChangeState(States.HoldingAChargeJump);
        }
    }

    private void HoldingAChargeJump_Enter()
    {
        Debug.Log("*** HoldingAChargeJump_Enter ***");
        playerMovement.SlowDownRigidBody2DForChargeJump();
        //playerMovement.ResetGravityToOne();
    }

    private void HoldingAChargeJump_Exit()
    {
        Debug.Log("*** HoldingAChargeJump_Enter ***");
        playerMovement.ResetRigidbody2DAfterChargeJump();
        //playerMovement.ResetGravityToOne();
    }






    public void SetChainJumpingLeft()
    {
        chainJumpingToTheLeft = true;
    }

    public void SetChainJumpingRight()
    {
        chainJumpingToTheRight = true;
    }

    public void ChangeState_IsChainJumping()
    {
        if (!IsCurrentStateStunLockedAndTakingDamage())
        {
            fsm.ChangeState(States.IsChainJumping);
        }
    }

    private void IsChainJumping_Enter()
    {
        Debug.Log("*** IsChainJumping_Enter ***");
        StartCoroutine(TurnGravityBackOn(0.1F));
    }

    //private float someValueLol = 7.0F;
    private void IsChainJumping_FixedUpdate()
    {

        if(chainJumpingToTheLeft == true)
        {
           // playerMovement.ChangeHorizontalVelocity(-someValueLol);
        }
        else if(chainJumpingToTheRight == true)
        {
            //playerMovement.ChangeHorizontalVelocity(someValueLol);
        }
        else
        {
            //playerMovement.ChangeVerticalVelocity(someValueLol);
        }
        
    }


    private void IsChainJumping_Exit()
    {
        Debug.Log("*** IsChainJumping_Exit ***");
        chainJumpingToTheLeft = false;
        chainJumpingToTheRight = false;
    //playerMovement.ResetRigidbody2DAfterChargeJump();
    //playerMovement.ResetGravityToOne();
    }




    public void ChangeState_Dashing()
	{
		fsm.ChangeState(States.Dashing);
	}

	public void ChangeState_ForcedJump()
	{
		fsm.ChangeState(States.ForcedJumping);
	}

    
    public void ChangeState_ForcedJump(float time, bool overrideFasterJump = false)
    {
        playerMovement.timeInForcedWallJumpState = time;
        _overrideFasterJump = overrideFasterJump;
        fsm.ChangeState(States.ForcedJumping);
    }


    private void ForcedJumping_Enter()
	{
		StartCoroutine(TurnGravityBackOn(playerMovement.timeInForcedWallJumpState));
	}

    private void ForcedJumping_Exit()
    {
        playerMovement.timeInForcedWallJumpState = originalTimeInForcedWallJumpState;
    }


    public void ResetJumpSpeedWhenGrounded()
    {
        _overrideFasterJump = false;
    }

    public bool IsOverriddenFasterJump()
    {
        //Debug.Log(_overrideFasterJump);
        return _overrideFasterJump;
    }


    public void ChangeStateToHurt(float stayStunnedTimer)
    {
        hurtThrowDirection = Vector2.zero;
        fsm.ChangeState(States.Hurt);
        StartCoroutine(ChangeToImmuneToDamageIEnumerator(stayStunnedTimer));
    }

    public void ChangeStateToHurt(float stayStunnedTimer, Vector2 throwDirection)
	{
		hurtThrowDirection = throwDirection;
		fsm.ChangeState(States.Hurt);
		StartCoroutine(ChangeToImmuneToDamageIEnumerator(stayStunnedTimer));
	}

	public void Hurt_FixedUpdate()
	{
		playerMovement.MoveWithVector(hurtThrowDirection);
		playerScript.HurtAnimationFlag = true;
	}

	public void Hurt_Finally()
	{
		playerScript.HurtAnimationFlag = false;
	}


    

    private IEnumerator ChangeToImmuneToDamageIEnumerator(float timer)
	{
		yield return new WaitForSeconds(timer);
        ChangeToImmuneToDamage();
    }

    public void ChangeToImmuneToDamage()
    {
        fsm.ChangeState(States.ImmuneToDamage);
        StartCoroutine(TurnGravityBackOn(1.0F));
    }


    public void ImmuneToDamage_FixedUpdate()
	{
		float a = playerScript.animator.GetComponent<SpriteRenderer>().color.a;

		if (a < .2)
		{
			playerScript.animator.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1.0f);
		}
		else
		{
			playerScript.animator.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (a - 0.1F));
		}

	}


	public void ImmuneToDamage_Finally()
	{
		playerScript.animator.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1.0f);
	}



	public void ChangeStateToDead(float respawnTimer)
	{
		fsm.ChangeState(States.Dead);
		StartCoroutine(TurnGravityBackOn(respawnTimer));
	}


	public void Dead_Enter()
	{
		//Debug.Log("*** Leto :: Dead_Enter() ***");
	}

	public void Dead_FixedUpdate()
	{
        //Debug.Log("*** Leto :: Dead_FixedUpdate() ***");
        playerMovement.ZeroOutAllVelocity();
	}

	public void Dead_Finally()
	{
		//Debug.Log("*** Leto :: Dead_Finally() ***");

		GameController.instance.ResetCameraSize();
		playerScript.playerStats.resetCurrentHPtoMax();
		playerScript.WarpToCurrentStartingLocation();
	}


	public void ChangeStateToNoControlNoGravity()
	{
		fsm.ChangeState(States.NoControlNoGravity);
	}

	private void NoControlNoGravity_Enter()
	{
		Debug.Log("*** Leto :: NoControlNoGravity_Enter() ***");
		//playerMovement.ResetGravityToZero();
		StartCoroutine(TurnGravityBackOn(.5F));
	}


	private IEnumerator TurnToNearNPCBackOn(float timer = 0.1F)
	{
		yield return new WaitForSeconds(timer);

		if (playerScript.DialogueNodeStartName == null)
		{
			Debug.Log("Message - TurnToNearNPCBackOn() - NPC is dead or disabled as there is a null start dialog node");
			fsm.ChangeState(States.GravityOn);
		}
		else
		{
			fsm.ChangeState(States.NearNPC);
		}

	}

	public void ChangeStateNearNPC()
	{       
		fsm.ChangeState(States.NearNPC);
	}


    private void NearNPC_FixedUpdate()
    {
        if(playerScript.DialogueNodeStartName == null)
        {
            Debug.Log("Message! - NearNPC_FixedUpdate() - forced to gravity on");
            fsm.ChangeState(States.GravityOn);
        }
    }


    public void ChangeStateInCutscene()
	{
		playerMovement.ZeroOutAllVelocity();
		fsm.ChangeState(States.InCutscene);
	}

    private void InCutscene_Enter()
    {
        playerScript.DisableButtonPrompt();
    }

    private void InCutscene_FixedUpdate()
	{
        playerMovement.ZeroOutAllHorizontalVelocity();
	}

    private void InCutscene_Exit()
    {
        playerScript.EnableButtonPromptAfterDialogEnds();
    }

    float immuneToDamageFromStunLockTimer = 0.0F;
    public void ChangeStateToStunLockedAndTakingDamage(float stayStunnedTimer)
    {
        if(!IsCurrentStateDead())
        {
            immuneToDamageFromStunLockTimer = stayStunnedTimer;
            fsm.ChangeState(States.StunLockedAndTakingDamage);
        }     
    }

    public void StunLockedAndTakingDamage_Enter()
    {
        playerScript.HurtAnimationFlag = true;
        playerScript.StunLockButtonPromptAnimator.enabled = true;
        playerScript.StunLockButtonPromptAnimator.GetComponentInChildren<SpriteRenderer>().enabled = true;
    }


    public void StunLockedAndTakingDamage_FixedUpdate()
    {
        playerScript.HurtAnimationFlag = true;
        playerMovement.ZeroOutAllVelocity();   
    }


    public void StunLockedAndTakingDamage_Finally()
    {
        playerScript.HurtAnimationFlag = false;
        playerScript.StunLockButtonPromptAnimator.enabled = false;
        playerScript.StunLockButtonPromptAnimator.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }


   
}
