using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    public float timeStart = 60;
    public Text textBox;

	void Start () {
        textBox.text = timeStart.ToString();
	}
	
	void Update () {
        timeStart -= Time.deltaTime;
        textBox.text = Math.Round((decimal) timeStart, 1).ToString();
	}
}
