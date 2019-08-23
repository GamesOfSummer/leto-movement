using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

    private PlayerScript _playerScript;

    private Rigidbody2D myRigidbody2D;
    private PlayerScriptStateMachine statemachine;

    [SerializeField]
    private float moveHorizontallyInPixels;

    [SerializeField]
    private float fallSpeedInPixels;

    [SerializeField]
    public float jumpSpeedInPixels;

    [SerializeField]
    private float jumpTimerReset = 0.5F;      //time allowed to 'hold' jump down

    public float timeInForcedWallJumpState = 0.1F;


    [SerializeField]
    private float dashVerticallyForce;
    [SerializeField]
    private float dashHorizontallyForce;


	[SerializeField]
    private bool canDash;
    [SerializeField]
    private bool canChargeJump;
    [SerializeField]
    private bool canChainJump;

    [SerializeField]
    private bool canDoubleJump;


    private float recalculatedFallSpeed = 0;
    private float fallingT = 0.0F;
    private float slowerJumpValue;
    private float jumpingT = 0.0F;

    public void Awake()
    {
        statemachine = gameObject.GetComponent<PlayerScriptStateMachine>();
        myRigidbody2D = GetComponent<Rigidbody2D>();

        _playerScript = GetComponent<PlayerScript>();
    }

    public void MoveLeft()
    {
        MoveHorizontal(-moveHorizontallyInPixels);
    }

    public void MoveRight()
    {
        MoveHorizontal(moveHorizontallyInPixels);
    }

    public void WallStick()
    {
        MoveVertical(0);
    }


    public void MoveWithVectorOverride(Vector2 direction)
    {
        GetComponent<Transform>().Translate(new Vector3(direction.x, direction.y, 0));
    }

    public void MoveWithVector(Vector2 direction)
    {
        MoveHorizontal(direction.x);
        MoveVertical(direction.y);
    }


    private void MoveHorizontal(float speed)
    {
        speed = Useful.instance.ConvertUnityUnitsToPixels(speed);
        var distanceToMove = _playerScript.CalculateHorizontalDistance(speed);

        GetComponent<Transform>().Translate(new Vector3(distanceToMove, 0, 0));
        GetComponent<Transform>().position = Useful.instance.RoundVectorToTwoPlaces(GetComponent<Transform>().position);
        GetComponent<Transform>().position = _playerScript.FinalAdjustHorizontalSpeed(speed);
    }


    private void MoveVertical(float speed)
    {
        speed = Useful.instance.ConvertUnityUnitsToPixels(speed);
        var distanceToMove = _playerScript.CalculateVerticalDistance(speed);
       
        GetComponent<Transform>().Translate(new Vector3(0, distanceToMove, 0));
        GetComponent<Transform>().position = Useful.instance.RoundVectorToTwoPlaces(GetComponent<Transform>().position);
        GetComponent<Transform>().position = _playerScript.FinalAdjustVerticalSpeed(speed);
    }




    public void ApplyGravity()
    {
        fallingT += (Time.deltaTime);

        var percentageTo100 = Mathf.Lerp(0, 1, (fallingT));
        var easedPercentage = Easings.QuinticEaseOut(percentageTo100);
        recalculatedFallSpeed = easedPercentage * fallSpeedInPixels;

        //Debug.Log(fallingT + " - " + percentageTo100 + " -- " + easedPercentage + " - " + recalculatedFallSpeed);

        MoveVertical(-1 * (recalculatedFallSpeed));
    }



    public void Jump()
    {
        if(slowerJumpValue > 0)
        {      
            jumpingT += (Time.deltaTime);

            if(statemachine.IsOverriddenFasterJump())
            {
                var percentageTo100 = Mathf.Lerp(0, 1, (jumpingT));
                var easedPercentage = Easings.QuinticEaseOut(percentageTo100);
                slowerJumpValue = (easedPercentage * jumpSpeedInPixels) * .7F;
            }
            else
            {
                var percentageTo100 = Mathf.Lerp(0, 1, (jumpingT));
                var easedPercentage = Easings.QuinticEaseOut(percentageTo100);
                slowerJumpValue = easedPercentage * jumpSpeedInPixels;
            }


            //Debug.Log(jumpingT + " - " + percentageTo100 + " -- " + easedPercentage + " - " + slowerJumpValue + "===>" + (jumpSpeedInPixels - slowerJumpValue));

            MoveVertical(jumpSpeedInPixels - slowerJumpValue);
        }  
    }

    public void Ground()
    {
        jumpingT = 0.0F;
        fallingT = 0.0F;
        slowerJumpValue = jumpSpeedInPixels;
        recalculatedFallSpeed = 0;
        statemachine.ResetJumpSpeedWhenGrounded();
    }



    public void ZeroOutAllHorizontalVelocity()
    {
        MoveHorizontal(0);
    }


    public void ZeroOutAllVelocity()
    {
        MoveWithVector(new Vector2(0, 0));
    }


    public bool IsDashEnabled()
    {
        return canDash;
    }

	public void SetIsDashEnabled(bool flag)
	{
		canDash = flag;
	}



    public void DashingOnCeiling(int direction)
    {
        MoveHorizontal(direction * dashHorizontallyForce);
        MoveVertical(0);
    }

    public void DashingOnWall(int direction)
    {
        MoveHorizontal(0);
        MoveVertical(direction * dashVerticallyForce);
    }

    public void DashingOnGround(int direction)
    {
        MoveHorizontal(direction * dashHorizontallyForce);
    }



    public bool IsChargeJumpEnabled()
    {
        return canChargeJump;
    }

    public void SetChargeJumpEnabled(bool flag)
    {
        canChargeJump = flag;
    }

    public bool IsChainJumpEnabled()
    {
        return canChainJump;
    }



    public void SetChainJumpEnabled(bool flag)
    {
        canChainJump = flag;
    }

    public bool IsDoubleJumpEnabled()
    {
        return canDoubleJump;
    }

    public void SetDoubleJump(bool flag)
    {
        canDoubleJump = flag;
    }

    public Rigidbody2D GetPlayerRigidBody2D()
    {
        return myRigidbody2D;
    }
    


    public int GetMaxJumpCount()
    {
        if(canDoubleJump)
        {
            return 2;
        }

        return 1;

    }



    public void SlowDownRigidBody2DForChargeJump()
    {
        //moveForceGradual = moveForceGradual / 3;
        //moveForceInstant = moveForceInstant / 3;
        //originalRigidbody2DMass = myRigidbody2D.
        //myRigidbody2D.gravityScale = 1;
    }

    public void ResetRigidbody2DAfterChargeJump()
    {
        //moveForceGradual = moveForceGradual * 3;
        //moveForceInstant = moveForceInstant * 3;
    }


    public float GetJumpForce()
    {
        return jumpSpeedInPixels;
    }

    public float GetJumpForceTimeToReset()
    {
        return jumpTimerReset;
    }


    public float GetHorizontalDashForce()
    {
        return dashHorizontallyForce;
    }

    public float GetVeritcalDashForce()
    {
        return dashVerticallyForce;
    }




}
