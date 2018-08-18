/* ScoreManager.cs
 * Author: Jack Bruce
 * Date Created: 8/15/18
 * Date Last Edited: 8/15/18
 * Description: Calculates scores based on rhythm
 * Scores valued on a spectrum from perfect upbeat to nothing (perfect downbeat)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public delegate void ScoreDelegate();
    public static event ScoreDelegate OnRhythmScore;

    private float perfectBeatDelta = .2f; // bigger the number = easier to score perfects
    private int perfectUpBeatBonus = 25;
    private float scoreMultiplier = 15;
    private static int jumpScore;
    private static string jumpScoreString;

    void OnEnable()
    {
        TapController.OnPlayerJumps += OnPlayerJumps;

    }

    void OnDisable()
    {
        TapController.OnPlayerJumps -= OnPlayerJumps;
    }

    void OnPlayerJumps() {
        jumpScore = 0;
        float bv = BeatMaker.GetBeatValue();
        jumpScore = (int)(bv * scoreMultiplier);
        if (jumpScore > 0) jumpScoreString = "+" + jumpScore.ToString();
        else jumpScoreString = jumpScore.ToString();
        if (IsPerfectUpBeat()) {
            jumpScore += perfectUpBeatBonus;
            //display perfect upbeat message + jumpScore
            jumpScoreString = "Perfect Upbeat! " + jumpScoreString;
        }

        OnRhythmScore(); // sent to game manager

    }

    private bool IsPerfectUpBeat() {
        float bv = BeatMaker.GetBeatValue();
        if (bv > 1 - perfectBeatDelta) return true;
        return false;
    }

    public static int GetJumpScore() {
        return jumpScore;
    }

    public static string GetJumpScoreString() {
        return jumpScoreString;
    }

}
