using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eFSState
{
    idle,
    pre,
    active,
    post
}

public class FloatingScore : MonoBehaviour
{
    [Header("Set Dynamically")]
    public eFSState state = eFSState.idle;
    [SerializeField]
    protected int _score = 0;
    public string scoreString;

    public int score
    {
        get
        {
            return (_score);
        }
        set
        {
            _score = value;
            scoreString = _score.ToString("N0");
            GetComponent<Text>().text = scoreString;
        }
    }
    public List<Vector2> bezierPts; 
    public List<float> fontSizes; 
    public float timeStart = -1f;
    public float timeDuration = 1f;
    public string easingCurve = Easing.InOut; 
                                              
    public GameObject reportFinishTo = null;
    private RectTransform rectTrans;
    private Text txt;