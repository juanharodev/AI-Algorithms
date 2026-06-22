using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tamagochi : MonoBehaviour
{
    [Header("Hunger")]
    [Range(0f, 1f)] public float currentHunger = 1f;
    public AnimationCurve hungerCurve;
    public Image hungerBar;


    [Header("Sanity")]
    [Range(0f, 1f)] public float currentSanity = 1f;
    public AnimationCurve sanityCurve;
    public Image sanityBar;


    [Header("Gambling")]
    [Range(0f, 1f)] public float currentGambling = 1f;
    public AnimationCurve gamblingCurve;
    public Image gambling;



    [Header("Sleep")]
    [Range(0f, 1f)] public float currentSleep = 1f;
    public AnimationCurve sleepCurve;
    public Image sleepBar;


    [Header("Colors")]
    public Color lowColor;
    public Color highColor;

    [Header("Lose per second")]
    public float minLose;
    public float maxLose;
    float hungerLose;
    float sanityLose;
    float gamblingLose;
    float sleepLose;

    [Header("Buttons")]
    public List<Button> buttons;

    [Header("Text")]
    public TextMeshProUGUI statusDisplay;


    private void Start()
    {
        StartCoroutine(RerollLoose());
        StartCoroutine(CheckStatusDelay());
    }

    private void Update()
    {
        currentHunger = Mathf.Max(0, currentHunger - (Time.deltaTime * hungerLose * 0.25f));
        currentSanity = Mathf.Max(0, currentSanity - (Time.deltaTime * sanityLose * 0.25f));
        currentGambling = Mathf.Max(0, currentGambling - (Time.deltaTime * gamblingLose * 0.25f));
        currentSleep = Mathf.Max(0, currentSleep - (Time.deltaTime * sleepLose * 0.25f));

        hungerBar.color = GetColor(hungerCurve.Evaluate(currentHunger));
        sanityBar.color = GetColor(sanityCurve.Evaluate(currentSanity));
        gambling.color = GetColor(gamblingCurve.Evaluate(currentGambling));
        sleepBar.color = GetColor(sleepCurve.Evaluate(currentSleep));

        hungerBar.fillAmount = hungerCurve.Evaluate(currentHunger);
        sanityBar.fillAmount = sanityCurve.Evaluate(currentSanity);
        gambling.fillAmount = gamblingCurve.Evaluate(currentGambling);
        sleepBar.fillAmount = sleepCurve.Evaluate(currentSleep);
    }

    private IEnumerator RerollLoose()
    {
        while (true)
        {
            hungerLose = UnityEngine.Random.Range(minLose, maxLose);
            sanityLose = UnityEngine.Random.Range(minLose, maxLose);
            gamblingLose = UnityEngine.Random.Range(minLose, maxLose);
            sleepLose = UnityEngine.Random.Range(minLose, maxLose);
            yield return new WaitForSeconds(1);
        }
    }

    Color GetColor(float value)
    {
        return Color.Lerp(lowColor, highColor, value);
    }

    public void AddStat(int toAdd)
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
        switch (toAdd)
        {
            case 0:
                currentHunger = MathF.Min(currentHunger + 0.1f, 1);
                break;
            case 1:
                currentSanity = MathF.Min(currentSanity + 0.1f, 1);
                break;
            case 2:
                currentGambling = MathF.Min(currentGambling + 0.1f, 1);
                break;
            case 3:
                currentSleep = MathF.Min(currentSleep + 0.1f, 1);
                break;
            default:
                Debug.LogError("Stat must be between 0 and 3");
                break;
        }
        StartCoroutine(EnableButtons());
    }

    IEnumerator EnableButtons()
    {
        yield return new WaitForSeconds(1.5f);
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }

    void CheckStatus()
    { 
        statusDisplay.text = string.Empty;

        if (currentHunger >= 0.9)
        {
            statusDisplay.text += "Suficiente comida.... por ahora\n";
        }else if(currentHunger >= 0.6)
        {
            statusDisplay.text += "treat?\n";
        }
        else if(currentHunger >= 0.3)
        {
            statusDisplay.text += "A que hora comen en este autobus?\n";
        }
        else
        {
            statusDisplay.text += "Comida!! Ahora!!!!!\n";
        }

        if (sanityCurve.Evaluate(currentSanity) < 0.3f)
        {
            statusDisplay.text += "DISTORT DISTORT DISTOR!!!!!\n";
        }
        else if (currentSanity < 0.4f)
        {
            statusDisplay.text += "Loca y highrolller\n";
        }
        else if (currentSanity < 0.6)
        {
            statusDisplay.text += "Hasta con la sanidad gamblea\n";
        }
        else
        {
            statusDisplay.text += "Rolleando como los grandes\n";
        }

        if (gamblingCurve.Evaluate(currentGambling) > 0.8)
        {
            statusDisplay.text += "Apostar es su pasion\n";
        } else if (currentGambling < 0.3)
        {
            statusDisplay.text += "Let her gamble, I said let her gamble\n";
        }
        else
        {
            statusDisplay.text += "Apostar es su perdision\n";
        }

        if (sleepCurve.Evaluate(currentSleep) >= 0.9)
        {
            statusDisplay.text += "Nada como estar bien descansado\n";
        }
        else if (currentSleep >= .8){
            statusDisplay.text += "Dormir demas es peor que no dormir\n";
        }
        else if (currentSleep >= 0.3)
        {
            statusDisplay.text += "Una dormidita no caeria mal\n";
        }
        else
        {
            statusDisplay.text += "Zzzz\n";
        }


    }

    IEnumerator CheckStatusDelay()
    {
        while (true)
        {
            CheckStatus();
            yield return new WaitForSeconds(1.5F);
        }
    }
}
