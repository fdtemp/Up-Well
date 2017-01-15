using UnityEngine;

public static class GameManager {

    public const float DeadLineStartTime = 10f;
    public static float DeadLineSpeed = 0.1f;

    private static GameObject ScorePrefab;

    public static AudioClip
        Audio_Shot,
        Audio_Score,
        Audio_Jump,
        Audio_Shoot,
        Audio_Die;

    public static GameObject Player;
    public static GameObject GameRegion;
    public static CubeManager CubeManager;
    public static GameObject DeadLine;
    public static TextMesh ScorePoint;
    public static TextMesh HeightPoint;
    public static GameObject Canvas;
    public static UnityEngine.UI.Image CanvasImage;

    public static int MaxHeight;
    private static bool PlayerDead;
    private static float StartTime;
    private static float DeadTime;
    private static float LastDogTime;

    public static float scorePoint;
    public static int heightPoint;

    public static void Start() {
        ScorePrefab = Resources.Load<GameObject>("Score");
        Audio_Jump = Resources.Load("Jump1", typeof(AudioClip)) as AudioClip;
        Audio_Score = Resources.Load("Computer", typeof(AudioClip)) as AudioClip;
        Audio_Shoot = Resources.Load("Gun1", typeof(AudioClip)) as AudioClip;
        Audio_Shot = Resources.Load("Cancel1", typeof(AudioClip)) as AudioClip;
        Audio_Die = Resources.Load("Wolf", typeof(AudioClip)) as AudioClip;

        Player = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Player"));
        GameRegion = GameObject.Find("GameRegion");
        CubeManager = GameRegion.GetComponent<CubeManager>();
        DeadLine = GameObject.Find("DeadLine");
        ScorePoint = GameObject.Find("SCOREPOINT").GetComponent<TextMesh>();
        HeightPoint = GameObject.Find("HEIGHTPOINT").GetComponent<TextMesh>();
        Canvas = GameObject.Find("Canvas");
        CanvasImage = GameObject.Find("Image").GetComponent<UnityEngine.UI.Image>();
        Canvas.SetActive(false);
        Player.transform.position = new Vector3(-6f, -8.5f);
        StartTime = Time.time;
        PlayerDead = false;
        MaxHeight = 20;
        DeadLineSpeed = 0.1f;
        scorePoint = 0;
        heightPoint = 0;
    }
    public static void Update() {

        if (PlayerDead) {
            CanvasImage.color = new Color(255, 0, 0, 0.75f * Mathf.Lerp(0, 1, (Time.time - DeadTime) / 3f));
            //if (Random.Range(0, 30) == 0 && Time.time - LastDogTime > 1f) {
            //    AudioSource.PlayClipAtPoint(Audio_Die, Vector3.zero);
            //    LastDogTime = Time.time;
            //}
        } else {
            if (Time.time - StartTime > DeadLineStartTime)
                DeadLine.transform.Translate(0, DeadLineSpeed * Time.deltaTime, 0);

            Vector3
                pp = Player.gameObject.transform.position,
                grp = GameRegion.transform.position;

            heightPoint = Mathf.FloorToInt(pp.y - grp.y);
            scorePoint += Time.deltaTime * (1 + heightPoint / 20);

            ScorePoint.text = i2s(Mathf.FloorToInt(scorePoint));
            HeightPoint.text = i2s(heightPoint);

            float delta = -pp.y;
            if (pp.y > 0) {
                pp.y = 0;
                grp.y += delta;
            } else if (grp.y < -8.5) {
                delta = Mathf.Min(-pp.y, -grp.y - 9);
                pp.y += delta;
                grp.y += delta;
            } else {
                CubeManager.generatingHeight = MaxHeight + 4;
                return;
            }
            Player.transform.position = pp;
            GameRegion.transform.position = grp;
            CubeManager.generatingHeight = Mathf.Max(MaxHeight + 4, Mathf.Floor(pp.y + 11f - grp.y));
            if (DeadLine.transform.localPosition.y < pp.y - 12)
                DeadLine.transform.Translate(0, (pp.y - 12 - DeadLine.transform.localPosition.y) / 10 * Time.deltaTime, 0);
        }
    }
    public static string i2s(int x) {
        string a = x.ToString();
        string b = "";
        for (int i = 0; i < 6 - a.Length; i++)
            b += "0";
        return b + a;
    }
    public static void PlayerDie() {
        AudioSource.PlayClipAtPoint(Audio_Die, Vector3.zero, 2);
        PlayerDead = true;
        Canvas.SetActive(true);
        DeadTime = Time.time;
    }
    public static void ScoreChange(int delta) {
        if (Player == null) return;
        AudioSource.PlayClipAtPoint(Audio_Score, Vector3.zero);
        scorePoint += delta;
        GameObject go = GameObject.Instantiate<GameObject>(ScorePrefab);
        go.transform.position = Player.transform.position + new Vector3(0,1.5f + Random.Range(-0.25f,0.25f),0);
        go.GetComponent<TextMesh>().text = (delta < 0) ? delta.ToString() : '+' + delta.ToString();
    }
}