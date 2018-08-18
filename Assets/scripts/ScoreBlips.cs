using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreBlips : MonoBehaviour {

    Text blip;
    public float duration;
    Stopwatch timer;

    void Start()
    {
        timer = new Stopwatch();
        timer.Start();
    }

    void Update()
    {
        if (timer.ElapsedMilliseconds > duration * 1000) {
            Destroy(gameObject);
            return;
        }

        ////Color myColor = blip.color;
        //float ratio = Time.time / duration;
        //Color color = blip.color;
        //color.a = Mathf.Lerp(1, 0, ratio);
        //blip.color = color;

    }


}
