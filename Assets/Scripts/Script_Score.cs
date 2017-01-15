using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Score : MonoBehaviour {
    private const float StayTime = 2f;
    private float StartTime;
	void Start() {
        StartTime = Time.time;
	}

	void Update() {
        if (Time.time - StartTime > StayTime)
            Destroy(gameObject);
	}
}
