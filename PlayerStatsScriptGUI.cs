using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsScriptGUI : MonoBehaviour {

    private GameObject healthBarFill;
    private GameObject currentHPGUI;


    void Awake()
    {
        healthBarFill = GameController.instance.healthBarFillGUI;
        currentHPGUI = GameController.instance.currentHPGUI;
    }


    public void UpdateGUI(int currentHP, int maxHP)
    {
        if(currentHPGUI.GetComponent<Text>() != null && healthBarFill.GetComponent<Image>() != null)
        {
            currentHPGUI.GetComponent<Text>().text = currentHP.ToString();
            healthBarFill.GetComponent<Image>().fillAmount = ((float)currentHP / maxHP);
        }
        
    }

}
