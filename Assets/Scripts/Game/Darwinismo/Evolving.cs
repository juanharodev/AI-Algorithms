using System;
using UnityEngine;
using UnityEngine.UI;

public class Evolving : MonoBehaviour
{

    [Header("Body")]
    public Image head;
    public Image body;
    public Image lArm;
    public Image rArm;
    public Image lLeg;
    public Image rLeg;

    public SpecimenValues values;

    public int totalSquares;
    public int totalTriangles;
    public int totalCirlces;
    public int totalHexagon;

    private void Awake()
    {
        values = new SpecimenValues();
    }

    public void Change(int _head, int _body, int _lArm, int _rArm, int _lLeg, int _rLeg)
    {
        
        totalSquares = 0;
        totalTriangles = 0;
        totalCirlces = 0;
        totalHexagon = 0;

        SetPart(head, _head);
        SetPart(body, _body);
        SetPart(lArm, _lArm);
        SetPart(rArm, _rArm);
        SetPart(lLeg, _lLeg);
        SetPart(rLeg, _rLeg);
        values.SetValues(_head,_body,_lArm,_rArm,_lLeg,_rLeg);


        if (values.headValue == values.bodyValue && values.bodyValue == values.lArmValue && values.lArmValue == values.rArmValue && values.rArmValue == values.lLegValue && values.lLegValue == values.rLegValue)
        {
            DarwinSimulator.Instance.PerfectFound(gameObject);
        }

        DarwinSimulator.Instance.ApplyForParent(this);
    }

    public void Clear()
    {
        head.sprite = null;
        body.sprite = null;
        lArm.sprite = null;
        rArm.sprite = null;
        lLeg.sprite = null;
        rLeg.sprite = null;
    }

    void SetPart(Image part, int value)
    {
        switch (value)
        {
            case 0: //Square
                part.sprite = DarwinSimulator.Instance.square;
                part.color = Color.red;
                totalSquares++;
                break;
            case 1: //Triangle
                part.sprite = DarwinSimulator.Instance.triangle;
                part.color = Color.yellow;
                totalTriangles++;
                break;
            case 2: //Circle
                part.sprite = DarwinSimulator.Instance.circle;
                part.color = Color.blue;
                totalCirlces++;
                break;
            case 3: //Hexagon
                part.sprite= DarwinSimulator.Instance.hexagon;
                part.color = Color.green;
                totalHexagon++;
                break;
            default:
                
                break;
        }
    }
}

[Serializable]
public class SpecimenValues
{
    public SpecimenValues() { }


    public SpecimenValues(int headValue, int bodyValue, int lArmValue, int rArmValue, int lLegValue, int rLegValue)
    {
        SetValues(headValue, bodyValue, lArmValue,rArmValue, lLegValue, rLegValue);
    }    
    public void SetValues(int headValue, int bodyValue, int lArmValue, int rArmValue, int lLegValue, int rLegValue)
    {
        this.headValue = headValue;
        this.bodyValue = bodyValue;
        this.lArmValue = lArmValue;
        this.rArmValue = rArmValue;
        this.lLegValue = lLegValue;
        this.rLegValue = rLegValue;
    }

    public int headValue;
    public int bodyValue;
    public int lArmValue;
    public int rArmValue;
    public int lLegValue;
    public int rLegValue;
}
