using System.Collections;
using UnityEngine;


[RequireComponent(typeof(PlayerStatsScriptGUI))]
public class PlayerStats : MonoBehaviour
{
    public int currentHP;
    public int maxHP;
    public float attack;
    public float defense;


    private PlayerStatsScriptGUI gui;

    public AudioClip lowHealthAudio;
    private AudioSource _audioSource;

    public void Awake()
    {
        gui = GetComponent<PlayerStatsScriptGUI>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.spatialBlend = 0;


        currentHP = maxHP;
        InvokeRepeating("LowHealthBeep", 2.0f, 1.5f);
    }



    private void LowHealthBeep()
    {
        if ( CurrentHealthFraction() < 0.25F && currentHP > 0)
        {
            _audioSource.clip = lowHealthAudio;
            _audioSource.Play();
        }
    }


    private float CurrentHealthFraction()
    {
        float currentHealthFloat = currentHP;
        float maxHealthFloat = maxHP;


        return currentHealthFloat / maxHealthFloat;
    }


    public void HealSmallHealth()
	{
		int value = (int)(.10 * maxHP);

		DenyOverHeal(value + currentHP);
		gui.UpdateGUI(currentHP, maxHP);
		GameController.instance.increaseDustScore(1);
	}


	public void HealBigHealth()
    {
        int value = (int)(.25 * maxHP);

        DenyOverHeal(value + currentHP);
        gui.UpdateGUI(currentHP, maxHP);
		GameController.instance.increaseDustScore(20);
    }

    public void FullHeal()
    {
        currentHP = maxHP;
        gui.UpdateGUI(currentHP, maxHP);
    }

    public void SavePointHeal()
    {
        DenyOverHeal(maxHP);
        gui.UpdateGUI(maxHP, maxHP);
    }


    private void DenyOverHeal(int newValue)
    {
        if (newValue > maxHP)
        {
            currentHP = maxHP;
        }
		else
		{
			currentHP = newValue;
		}
    }

    public void takeDamage(float damage)
    {
        takeDamage((int)damage);
    }

    //TODO - defense
    public void takeDamage(int damage)
    {
        currentHP = currentHP - damage;

        if( currentHP < 0)
        {
            currentHP = 0;
        }


       gui.UpdateGUI(currentHP, maxHP);

    }



    public bool isDead()
    {
        return (currentHP <= 0);
    }

    public void resetCurrentHPtoMax()
    {
        currentHP = maxHP;
        gui.UpdateGUI(currentHP, maxHP);
    }


}