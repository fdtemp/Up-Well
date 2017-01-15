using UnityEngine;
using System.Collections;


/// <summary>
/// DK wt.
/// Deal the strength and damage recieving.
/// </summary>
public class CubeBody : MonoBehaviour 
{
    public float hp = 6f;
    private bool Touched = false; 
    private float TouchTime = 0;

    public void RecieveDmg(float dmg)
    {
        hp -= dmg;
    }

    void Update() {
        if (Touched) {
            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            Color col = sr.color;
            col.a = 1 - (Time.time - TouchTime) / 2;
            sr.color = col;
            gameObject.transform.Translate(0, -4f * Time.deltaTime, 0);
            if (Time.time - TouchTime > 2f)
                Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        if (hp <= 0.1f) {
            GameManager.ScoreChange(50);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.name == "DeadLine") {
            Touched = true;
            TouchTime = Time.time;
        }
    }

}
