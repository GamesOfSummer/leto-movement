using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerScript : MonoBehaviour
{
    // there's a race condition with how the game starts, how leto loads, and
    // how leto knows where she spawned at
    // needed so the companion spawns in the right position and not 0,0
    private IEnumerator InitializeCompanionArrayCoroutine()
    {
        yield return new WaitForSeconds(0.01F);
        InitializeCompanionArray();
    }


    private void InitializeCompanionArray()
    {
        var companionSlot = Useful.instance.FindGameObjectByNameInChildren(this.gameObject, "CompanionSlot");
        companionSlot.GetComponent<SpringJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();

        currentCompanionGameObject = companionSlot;
        currentCompanionGameObject.transform.position = currentCompanionGameObject.transform.TransformPoint(currentCompanionGameObject.transform.localPosition);


        addCompanion(CompanionNameEnum.NONE);
        SetUpLetoDependingOnProgress();
    }


    private void SetUpLetoDependingOnProgress()
    {
        var _levelNameManager = GameController.instance._levelNameManager;
        var _currentLevelEnum = GameController.instance._currentLevelEnum;
        var _timeManager = GameController.instance._timeManager;
        var currentDustScoreInt = GameController.instance.currentDustScoreInt;

        if (!_levelNameManager.IsJohnMule(_currentLevelEnum) || _timeManager.DayInGame == TimeManager.Day.Day2)
        {
            LoadAdditionalCompanionsDependingOnLevel();
        }
        else if (_levelNameManager.IsJohnMule(_currentLevelEnum)) //save so alma's location is correct
        {
            _gameSaveController.SavedGameOnRegularLevel(_currentLevelEnum, currentDustScoreInt, currentCompanionArrayIndex);
        }
    }


    private void LoadAdditionalCompanionsDependingOnLevel()
    {
        var _levelNameManager = GameController.instance._levelNameManager;
        var _currentLevelEnum = GameController.instance._currentLevelEnum;
        var _timeManager = GameController.instance._timeManager;

        if (_timeManager.DayInGame == TimeManager.Day.Day5)
        {
            //nothing, no companions as it's the Scilla fight
            Debug.Log("Reminder - Day 5 means NO COMPANIONS LOAD");
        }
        else if (_currentLevelEnum == LevelNameManager.LevelEnum.__debugroom)
        {
            addCompanion(CompanionNameEnum.Gilbert);
            addCompanion(CompanionNameEnum.Tabris);
            addCompanion(CompanionNameEnum.Oz);

            setActiveCompanion(CompanionNameEnum.NONE);

        }
        else
        {
            if (_gameSaveController.HasGilbert() && (LevelNameManager.LevelEnum.level_2_caves_2 != _currentLevelEnum))
            {
                addCompanion(CompanionNameEnum.Gilbert);
            }

            if (_gameSaveController.HasTabris())
            {
                addCompanion(CompanionNameEnum.Tabris);
            }

            if (_gameSaveController.HasOz())
            {
                addCompanion(CompanionNameEnum.Oz);
            }


            if (_currentLevelEnum == LevelNameManager.LevelEnum.level_1_grasslands_2 || _currentLevelEnum == LevelNameManager.LevelEnum.level_2_caves_1)
            {
                setActiveCompanion(CompanionNameEnum.Gilbert);
            }
            else if (_currentLevelEnum == LevelNameManager.LevelEnum.level_2_caves_4)
            {
                setActiveCompanion(CompanionNameEnum.Tabris);
            }
            else if (_currentLevelEnum == LevelNameManager.LevelEnum.level_3_lava_1)
            {
                setActiveCompanion(CompanionNameEnum.NONE);
            }
            else if (_currentLevelEnum == LevelNameManager.LevelEnum._johnmule && _timeManager.DayInGame == TimeManager.Day.Day2)
            {
                setActiveCompanion(CompanionNameEnum.Tabris);
                Useful.instance.SetAnimationFlagOutsideDialogueAfterDelay("Tabris(Clone)", "NonHostile");
            }
            else
            {
                SetupLastSavedCompanion();
            }

        }

    }



    public void SetupLastSavedCompanion()
    {
        int lastUsedCompanionIndex = _gameSaveController.ReturnIntValue(SavedGameFlagsINT.int_last_companion_used);

        if (lastUsedCompanionIndex <= companionArray.Count)
        {
            SetCompanionByIndexOfCompanionArray(lastUsedCompanionIndex);
        }
    }



    public void SetCompanionByIndexOfCompanionArray(int lastUsedCompanionIndex)
    {
        int i = 0;
        foreach (GameObject companion in companionArray)
        {
            if (lastUsedCompanionIndex == i)
            {
                currentCompanionArrayIndex = lastUsedCompanionIndex;
                var holder = CompanionNameEnum.NONE.GetCompanionEnumFromString(companion.name);
                setActiveCompanion(holder);
            }
            i++;
        }

    }


    public void addCompanion(CompanionNameEnum companionNameEnum)
    {
        companionArray.Add(Resources.Load<GameObject>("Companions/" + companionNameEnum.GetStringValue()));
        ReplaceOldCompanionWithNew(companionNameEnum);
    }

    public void setActiveCompanion(CompanionNameEnum companionNameEnum)
    {
        string companionName = companionNameEnum.Value;
        int i = 0;
        foreach (var name in companionArray)
        {
            if (name.name == companionName)
            {
                currentCompanionArrayIndex = i;
                ReplaceOldCompanionWithNew(companionNameEnum);
            }
            i++;

        }
    }


    public void switchToNextCompanion()
    {
        currentCompanionArrayIndex++;
        if (currentCompanionArrayIndex > (companionArray.Count - 1))
        {
            currentCompanionArrayIndex = 0;
        }

        ReplaceOldCompanionWithNew(CompanionNameEnum.Gilbert.GetCompanionEnumFromString(companionArray[currentCompanionArrayIndex].name));

    }


    private void ReplaceOldCompanionWithNew(CompanionNameEnum companionNameEnum)
    {
        string companionName = companionNameEnum.Value;
        int i = 0;
        foreach (var name in companionArray)
        {
            if (name.name == companionName)
            {
                GameObject companion = GameObject.Instantiate(companionArray[i]) as GameObject;
                ReplaceOldCompanionWithNew(companion);
            }
            i++;
        }
    }


    private void ReplaceOldCompanionWithNew(GameObject companion)
    {
        Rigidbody2D rigidbody2D = currentCompanionGameObject.GetComponent<Rigidbody2D>();
        SpringJoint2D springjoint2D = currentCompanionGameObject.GetComponent<SpringJoint2D>();

        companion.transform.position = currentCompanionGameObject.transform.position;
        companion.transform.position = new Vector3(companion.transform.position.x, companion.transform.position.y, 0);


        companion.GetComponent<Companion>().SetPlayerFields(gameObject, this, playerMovement, playerStats);


        if (companion.GetComponent<SpringJoint2D>() == null)
        {
            companion.AddComponent<SpringJoint2D>();
        }

        companion.GetComponent<SpringJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
        companion.GetComponent<SpringJoint2D>().autoConfigureDistance = false;
        companion.GetComponent<SpringJoint2D>().frequency = springjoint2D.frequency;
        companion.GetComponent<SpringJoint2D>().distance = springjoint2D.distance;
        companion.GetComponent<SpringJoint2D>().dampingRatio = springjoint2D.dampingRatio;


        if (companion.GetComponent<Rigidbody2D>() == null)
        {
            companion.AddComponent<Rigidbody2D>();
        }

        companion.GetComponent<Rigidbody2D>().mass = rigidbody2D.mass;
        companion.GetComponent<Rigidbody2D>().gravityScale = rigidbody2D.gravityScale;
        companion.GetComponent<Rigidbody2D>().drag = rigidbody2D.drag;
        companion.GetComponent<Rigidbody2D>().freezeRotation = true;

        Destroy(currentCompanionGameObject);


        currentCompanionGameObject = companion;
        companionScript = currentCompanionGameObject.GetComponent<Companion>().ReturnScript();
        HandleAnyNeededCompanionUpgrades(currentCompanionGameObject);

    }



    public Rigidbody2D ReturnCompanionRigidbody()
    {
        return currentCompanionGameObject.GetComponent<Rigidbody2D>();
    }

    private void HandleAnyNeededCompanionUpgrades(GameObject activeCompanion)
    {
        //Debug.Log(activeCompanion.name + " -> " + CompanionNameEnum.Gilbert.GetStringValue());
        activeCompanion.GetComponent<Companion>().SetBulletNowHasABonusFromGameProgression(0);

        if (IsGilbertAndHeHasTrainedWithTabris(activeCompanion))
        {
            Debug.Log("Upgraded Gilbert!");
            activeCompanion.GetComponent<Companion>().SetBulletNowHasABonusFromGameProgression(35);
        }

        if(IsLavaLevelAndGilbertIsNaked(activeCompanion))
        {
            Debug.Log("Upgraded Gilbert to being naked!");
            activeCompanion.GetComponent<Companion>().SetBoolFlagOnAnimator("IsNaked");
        }

    }

    private bool IsGilbertAndHeHasTrainedWithTabris(GameObject activeCompanion)
    {
        return ((int)GameController.instance._timeManager.GetCurrentDay() > 2) && IsGilbert(activeCompanion);
    }

    private bool IsLavaLevelAndGilbertIsNaked(GameObject activeCompanion)
    {
        return (_gameSaveController.ReturnBoolValue(SavedGameFlagsBOOL.bool_day_3_gilbert_is_naked) && IsGilbert(activeCompanion));
    }

    private bool IsGilbert(GameObject activeCompanion)
    {
        return activeCompanion.name.Contains(CompanionNameEnum.Gilbert.GetStringValue());
    }
}
