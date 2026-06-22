using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MrExpert : MonoBehaviour
{
    [SerializeField] MindMaster mindMaster;

    List<ColorBall> wrong = new List<ColorBall>();
    List<BallAttempt> contains = new List<BallAttempt>();
    List<ColorBall> notTested = new List<ColorBall>();
    List<Ball> correct = new List<Ball>();

    void OnEnable()
    {
        if(btnStartGuess != null)
        {
            btnStartGuess.onClick.AddListener(StartGuess);
            btnStartGuess.onClick.AddListener(()=>SetButtonState(false));
        }
    }

    void OnDisable()
    {
        if(btnStartGuess != null)
        {
            btnStartGuess.onClick.RemoveAllListeners();
        }
    }

    IEnumerator GuessRoutine(float waitTime = 0)
    {
        mindMaster.StartGame();
        wrong.Clear();
        contains.Clear();
        notTested.Clear();
        correct.Clear();

        List<int> positionsToFound = new List<int> { 0, 1, 2, 3 };

        //Add all possibilities
        foreach (ColorBall colorBall in Enum.GetValues(typeof(ColorBall)))
        {
            notTested.Add(colorBall);
        }

        (int correct, int contains) guessResult = mindMaster.Guess(notTested[0], notTested[0], notTested[0], notTested[0]);
        yield return new WaitForSeconds(waitTime);
        //Step 1: Find a wrong color
        while (guessResult.contains > 0 || guessResult.correct > 0)
        {
            contains.Add(new BallAttempt(notTested[0]));
            notTested.RemoveAt(0);
            //All right colors were found
            if (contains.Count == 4) { break; }
            //Reroll guess
            guessResult = mindMaster.Guess(notTested[0], notTested[0], notTested[0], notTested[0]);
            yield return new WaitForSeconds(waitTime);
        }
        wrong.Add(notTested[0]);
        notTested.RemoveAt(0);
        ColorBall guess0;
        ColorBall guess1;
        ColorBall guess2;
        ColorBall guess3;

        // Step 2: Find the 4 right colors
        while (contains.Count + correct.Count < 4 && 4 - contains.Count - correct.Count != notTested.Count )
        {
            guess1 = wrong[0];
            guess0 = wrong[0];
            guess2 = wrong[0];
            guess3 = wrong[0];
            (ColorBall color, int pos) toTry1, toTry2;
            toTry1.pos = positionsToFound[0];
            toTry2.pos = positionsToFound.Count > 1? positionsToFound[1] : (positionsToFound[0] + 1) % 4;
            //Try the first able position
            {
                if (toTry1.pos == 0)
                {
                    guess0 = notTested[0];
                    toTry1.color = guess0;
                }
                else if (toTry1.pos == 1)
                {
                    guess1 = notTested[0];
                    toTry1.color = guess1;
                }
                else if (toTry1.pos == 2)
                {
                    guess2 = notTested[0];
                    toTry1.color = guess2;
                }
                else
                {
                    guess3 = notTested[0];
                    toTry1.color = guess3;
                }
            }
            // Try the second able position
            {
                if (toTry2.pos == 0)
                {
                    guess0 = notTested[1];
                    toTry2.color = guess0;
                }
                else if (toTry2.pos == 1)
                {
                    guess1 = notTested[1];
                    toTry2.color = guess1;
                }
                else if (toTry2.pos == 2)
                {
                    guess2 = notTested[1];
                    toTry2.color = guess2;
                }
                else
                {
                    guess3 = notTested[1];
                    toTry2.color = guess3;
                }
            }

            //Try to guess

            guessResult = mindMaster.Guess(guess0, guess1, guess2, guess3);
            yield return new WaitForSeconds(waitTime);

            //Check result
            {
                if (guessResult.contains > 0 || guessResult.correct > 0)
                {
                    //High roll, 2 correct
                    if (guessResult.correct == 2)
                    {
                        //Add ball 1 as correct
                        correct.Add(new Ball(toTry1.color, toTry1.pos));

                        //Add ball 2 as correct
                        correct.Add(new Ball(toTry2.color, toTry2.pos));

                        //Remove found positions
                        positionsToFound.Remove(toTry1.pos);
                        positionsToFound.Remove(toTry2.pos);
                    }
                    //Mid roll, 2 contains
                    else if (guessResult.contains == 2)
                    {
                        contains.Add(new BallAttempt(toTry1.color, toTry1.pos));
                        contains.Add(new BallAttempt(toTry2.color, toTry2.pos));
                    }
                    //OK roll, 1 correct
                    else if (guessResult.correct == 1)
                    {
                        //Make second guess wrong
                        if (toTry2.pos == 0)
                        {
                            guess0 = wrong[0];
                        }
                        else if (toTry2.pos == 1)
                        {
                            guess1 = wrong[0];
                        }
                        else if (toTry2.pos == 2)
                        {
                            guess2 = wrong[0];
                        }
                        else
                        {
                            guess3 = wrong[0];
                        }

                        //Better ok roll: 1 correct 1 contains
                        if (guessResult.contains > 0)
                        {
                            //Reroll with second guess as wrong
                            guessResult = mindMaster.Guess(guess0, guess1, guess2, guess3);
                            yield return new WaitForSeconds(waitTime);

                            //First was correct, second was contains
                            if (guessResult.correct > 0)
                            {
                                //Add correct ball based on the first try
                                correct.Add(new Ball(toTry1.color, toTry1.pos));

                                //Add second ball as contains
                                contains.Add(new BallAttempt(toTry2.color, toTry2.pos));

                                //Remove found position of ball 1
                                positionsToFound.Remove(toTry1.pos);
                            }
                            //Second on was right, first only contains
                            else
                            {
                                //Add correct ball based on the second try
                                correct.Add(new Ball(toTry2.color, toTry2.pos));

                                //Add first ball as contains
                                contains.Add(new BallAttempt(toTry1.color, toTry1.pos));

                                //Remove found position of ball 2
                                positionsToFound.Remove(toTry2.pos);
                            }
                        }
                        //Another ok roll: one correct, one wrong
                        else
                        {
                            //Reroll 
                            guessResult = mindMaster.Guess(guess0, guess1, guess2, guess3);
                            yield return new WaitForSeconds(waitTime);

                            //First right, second was wrong
                            if (guessResult.correct > 0)
                            {
                                correct.Add(new Ball(toTry1.color, toTry1.pos));
                                //Remove position of first try
                                positionsToFound.Remove(toTry1.pos);
                            }
                            //Second right, first wrong
                            else
                            {
                                correct.Add(new Ball(toTry2.color, toTry2.pos));
                                //Remove position of second try
                                positionsToFound.Remove(toTry2.pos);
                            }
                        }
                    }
                    //Not so good roll: one contains, one wrong
                    else
                    {
                        //Make first ball be in the position of the second one
                        if (toTry2.pos == 0)
                        {
                            guess0 = toTry1.color;
                        }
                        else if (toTry2.pos == 1)
                        {
                            guess1 = toTry1.color;
                        }
                        else if (toTry2.pos == 2)
                        {
                            guess2 = toTry1.color;
                        }
                        else
                        {
                            guess3 = toTry1.color;
                        }

                        //Make the first ball position wrong
                        if (toTry1.pos == 0)
                        {
                            guess0 = wrong[0];
                        }
                        else if (toTry1.pos == 1)
                        {
                            guess1 = wrong[0];
                        }
                        else if (toTry1.pos == 2)
                        {
                            guess2 = wrong[0];
                        }
                        else
                        {
                            guess3 = wrong[0];
                        }
                        //Reroll
                        guessResult = mindMaster.Guess(guess0, guess1, guess2, guess3);
                        yield return new WaitForSeconds(waitTime);

                        //Now it's a good roll: One correct in new position
                        if (guessResult.correct > 0)
                        {
                            correct.Add(new Ball(toTry1.color, toTry2.pos));
                            positionsToFound.Remove(toTry2.pos);
                        }
                        else
                        {
                            //Only information roll: No position found but 2 less possibilities 
                            //First contains
                            if (guessResult.contains > 0)
                            {
                                contains.Add(new BallAttempt(toTry1.color, new List<int> { toTry1.pos, toTry2.pos }));
                            } 
                            //Sevond contains
                            else
                            {
                                contains.Add(new BallAttempt(toTry2.color, toTry2.pos));
                            }
                        }

                    }
                }
                //Remove tested elements
                notTested.Remove(toTry1.color);
                notTested.Remove(toTry2.color);
            }
        }

        //If missing color, left balls are those colors
        if (contains.Count + correct.Count < 4)
        {
            foreach(ColorBall ball in notTested)
            {
                contains.Add(new BallAttempt(ball));
            }
        }

        //Final step: Find the right positions
        while (positionsToFound.Count > 1)
        {
            (ColorBall color, int pos) toTry1, toTry2;
            bool found = false;
            for (int i = 0; i < positionsToFound.Count - 1 && !found; i++)
            {
                //Skip if we already searched this position
                if(contains[0].triedPositions.Contains(positionsToFound[i])) { continue; }
                // If 3 where wrong, 4th is right
                if (contains[0].triedPositions.Count == 3)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (!contains[0].triedPositions.Contains(j))
                        {
                            correct.Add(new Ball(contains[0].color, j));
                            positionsToFound.Remove(j);
                            contains.RemoveAt(0);
                            break;
                        }
                    }
                }
                if (contains.Count < 2) { break; }
              
                //Search the current ball in current position
                toTry1.pos = positionsToFound[i];
                //Search next ball in next position
                toTry2.pos = positionsToFound[(i + 1) % positionsToFound.Count];

                guess0 = wrong[0];
                guess1 = wrong[0];
                guess2 = wrong[0];
                guess3 = wrong[0];

                //Try for the next one
                {
                    if (toTry1.pos == 0)
                    {
                        guess0 = contains[0].color;
                        toTry1.color = guess0;
                    }
                    else if (toTry1.pos == 1)
                    {
                        guess1 = contains[0].color;
                        toTry1.color = guess1;
                    }
                    else if (toTry1.pos == 2)
                    {
                        guess2 = contains[0].color;
                        toTry1.color = guess2;
                    }
                    else
                    {
                        guess3 = contains[0].color;
                        toTry1.color = guess3;
                    }
                }

                //Try the second next one 
                {
                    if (toTry2.pos == 0)
                    {
                        guess0 = contains[1].color;
                        toTry2.color = guess0;
                    }
                    else if (toTry2.pos == 1)
                    {
                        guess1 = contains[1].color;
                        toTry2.color = guess1;
                    }
                    else if (toTry2.pos == 2)
                    {
                        guess2 = contains[1].color;
                        toTry2.color = guess2;
                    }
                    else
                    {
                        guess3 = contains[1].color;
                        toTry2.color = guess3;
                    }
                }
                guessResult = mindMaster.Guess(guess0, guess1, guess2, guess3);
                yield return new WaitForSeconds(waitTime);
                
                //Reroll check
                {
                    if (guessResult.correct > 0)
                    {
                        //High roll: 2 correct
                        if (guessResult.correct == 2)
                        {
                            //Add first one as correct
                            correct.Add(new Ball(toTry1.color, toTry1.pos));

                            //Add second one as correct
                            correct.Add(new Ball(toTry2.color, toTry2.pos));

                            //Remove found positions
                            positionsToFound.Remove(toTry1.pos);
                            contains.RemoveAt(0);
                            positionsToFound.Remove(toTry2.pos);
                            contains.RemoveAt(0);
                            found = true;
                        }
                        //Good roll, one correct
                        else if (guessResult.correct == 1)
                        {
                            //Make second wrong
                            if (toTry2.pos == 0)
                            {
                                guess0 = wrong[0];
                            }
                            else if (toTry2.pos == 1)
                            {
                                guess1 = wrong[0];
                            }
                            else if (toTry2.pos == 2)
                            {
                                guess2 = wrong[0];
                            }
                            else
                            {
                                guess3 = wrong[0];
                            }

                            //Reroll 
                            guessResult = mindMaster.Guess(guess0, guess1, guess2, guess3);
                            yield return new WaitForSeconds(waitTime);

                            //First was right
                            if (guessResult.correct > 0)
                            {
                                //Check position as tried for ball 2
                                contains[1].triedPositions.Add(toTry2.pos);
                                //Add ball 1 as right
                                correct.Add(new Ball(toTry1.color, toTry1.pos));
                                //Remove found position
                                positionsToFound.Remove(toTry1.pos);
                                contains.RemoveAt(0);
                                found = true;
                            }
                            //Second was right
                            else
                            {

                                //Check position as tried for ball 2
                                contains[0].triedPositions.Add(toTry1.pos);
                                //Add ball 2 as right
                                correct.Add(new Ball(toTry2.color, toTry2.pos));
                                //Remove found position
                                positionsToFound.Remove(toTry2.pos);
                                contains.RemoveAt(1);
                            }
                        }
                    }
                }

            }
            
            //Not found on the first 3 positions, add it on the remaining one
            if (!found)
            {
                correct.Add(new Ball(contains[0].color, positionsToFound[positionsToFound.Count - 1]));
                contains.RemoveAt(0);
                positionsToFound.RemoveAt(positionsToFound.Count - 1);
            }
        }

        //If only 1 ball is missing add it to the missing position
        if (positionsToFound.Count == 1 && correct.Count < 4)
        {
            //Last position for last ball
            correct.Add(new(contains[0].color, positionsToFound[0]));
        }
    

        List<ColorBall> finalRoll = new List<ColorBall>();
        for (int i = 0; i < correct.Count; i++)
        {
            foreach (Ball ball in correct)
            {
                if(ball.correctPosition == i)
                {
                    finalRoll.Add(ball.colorBall);
                    break;
                }
            }
        }
        //Last roll, always right but not always in less than the max attempts
        guessResult = mindMaster.Guess(finalRoll[0], finalRoll[1], finalRoll[2], finalRoll[3]);
        yield return new WaitForSeconds(waitTime);
        SetButtonState(true);
    }

    public void StartGuess()
    {
        StartCoroutine(GuessRoutine(0.5f));
    }

#region UI Buttons
    [SerializeField] Button btnStartGuess;

    void SetButtonState(bool state)
    {
        btnStartGuess.interactable = state; 
    }

#endregion
}

#region Guess data
[Serializable]
public class Ball
{
    public ColorBall colorBall;
    public int correctPosition;
    public Ball(ColorBall colorBall, int correctPosition)
    {
        this.colorBall = colorBall;
        this.correctPosition = correctPosition;
    }
}

public class BallAttempt
{
    public ColorBall color;
    public List<int> triedPositions;

    public BallAttempt(ColorBall color)
    {
        this.color = color;
        triedPositions = new List<int>();
    }

    public BallAttempt(ColorBall color, int postition)
    {
        this.color = color;
        triedPositions = new List<int>();
        triedPositions.Add(postition);
    }

    public BallAttempt(ColorBall color, List<int> positions) {
        this.color = color;
        triedPositions = new List<int>(positions);
    }
}
#endregion