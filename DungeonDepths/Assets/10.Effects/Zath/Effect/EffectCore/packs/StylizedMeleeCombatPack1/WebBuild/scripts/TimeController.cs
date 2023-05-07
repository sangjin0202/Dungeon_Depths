using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour {


    public float difference = 0.01f;
	[SerializeField] float timeScale;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        if(Input.GetKey(KeyCode.A))
        {
			Time.timeScale -= difference;
			Time.timeScale = (Time.timeScale <= 0.01f) ? 0.01f: Time.timeScale;

		}
        if (Input.GetKey(KeyCode.S))
        {
            Time.timeScale += difference;
			Time.timeScale = (Time.timeScale > 2.01f) ? 0.01f : Time.timeScale;
		}
        if (Input.GetKey(KeyCode.D))
        {
            Time.timeScale = 1;
        }
		timeScale = Time.timeScale;

    }
}
