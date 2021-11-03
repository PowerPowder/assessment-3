using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    //private Tween activeTween;
    private List<Tween> activeTweens = new List<Tween>();

    void Update()
    {
        if (activeTweens.Count > 0)
        {
            for (int i = 0; i < activeTweens.Count; i++)
            {
                Tween tween = activeTweens[i];

                float distance = Vector3.Distance(tween.Target.position, tween.EndPos);

                if (distance > 0.01f)
                {
                    float timeFraction = (Time.time - tween.StartTime) / tween.Duration;
                    tween.Target.position = Vector3.Lerp(tween.StartPos, tween.EndPos, timeFraction);
                }
                else
                {
                    tween.Target.position = tween.EndPos;
                    activeTweens.RemoveAt(i);
                }
            }
        }
    }

    public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
        if (!TweenExists(targetObject))
        {
            Tween tween = new Tween(targetObject, startPos, endPos, Time.time, duration);
            activeTweens.Add(tween);
            return true;
        }

        return false;
    }

    public bool TweenExists(Transform target)
    {
        foreach (Tween tween in activeTweens)
        {
            if (tween.Target.Equals(target))
                return true;
        }

        return false;
    }

    public bool waiting()
    {
        return activeTweens.Count == 0 ? true : false;
    }
}
