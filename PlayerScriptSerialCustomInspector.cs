#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(PlayerScriptSerial))]
public class PlayerScriptSerialCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        PlayerScriptSerial script = (PlayerScriptSerial)target;

        EditorGUILayout.LabelField("Current Controller", script.currentController);

        EditorGUILayout.LabelField("Current State", script.currentState);
        EditorGUILayout.LabelField("NPC Node", script.DialogueNodeStartName);

        EditorGUILayout.LabelField("Wall Nearby?", script.wallNearby);

        EditorGUILayout.LabelField("Ceiling Nearby?", script.ceilingNearby);
        EditorGUILayout.LabelField("ceilingNearbyExtraCheckForCeilingDashing Nearby?", script.ceilingNearbyExtraCheckForCeilingDashing);
        EditorGUILayout.LabelField("(G) (Wide)?", script.grounded);
        EditorGUILayout.LabelField("(G) (Center)?", script.groundedCenterOnly);
        EditorGUILayout.LabelField("(G) (Center/Ground Only)?", script.groundedCenterOnlyNoEnemyUnderneath);
        

        EditorGUILayout.LabelField("(G) Allow for Edge Jumps?", script.groundedCheckAllowForEdgeJumpTrue);
        EditorGUILayout.LabelField("(G) groundedCheckOneSideOnlyForFallingAnimationFlagTrue?", script.groundedCheckOneSideOnlyForFallingAnimationFlagTrue);
        

        EditorGUILayout.LabelField("Left Diagonal Nearby?", script.leftDiagonalNearby);
        EditorGUILayout.LabelField("Right Diagonal Nearby?", script.rightDiagonalNearby);

        EditorGUILayout.LabelField("Left Wall Nearby?", script.leftWallNearby);
        EditorGUILayout.LabelField("Right Wall Nearby?", script.rightWallNearby);


        EditorGUILayout.LabelField("illegalCeilingNearby?", script.illegalCeilingNearby);
        EditorGUILayout.LabelField("illegalLeftWallNearby?", script.illegalLeftWallNearby);
        EditorGUILayout.LabelField("illegalRightWallNearby?", script.illegalRightWallNearby);


        EditorGUILayout.LabelField("Gravity Scale", script.gravityScale);

        EditorGUILayout.LabelField("Velocity X", script.velocityX);
        EditorGUILayout.LabelField("Velocity Y", script.velocityY);

        EditorGUILayout.LabelField("Jumps Left", script.jumpsLeft);
        EditorGUILayout.LabelField("Jumps State", script.jumpsState);

        if (GUI.changed)
            EditorUtility.SetDirty(target);

    }
}

#endif

