using UnityEngine; 
using System.Collections.Generic;
using System;
using TMPro;

public class MindMaster : MonoBehaviour
{
    private void Start()
    {
        StartGame();
    }

    public List<ColorBall> Result = new List<ColorBall>(4);
    int turn = 0;
    [SerializeField] int maxTurns = 10;
    public int CurrentTurn => turn;

    public void StartGame()
    {
        ClearAttempts();
        lblResult.text = string.Empty;
        Result.Clear();
        turn = 0;
        List<ColorBall> types = new List<ColorBall>();

        foreach (ColorBall colorBall in Enum.GetValues(typeof(ColorBall))) 
        {
            types.Add(colorBall);
        }
        int index = 0;

        for (int i = 0; i < 4; i++)
        {
            index = UnityEngine.Random.Range(0, types.Count);
            Result.Add(types[index]);
            types.RemoveAt(index);
        }
        ShowResult(Result[0],Result[1],Result[2],Result[3]);
    }


    public (int correct, int contains) Guess(ColorBall _color1, ColorBall _color2, ColorBall _color3, ColorBall _color4)
    {
        int _correct = 0;
        int _contains = 0;

        List<ColorBall> _guess = new List<ColorBall> { _color1, _color2, _color3, _color4};

        for(int i = 0; i < 4; i++)
        {
            if (Result[i] == _guess[i])
            {
                _correct++;
                continue;
            }
            if (_guess.Contains(Result[i]))
            {
                _contains++;
            }
        }
        ShowAttempt(turn,_color1,_color2,_color3,_color4,_correct,_contains);
        turn++;
        
        if(turn > maxTurns)
        {
            lblResult.text = "Failure";
        }
        else if(_correct == 4)
        {
            lblResult.text = "Solved";
        }
        return (_correct, _contains);
    }

#region Game visualization
    [SerializeField] GuessEntry result;
    [SerializeField] List<GuessEntry> entries;
    [SerializeField] TextMeshProUGUI lblResult;
    
    private void ShowResult(ColorBall _color1, ColorBall _color2, ColorBall _color3, ColorBall _color4)
    {
        result.SetResult(_color1,_color2,_color3,_color4);
    }

    void ShowAttempt(int turn, ColorBall _color1, ColorBall _color2, ColorBall _color3, ColorBall _color4, int correct, int contains)
    {
        if(turn>=entries.Count){return;}
        entries[turn].SetGuess(_color1,_color2,_color3,_color4,correct, contains);
        entries[turn].gameObject.SetActive(true);
        
    }
    void ClearAttempts()
    {
        foreach(GuessEntry g in entries)
        {
            g.gameObject.SetActive(false);
        }
    }
#endregion
}



public enum ColorBall
{
    Red,
    Green,
    Blue,
    Yellow,
    Brown,
    Orange,
    Black,
    White
}