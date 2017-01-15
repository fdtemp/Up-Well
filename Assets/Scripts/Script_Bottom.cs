using UnityEngine;

class Script_Bottom : MonoBehaviour {
    private bool Stay = false;
    private bool Enter = false;
    private float LastTime = 0;
    public bool Collided {
        get {
            return Stay || Enter;
        }
    }
    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.name == "Cube(Clone)") {
            Enter = true;
            if (coll.gameObject.GetComponent<CubeObject>() != null
            && Time.time - LastTime > 1f) {
                GameManager.ScoreChange(100);
                LastTime = Time.time;
            }
        }
    }
    void OnTriggerStay2D(Collider2D coll) {
        if (coll.gameObject.name == "Cube(Clone)") {
            Stay = true;
        }
    }
    void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.name == "Cube(Clone)") {
            Enter = false;
        }
    }
    void LateUpdate() {
        Stay = false;
    }
}