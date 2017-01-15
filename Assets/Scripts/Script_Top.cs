using UnityEngine;

class Script_Top : MonoBehaviour {
    private bool Stay = false;
    private bool Enter = false;
    public bool Collided {
        get {
            return Stay || Enter;
        }
    }
    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.name == "Cube(Clone)" && coll.gameObject.GetComponent<CubeObject>() != null) {
            Enter = true;
        }
    }
    void OnTriggerStay2D(Collider2D coll) {
        if (coll.gameObject.name == "Cube(Clone)" && coll.gameObject.GetComponent<CubeObject>() != null) {
            Stay = true;
        }
    }
    void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.name == "Cube(Clone)" && coll.gameObject.GetComponent<CubeObject>() != null) {
            Enter = false;
        }
    }
    void LateUpdate() {
        Stay = false;
    }
}