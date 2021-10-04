using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    public float timeStart = 60;
    public Text textBox;

    public bool active { get; set; }

	void Start ()
    {
		textBox.text = timeStart.ToString();
	}
	
	void Update ()
    {

        if (timeStart <= 0 && active) {
            StopTimer();
            // TODO: End level (Time's Up! Final Score) need a gm instance
            AudioSource BGM = Camera.main.GetComponent<AudioSource>();
            BGM.Pause();
            FindObjectOfType<AudioManager>().Play("GameOver");
        }
        if (!active) return;
        timeStart -= Time.deltaTime;
        textBox.text = Math.Round((decimal) timeStart, 1).ToString();
	}

    public void StartTimer()
    {
        active = true;
    }

    public void StopTimer()
    {
        active = false;
    }

    public void AddTime(int seconds)
    {
        timeStart += seconds;
    }
}
