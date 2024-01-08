using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongTweener : MonoBehaviour
{
    public TweenType type;
    public Vector3 from;
    public Vector3 to;
    public float time;

    private RectTransform rect;

    private void Start()
    {
        if (!gameObject.GetComponent<RectTransform>()) { rect = null; }

        rect = gameObject.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (rect == null)
        {
            LoopGameObject(gameObject);
        }
        else
        {
            LoopUI(rect);
        }
    }

    void LoopGameObject(GameObject g)
    {
        switch (type)
        {
            case TweenType.translate:
                g.transform.position = Vector3.Lerp(from, to, Mathf.PingPong(Time.time / time, 1));
                break;
            case TweenType.scale:
                g.transform.localScale = Vector3.Lerp(from, to, Mathf.PingPong(Time.time / time, 1));
                break;
            case TweenType.rotate:
                g.transform.localEulerAngles = Vector3.Lerp(from, to, Mathf.PingPong(Time.time / time, 1));;
                break;
        }
    }

    void LoopUI(RectTransform r)
    {
        switch (type)
        {
            case TweenType.translate:
                r.anchoredPosition = Vector2.Lerp(from, to, Mathf.PingPong(Time.time / time, 1));
                break;
            case TweenType.scale:
                r.localScale = Vector3.Lerp(from, to, Mathf.PingPong(Time.time / time, 1));
                break;
            case TweenType.rotate:
                r.localEulerAngles = Vector3.Lerp(from, to, Mathf.PingPong(Time.time / time, 1));;
                break;
        }
    }
}
