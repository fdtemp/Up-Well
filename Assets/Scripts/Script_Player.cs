using System;
using UnityEngine;

public class Script_Player : MonoBehaviour {

    public const float
        x = -10 * (4/3f),
        y = -10f,
        xlen = 20 * (4/3f),
        ylen = 20f;

    public const float MoveSpeed = 3.5f;
    public const float JumpSpeed = 12.5f;
    public const float JumpSpeed_AfterShoot = JumpSpeed * 0.7f;
    public const float ShootInterval = 0.125f;
    public const float ShootDamage = 1f;

    private bool TurnRight = true;
    private float LastShootTime = -ShootInterval;

    public GameObject BulletPrefab;
    private GameObject Entity;
    private Script_Top TriggerTop;
    private Script_Bottom TriggerBottom;
    private Rigidbody2D rb2d;

    private Vector3 GetMousePosition() {
        return new Vector3(
            x + (Input.mousePosition.x / Screen.width) * xlen,
            y + (Input.mousePosition.y / Screen.height) * ylen
        );
    }

    void Start () {
        Entity = gameObject;
        TriggerTop = transform.Find("Top").GetComponent<Script_Top>();
        TriggerBottom = transform.Find("Bottom").GetComponent<Script_Bottom>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        BulletPrefab = Resources.Load<GameObject>("Bullet");
    }

    void Update() {

        if (TriggerTop.Collided) {
            GameManager.PlayerDie();
            Destroy(gameObject);
        }

        //Moving
        int Horizontal = 0, Vertical = 0;
        if (Input.GetKey(KeyCode.A)) Horizontal -= 1;
        if (Input.GetKey(KeyCode.D)) Horizontal += 1;
        if (Input.GetKey(KeyCode.W) && TriggerBottom.Collided) {
            AudioSource.PlayClipAtPoint(GameManager.Audio_Jump, Vector3.zero);
            Vertical = 1;
        }

        if ((Horizontal > 0 && !TurnRight) || (Horizontal < 0 && TurnRight)) {
            Vector3 s = Entity.transform.localScale;
            s.x = -s.x;
            Entity.transform.localScale = s;
            TurnRight = !TurnRight;
        }

        Vector3 v = rb2d.velocity;
        v.x = Horizontal * MoveSpeed;
        if (Vertical != 0) v.y = Vertical * ((Time.time - LastShootTime > ShootInterval) ? JumpSpeed : JumpSpeed_AfterShoot);
        rb2d.velocity = v;

        //Shooting
        if (Time.time - LastShootTime > ShootInterval && Input.GetMouseButton(0)) {
            Vector3 pos = GetMousePosition();
            AudioSource.PlayClipAtPoint(GameManager.Audio_Shoot, Vector3.zero);

            GameObject bullet = GameObject.Instantiate<GameObject>(BulletPrefab);
            Script_Bullet script = bullet.GetComponent<Script_Bullet>();

            bullet.transform.position = transform.position;
            script.Direction = (pos - transform.position).normalized;

            LastShootTime = Time.time;
        }
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.name == "DeadLine") {
            GameManager.PlayerDie();
            Destroy(gameObject);
        }
    }
}
