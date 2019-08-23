using UnityEngine;

[System.Serializable]
public class PlayerScriptSerial : MonoBehaviour
{

    [SerializeField]
    public string currentController;

    [SerializeField]
    public string grounded;

    [SerializeField]
    public string groundedCenterOnly;

    [SerializeField]
    public string groundedCheckOneSideOnlyForFallingAnimationFlagTrue;

    [SerializeField]
    public string groundedCheckAllowForEdgeJumpTrue;


    [SerializeField]
    public string groundedCenterOnlyNoEnemyUnderneath;

    [SerializeField]
    public string currentState;

    [SerializeField]
    public string gravityScale;

    [SerializeField]
    public string velocityX;

    [SerializeField]
    public string velocityY;

    [SerializeField]
    public string wallNearby;

    [SerializeField]
    public string ceilingNearby;

    [SerializeField]
    public string ceilingNearbyExtraCheckForCeilingDashing;

    [SerializeField]
    public string leftWallNearby;


    [SerializeField]
    public string leftDiagonalNearby;

    [SerializeField]
    public string rightDiagonalNearby;


    [SerializeField]
    public string rightWallNearby;





    [SerializeField]
    public string illegalCeilingNearby;

    [SerializeField]
    public string illegalLeftWallNearby;

    [SerializeField]
    public string illegalRightWallNearby;



    [SerializeField]
    public string jumpsLeft;

    [SerializeField]
    public string jumpsState;


    [SerializeField]
    public string DialogueNodeStartName;


    private PlayerScript playerScript;
    private PlayerStats playerStats;
    private PlayerScriptStateMachine statemachine;

    private void Awake()
    {
        GameObject leto = GameObject.FindGameObjectWithTag("Leto");
        playerScript = leto.GetComponent<PlayerScript>();
        playerStats = leto.GetComponent<PlayerStats>();
        statemachine = leto.GetComponent<PlayerScriptStateMachine>();
        
        if (playerScript == null)
        {
            Debug.Log("Player Script Serializable - Leto's Script is missing?");
        }

        if (playerStats == null)
        {
            Debug.Log("Player Script Serializable - Leto's Stats are missing?");
        }
    }

    private void Update()
    {
		if(playerScript != null && playerStats != null && statemachine != null)
		{
            DialogueNodeStartName = playerScript.DialogueNodeStartName;

            grounded = playerScript.groundedCheckWideTrue.ToString();
            groundedCenterOnly = playerScript.groundedCenterOnly.ToString();


            groundedCheckOneSideOnlyForFallingAnimationFlagTrue = playerScript.groundedCheckOneSideOnlyForFallingAnimationFlagTrue.ToString();
            groundedCheckAllowForEdgeJumpTrue = playerScript.groundedCheckAllowForEdgeJumpTrue.ToString();
            groundedCenterOnlyNoEnemyUnderneath = playerScript.groundedCenterOnlyNoEnemyUnderneath.ToString();

            currentState = statemachine.GetCurrentState().ToString();
            currentController = playerScript.debugCurrentControllerString;

			if (playerScript.GetRigidBody2D() != null)
			{
				gravityScale = playerScript.GetRigidBody2D().gravityScale.ToString();
				velocityX = playerScript.GetRigidBody2D().velocity.x.ToString();
				velocityY = playerScript.GetRigidBody2D().velocity.y.ToString();
			}

            wallNearby = playerScript.WallNearby().ToString();

             ceilingNearbyExtraCheckForCeilingDashing = playerScript.ceilingNearbyExtraCheckForCeilingDashing.ToString();

            ceilingNearby = playerScript.ceilingNearby.ToString();
			leftWallNearby = playerScript.leftWallNearby.ToString();
			rightWallNearby = playerScript.rightWallNearby.ToString();

			leftDiagonalNearby = playerScript.leftTopDiagonalNearby.ToString();
			rightDiagonalNearby = playerScript.rightTopDiagonalNearby.ToString();

            illegalCeilingNearby = playerScript.illegalCeilingNearby.ToString();   
            illegalLeftWallNearby = playerScript.illegalLeftWallNearby.ToString();
            illegalRightWallNearby = playerScript.illegalRightWallNearby.ToString();


            jumpsLeft = playerScript.jumpsLeftCount.ToString();
                jumpsState = playerScript.JumpState.ToString();
				//maxJumpsLeft = playerStats.maxJumpCount.ToString();
		}
	}
}