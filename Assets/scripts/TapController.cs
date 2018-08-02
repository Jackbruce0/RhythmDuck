/* TapController.cs
 * Things I don't know about this script:
 * - Quaternion and Euler values
 * - Lerp
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)) ]
public class TapController : MonoBehaviour {

    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    public float tapForce = 10;
    public float tiltSmooth = 5;
    public Vector3 startPos;

    public AudioSource tapAudio;
    public AudioSource scoreAudio;
    public AudioSource dieAuidio;

    Rigidbody2D rigidbody;
    Quaternion downRotation;
    Quaternion forwardRotation;

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
    }

    void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    } 

    void OnGameStarted()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.simulated = true;
    }

    void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }


    void Update()
    {
        if (game.GameOver) return;
        if (Input.GetMouseButtonDown(0)) //player taps
        {
            tapAudio.Play();
            transform.rotation = forwardRotation; //rotate bird up
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(Vector2.up * tapForce, ForceMode2D.Force); //push bird up
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

}
