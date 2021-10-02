using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    public float timeStart = 60;
    public Text textBox;

	// Use this for initialization
	void Start () {
        textBox.text = timeStart.ToString();
	}
	
	// Update is called once per frame
	void Update () {
        timeStart -= Time.deltaTime;
        textBox.text = Mathf.Round(timeStart).ToString();
	}
    // bool active = false;
    // float currentTime;

    // public int startMinutes;
    // public Text currrentTimeText;

    // void Start()
    // {
    //     currentTime = startMinutes * 60;
    // }

    // void Update()
    // {
    //     if (active) {
    //         currentTime -= Time.deltaTime;
    //         if (currentTime <= 0) {
    //             Debug.Log("Time's Up!");
    //         }
    //     }
    //     TimeSpan time = TimeSpan.FromSeconds(currentTime);

    //     currrentTimeText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString();
    // }

    // public void StartTimer()
    // {
    //     active = true;
    // }

    // public void StopTimer()
    // {
    //     active = false;
    // }
}
