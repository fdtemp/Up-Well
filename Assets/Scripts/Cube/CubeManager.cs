using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// dk wt.
/// Use this to manage block creation.
/// Mount on something global, such as camera.
/// </summary>
public class CubeManager : MonoBehaviour {

    public const int RegionWidth = 10;
    public const int RegionHeight = 500;

    float t = 0f; // time counter creating blocks.
    public float Interval = 3f; // delay between placing 2 group of blocks.
    public int num = 1; // number of blocks every group place.

    public float velocity = 2f;

    float st = 0f; // time counter of step.
    public float velocityStep = 0.35f; // how much velocity should be added per difficulty step.
    public float IntervalStep = 0.22f;
    public float DeadLineSpeedStep = 0.06f;
    public int numStep = 0; // how many group should be added per difficulty step.
    public float stepTime = 15f; // how many seconds will num add by numStep.

    public GameObject sourceCube;
    public GameObject GameRegion;
    public float sourceHeight = 1f;
    public float sourceWidth = 1f;
    public float generatingHeight = 20f;
    public float borderLeft = 0;
    public int column = 10;

    public int[] blockWidth = {
        4,
        3, 3,
        2,
        2, 2, 2, 2,
        3, 3,
        3, 3, 3, 3 };
    public int[] blockHeight = {
        1,
        2, 2,
        2,
        2, 2, 2, 2,
        2, 2,
        2, 2, 2, 2 };
    public string[] blocks = {
        "####",
        ".#.###", "###.#.",
        "####",
        ".###", "#.##", "##.#", "###.",
        "#.####", "####.#",
        "#..###", "..####", "####..", "###..#" };

    // index: which block should I create.
    // loc: left top cube location (Maybe empty).
    public void GenerateBlock(int index, Vector2 loc)
    {
        //Debug.Log(index);
        int w = blockWidth[index];
        int h = blockHeight[index];
        string shape = blocks[index];

        CubeObject[] cubes = new CubeObject[w * h];

        for (int i = 0; i < h; i++)
            for (int j = 0; j < w; j++)
                if (shape[i * w + j] == '#')
                {
                    int idx = i * w + j;

                    // single cube initialization.
                    cubes[idx] = Instantiate(sourceCube).GetComponent<CubeObject>();
                    cubes[idx].velocity = velocity;
                    cubes[idx].transform.parent = GameRegion.transform;
                    cubes[idx].transform.localPosition =
                        loc +
                        Vector2.down * sourceHeight * (i+0.5f) +
                        Vector2.right * sourceWidth * (j+0.5f);

                    // add adjacents.
                    if (idx >= w && shape[idx - w] == '#')
                    {
                        cubes[idx].adjacent.AddLast(cubes[idx - w]);
                        cubes[idx - w].adjacent.AddLast(cubes[idx]);
                    }
                    if (idx % w > 0 && shape[idx - 1] == '#')
                    {
                        cubes[idx].adjacent.AddLast(cubes[idx - 1]);
                        cubes[idx - 1].adjacent.AddLast(cubes[idx]);
                    }
                }
    }

    void Awake()
    {

    }

    void Start()
    {
        GameObject w;
        sourceCube = Resources.Load<GameObject>("Cube");
        GameRegion = GameObject.Find("GameRegion");
        for (int i = 0; i < 20; i++) {
            w = GameObject.Instantiate<GameObject>(sourceCube);
            w.transform.localPosition = new Vector3(-RegionWidth-1.5f, -9.5f + i);
            w.GetComponent<CubeBody>().hp = 999999;
            w.GetComponent<CubeObject>().enabled = false;
            w = GameObject.Instantiate<GameObject>(sourceCube);
            w.transform.localPosition = new Vector3(-0.5f, -9.5f + i);
            w.GetComponent<CubeBody>().hp = 999999;
            w.GetComponent<CubeObject>().enabled = false;
        }
        for (int i = 0; i < 10; i++) {
            w = GameObject.Instantiate<GameObject>(sourceCube);
            w.transform.parent = GameRegion.transform;
            w.transform.localPosition = new Vector3(i + 0.5f, -0.5f);
            w.GetComponent<CubeBody>().hp = 999999;
            Destroy(w.GetComponent<CubeObject>());
            w = GameObject.Instantiate<GameObject>(sourceCube);
            w.transform.parent = GameRegion.transform;
            w.transform.localPosition = new Vector3(i + 0.5f, RegionHeight + 0.5f);
            w.GetComponent<CubeBody>().hp = 999999;
            Destroy(w.GetComponent<CubeObject>());
        }
    }

    void Update()
    {
        t += Time.deltaTime;
        if (t > Interval) // generate a block.
        {
            t -= Interval;
            for (int i = 0; i < num; i++)
            {
                Vector2 loc = generatingHeight * Vector2.up;
                int bid = (int)Mathf.Floor(Random.Range(0f, blocks.Length));
                GenerateBlock(bid,
                    new Vector2(
                        Mathf.Floor(Random.Range(0f, column - blockWidth[bid] + 1)) * sourceWidth + borderLeft,
                        generatingHeight
                    ));
            }
        }

        st += Time.deltaTime;
        if (st > stepTime && Interval > 0.6f)
        {
            st -= stepTime;
            Interval -= IntervalStep;
            GameManager.DeadLineSpeed += DeadLineSpeedStep;
            velocity += velocityStep;
            num += numStep;
        }
    }

    void LateUpdate()
    {

    }


}



