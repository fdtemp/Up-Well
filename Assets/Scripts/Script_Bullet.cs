using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Bullet : MonoBehaviour {
    public const float Damage = 1f;
    public const float Speed = 10f;
    public Vector3 Direction;
    private bool Shot = false;
    private float StartTime;
	void Start() {
        StartTime = Time.time;
	}

	void Update() {
        transform.Translate(Direction * Speed * Time.deltaTime);
        if (Shot || Time.time - StartTime > 5f) {
            AudioSource.PlayClipAtPoint(GameManager.Audio_Shot, Vector3.zero);
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.name == "Cube(Clone)") {
            Shot = true;
            coll.gameObject.GetComponent<CubeBody>().RecieveDmg(Damage);
        }
    }
}
