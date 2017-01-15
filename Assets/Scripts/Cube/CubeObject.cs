using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// DK wt.
/// Deal the movement of a cube.
/// Will delete itself after collide to an existing static cube.
/// </summary>
public class CubeObject : MonoBehaviour 
{
    public float velocity = 0f;
    public bool isDestroyed = false;
    public LinkedList<CubeObject> adjacent = new LinkedList<CubeObject>();

	void Start() 
	{
	
	}
	
	void Update() 
	{
        this.gameObject.transform.Translate(velocity*Time.deltaTime*Vector2.down);
	}
	
	void LateUpdate()
	{
        if (isDestroyed) Destroy(this);
	}

    // Assume that there'll be nothing when a moving cube strikes another moving.
    void OnCollisionEnter2D(Collision2D x)
    {
        
        CubeBody b = x.gameObject.GetComponent<CubeBody>();
        if (b == null) return; // do nothing if touched player and border.

        CubeObject e = x.gameObject.GetComponent<CubeObject>();
        if (e != null) return; // do nothing with other moving objects.

        Debug.Log("Cube Collide!");

        PreDestroy();
    }

    public void PreDestroy()
    {
        isDestroyed = true;
        Vector3 p = transform.localPosition;
        p.y = Mathf.Floor(p.y) + 0.5f;
        transform.localPosition = p;
        foreach (CubeObject x in adjacent)
            if (x != null) // adjacent cube object not destroyed.
                if (!x.isDestroyed) {
                    GameManager.MaxHeight =
                        Mathf.Max(GameManager.MaxHeight, Mathf.FloorToInt(x.transform.localPosition.y));
                    p = x.transform.localPosition;
                    p.y = Mathf.Floor(p.y) + 0.5f;
                    x.transform.localPosition = p;
                    x.PreDestroy();
                }
    }
}
