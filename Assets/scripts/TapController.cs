/* TapController.cs
 * Author: Jack Bruce plus Youtube guy
 * Date Created: Initial
 * Date Last Edited: 8/15/18
 * Description: Manages jumping from taps or keys. Handles scoring w/ help from
 * BeatMaker.cs
 * Things I don't know about this script:
 * - Quaternion and Euler values
 * - Lerp
*/

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)) ]
public class TapController : MonoBehaviour {

    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;
    public static event PlayerDelegate OnPlayerJumps;

    public float tapForce = 10;
    public float minTapForce = 2;
    public float maxTapForce = 10;

    public float tiltSmooth = 5;
    public Vector3 startPos;

    public AudioSource tapAudio;
    public AudioSource scoreAudio;
    public AudioSource dieAuidio;
    new Rigidbody2D rigidbody;
    Quaternion downRotation;
    Quaternion forwardRotation;

    Stopwatch timer = new Stopwatch();

    GameManager game;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        startPos = rigidbody.transform.position;
        downRotation = Quaternion.Euler(0, 0, -60);
        forwardRotation = Quaternion.Euler(0,0,35);
        game = GameManager.Instance;
        rigidbody.simulated = false;

    }

    void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
        BeatMaker.OnUpBeat += OnUpBeat;
        BeatMaker.OnDownBeat += OnDownBeat;

    }

    void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
        BeatMaker.OnUpBeat -= OnUpBeat;
        BeatMaker.OnDownBeat -= OnDownBeat;
    } 

    void OnGameStarted()
    {
        timer.Start();
        rigidbody.velocity = Vector3.zero;
        rigidbody.simulated = true;
    }

    void OnGameOverConfirmed()
    {
        timer.Reset();
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }

    void OnUpBeat () { // tapForce should be max
        tapForce = maxTapForce;
    }

    void OnDownBeat() { // tapForce should be min
        tapForce = minTapForce;
    }


    void Update()
    {
        if (game.GameOver) return;
        UpdateTapForce();
        if (Input.GetMouseButtonDown(0) || Input.anyKeyDown) //player taps
        {
            Jump();
            OnPlayerJumps(); //event sent to ScoreManager
        }
        /* Lerp(src value, target value, time)
         * value changes from src to target over a certain time period.
         * we want the bird to rotate smoothly
         */ 
        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation,tiltSmooth * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.gameObject.tag == "ScoreZone") 
        {
            //register a score event
            OnPlayerScored(); //event sent to GameManager
            //play a sound
            scoreAudio.Play();
        }

        if (col.gameObject.tag == "DeadZone") 
        {
            rigidbody.simulated = false; //freezes bird location
            //register a dead event
            OnPlayerDied(); //event sent to GameManager
            //play a sound
            dieAuidio.Play();
        }
    }

    private void Jump() {
        tapAudio.Play();
        transform.rotation = forwardRotation; //rotate bird up
        rigidbody.velocity = Vector3.zero;


        rigidbody.AddForce(Vector2.up * tapForce, ForceMode2D.Force); //push bird up
    }

    private void UpdateTapForce() {
        float bv = BeatMaker.GetBeatValue(); //sin function w/ time
        float amplitude = (maxTapForce - minTapForce) / 2;
        tapForce = amplitude * bv + amplitude + minTapForce;
    }

}
