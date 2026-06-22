using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GuessEntry : MonoBehaviour
{
    [SerializeField] List<Image> guess;
    [SerializeField] List<Image> answers;

    static Color CORRECT_COLOR = new Color(1,0,0.96f);
    static Color CONTAIN_COLOR = new Color(0.44f,0.059f,0.97f);
    static Color WRONG_COLOR = new Color(0.13f,0,0.27f);



    public void SetResult(ColorBall _color1, ColorBall _color2, ColorBall _color3, ColorBall _color4)
    {
        guess[0].color = ColorBallToColor(_color1);
        guess[1].color = ColorBallToColor(_color2);
        guess[2].color = ColorBallToColor(_color3);
        guess[3].color = ColorBallToColor(_color4);
    }

    public void SetGuess(ColorBall _color1, ColorBall _color2, ColorBall _color3, ColorBall _color4, int totalCorrect, int totalContain)
    {
        SetResult(_color1,_color2,_color3,_color4);
        for(int i = 0; i<4; i++)
        {
            //Set guesses
            //Correct ones
            if (totalCorrect > 0)
            {
                totalCorrect--;
                answers[i].color = CORRECT_COLOR;
            }
            else if(totalContain  > 0)
            {
                totalContain--;
                answers[i].color = CONTAIN_COLOR;
            }
            //Wrong ones
            else
            {
                answers[i].color = WRONG_COLOR;
            }
        }
    }

    Color ColorBallToColor(ColorBall colorBall)
    {
        return colorBall switch
        {
            ColorBall.Red => Color.red,
            ColorBall.Green => Color.green,
            ColorBall.Blue => Color.blue,
            ColorBall.Yellow => Color.yellow,
            ColorBall.Brown => new Color(0.25f, 0.1f, 0),
            ColorBall.Orange => new Color(1, 0.6f, 0),
            ColorBall.Black => Color.black,
            ColorBall.White => Color.white,
            _ => throw new System.Exception("Colorball not registered"),
        };
    }
}