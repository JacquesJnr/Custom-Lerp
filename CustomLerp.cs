using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public enum TweenType
{
    translate,
    scale,
    rotate
}

public class CustomLerp : MonoBehaviour
{
    public static CustomLerp Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    private void AddLooper(GameObject g, TweenType type, Vector3 from, Vector3 to, float time)
    {
        PingPongTweener loopInstance =  g.AddComponent<PingPongTweener>();

        loopInstance.type = type;
        loopInstance.from = from;
        loopInstance.to = to;
        loopInstance.time = time;
    }

    #region Translate

    public void Move(GameObject g, Vector3 to, float time, bool loop)
    {
        if (loop)
        {
            AddLooper(g, TweenType.translate, g.transform.position, to, time);
            return;
        }
        
        StartCoroutine(Tween(g, TweenType.translate, to, time));
    }
    private void LerpPosition(GameObject g, Vector3 to, float time)
    {
        g.transform.position = Vector3.Lerp(g.transform.position, to, time);
    }
    
    public void UI_Move(RectTransform rect, Vector3 to, float time, bool loop)
    {
        if (loop)
        {
            AddLooper(rect.gameObject, TweenType.translate, rect.anchoredPosition, to, time);
            return;
        }
        
        StartCoroutine(TweenUI(rect, TweenType.translate, to, time));
    }

    private void LerpAnchoredPosition(RectTransform rect, Vector3 to, float time)
    {
        rect.anchoredPosition = Vector3.Lerp(rect.anchoredPosition, to, time);
    }
    
    #endregion

    #region Scale

    public void Scale(GameObject g, Vector3 to, float time, bool loop)
    {
        if (loop)
        {
            AddLooper(g, TweenType.scale, g.transform.localScale, to, time);
            return;
        }
        
        StartCoroutine(Tween(g, TweenType.scale, to, time));
    }
    
    private void LerpLocalScale(GameObject g, Vector3 to, float time)
    {
        // Check if UI
        if (g.GetComponent<RectTransform>() != null)
        {
            RectTransform rect = g.GetComponent<RectTransform>();
            rect.localScale = Vector3.Lerp(rect.localScale, to, time);
            return;
        }
        
       g.transform.localScale = Vector3.Lerp(g.transform.localScale, to, time);
    }

    public void UI_Scale(RectTransform rect, Vector3 to, float time, bool loop)
    {
        if (loop)
        {
            AddLooper(rect.gameObject, TweenType.scale, rect.localScale, to, time);
            return;
        }

        StartCoroutine(TweenUI(rect, TweenType.scale, to, time));
    }

    #endregion

    #region Rotate

    public void RotateZ(GameObject g, Vector3 to, float time, bool loop)
    {
        if (loop)
        {
            AddLooper(g, TweenType.rotate, g.transform.localEulerAngles, to, time);
            return;
        }
        
        StartCoroutine(Tween(g, TweenType.rotate, to, time));
    }

    private void LerpZRotation(GameObject g, Vector3 to, float time)
    {
        float z = Mathf.Lerp(g.transform.localEulerAngles.z, to.z, time);
        Vector3 lerpRotation = new Vector3(0,0,z);
        
        // Check if UI
        if (g.GetComponent<RectTransform>() != null)
        {
            RectTransform rect = g.GetComponent<RectTransform>();
            rect.localEulerAngles = lerpRotation;
            return;
        }
        
        g.transform.localEulerAngles = lerpRotation;
    }

    public void RotateZ(RectTransform rect, Vector3 to, float time, bool loop)
    {
        if (loop)
        {
            AddLooper(rect.gameObject, TweenType.rotate, rect.localEulerAngles, to, time);
            return;
        }
        
        StartCoroutine(TweenUI(rect, TweenType.rotate, to, time));
    }
    
    #endregion

    // Moves a Non-UI GameObject
    private IEnumerator Tween(GameObject g, TweenType type , Vector3 to, float time)
    {
        float t = 0;
        while (t < time)
        {
            switch (type)
            {
                case TweenType.translate:
                    LerpPosition(g, to, t / time);
                    break;
                case TweenType.scale:
                    LerpLocalScale(g, to, t / time);
                    break;
                case TweenType.rotate:
                    LerpZRotation(g, to, t / time);
                    break;
                default:
                    break;
            }
            t += Time.deltaTime;
            yield return null;
        }
        
        SetComplete(g ,type, to);
    }
    
    // Lerps a UI Element
    IEnumerator TweenUI(RectTransform rect, TweenType type , Vector3 to, float time)
    {
        float t = 0;

        while(t < time)
        {
            switch (type)
            {
                case TweenType.translate:
                   LerpAnchoredPosition(rect, to, t / time);
                    break;
                case TweenType.scale:
                    LerpLocalScale(rect.gameObject, to, t / time);
                    break;
                case TweenType.rotate:
                    LerpZRotation(rect.gameObject, to, t / time);
                    break;
                default:
                    break;
            }
            t += Time.deltaTime;
            yield return null;
        }
        
        SetComplete(rect, type, to);
    }

    // Ensure GameObject Lerp Completion
    private void SetComplete(GameObject g ,TweenType type, Vector3 to)
    {
        switch (type)
        {
            case TweenType.translate:
                g.transform.position = to;
                break;
            
            case TweenType.scale:
                g.transform.localScale = to;
                break;
            
            case TweenType.rotate:
                g.transform.localEulerAngles = to;
                break;
        }
    }

    // Ensure UI Lerp Completion
    private void SetComplete(RectTransform rect, TweenType type, Vector3 to)
    {
        switch (type)
        {
            case TweenType.translate:
                rect.anchoredPosition = to;
                break;
            
            case TweenType.scale:
                rect.localScale = to;
                break;
            
            case TweenType.rotate:
                rect.localEulerAngles = to;
                break;
        }
    }
}