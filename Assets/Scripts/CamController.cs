using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public GameManager gm;
    public float targetToHeightRatio = 0.5f;
    [SerializeField]
    GameObject goalLine;
    Transform goalLineInstance;

    public float[] camSizes;
    int sizeIndex;

    public GameObject platform;
    float platformDistFromBtm;

    Camera cam;

    public float btmPos { get { return transform.position.y - cam.orthographicSize; } }
    float goalLineHeight { get { return btmPos + 2 * cam.orthographicSize * targetToHeightRatio; } }

    // Start is called before the first frame update
    void Awake()
    {
        cam = Camera.main;
        platformDistFromBtm = platform.transform.position.y - btmPos;
        goalLineInstance = Instantiate(goalLine, new Vector3(0, goalLineHeight, 0), Quaternion.identity).transform;
    }

    public void IncreaseCamSize() {
        if (sizeIndex + 1 >= camSizes.Length) return;
        cam.orthographicSize = camSizes[++sizeIndex];
        float dist2Btm = platform.transform.position.y - btmPos;
        cam.transform.position += new Vector3(0, dist2Btm - platformDistFromBtm, 0);
        Vector3 tpos = goalLineInstance.position;
        targetToHeightRatio *= 1.1f;
        gm.generateHeight2ScreenH *= 1.1f;
        goalLineInstance.position = new Vector3(tpos.x, goalLineHeight, 0);
        gm.UpdateLevelSettings();
    }

    public Vector3 CamRatioHeight(float ratio) {
        return new Vector3(0, btmPos + 2 * ratio * cam.orthographicSize, 0);
    }
}
