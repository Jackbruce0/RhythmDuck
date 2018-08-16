/* BeatMaker.cs
 * Author: Jack Bruce
 * Date Created: 8/12/18
 * Date Last Edited: 8/15/18
 * Description: Makes beats and keeps track of them to use for Jump Force control.
 */

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BeatMaker : MonoBehaviour {

    public delegate void Beat();
    public static event Beat OnUpBeat;
    public static event Beat OnDownBeat;

    // 1 = downbeat, 2 = upbeat
    List<int> whichBeat = new List<int>() { 1, 2 };
    int beatMark = 0;

    bool timerReset = true;
    //public float tempo; // in bpm
    private static float beatValue = 0;
    public float beatDelta = .0001f;
    bool lastUpBeat = true;

    public AudioSource downBeat;
    public AudioSource upBeat;

	// Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        beatValue = Mathf.Sin(Time.time * 5); //+ 1; //0 <= beatValue <= 1
        BeatCheck(beatValue);


        //if (timerReset && beatMark < whichBeat.Count)
        //{
        //    StartCoroutine(SpawnBeat());
        //    timerReset = false;
        //}
	}

    //IEnumerator SpawnBeat() {
    //    yield return new WaitForSeconds(60/tempo); //tempo is not accurate


    //    //if downBeat
    //    if (whichBeat[beatMark] == 1) {
    //        //spawn downBeat object ?
    //        OnDownBeat(); // to TapController
    //        downBeat.Play();
    //        beatMark++;

    //    } else if (whichBeat[beatMark] == 2) { // if upBeat
    //        //spawn upBeat object
    //        OnUpBeat(); // to TapController
    //        upBeat.Play();
    //        beatMark--;

    //    }
    //    timerReset = true;
    //}

    void BeatCheck(float bv) {
        if (bv >= 1 - beatDelta && !lastUpBeat) {
            OnUpBeat(); // to TapController
            upBeat.Play();
            lastUpBeat = true;
        }

            if (bv <= -1 + beatDelta && lastUpBeat) {
            OnDownBeat(); // to TapController
            downBeat.Play();
            lastUpBeat = false;
        }
    }

    public static float GetBeatValue() { // used in TapController for TapForce and Scoring
        return beatValue;
    }
    

}
