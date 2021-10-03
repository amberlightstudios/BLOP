using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    public float timeStart = 60;
    public Text textBox;

    bool active;

	void Start ()
    {
		textBox.text = timeStart.ToString();
	}
	
	void Update ()
    {
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
}
