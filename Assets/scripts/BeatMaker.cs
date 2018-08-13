/* BeatMaker.cs
 * Author: Jack Bruce
 * Date Created: 8/12/18
 * Date Last Edited: 8/12/18
 * Description: Makes beats and keeps track of them to use for Jump Force control.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatMaker : MonoBehaviour {

    public delegate void Beat();
    public static event Beat OnUpBeat;
    public static event Beat OnDownBeat;

    // 1 = downbeat, 2 = upbeat
    List<int> whichBeat = new List<int>() { 1, 2 };
    int beatMark = 0;

    bool timerReset = true;
    public float tempo; // in bpm

    public AudioSource downBeat;
    public AudioSource upBeat;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        if (timerReset && beatMark < whichBeat.Count) {
            StartCoroutine(spawnBeat());
            timerReset = false;
        }
	}

    IEnumerator spawnBeat() {
        yield return new WaitForSeconds(60/tempo); //tempo is not accurate


        //if downBeat
        if (whichBeat[beatMark] == 1) {
            //spawn downBeat object ?
            OnDownBeat(); // to TapController
            downBeat.Play();
            beatMark++;
        } else if (whichBeat[beatMark] == 2) { // if upBeat
            //spawn upBeat object
            OnUpBeat(); // to TapController
            upBeat.Play();
            beatMark--;
        }
        timerReset = true;
    }

}
