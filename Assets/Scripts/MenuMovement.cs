using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMovement : MonoBehaviour
{
    public GameObject[] objects;
    public Vector3[] points;

    private Tweener tweener;
    private Vector3[] lastPos;
    private int[] currentPoints;
    private bool[] needNewTween;
    private Tweener[] tweeners;

    void Start()
    {
        tweener = GetComponent<Tweener>();

        lastPos = new Vector3[objects.Length];

        currentPoints = new int[objects.Length];
        for (int i = 0; i < objects.Length; i++)
            currentPoints[i] = 0;

        tweeners = new Tweener[objects.Length];
        for (int i = 0; i < objects.Length; i++)
            tweeners[i] = objects[i].GetComponent<Tweener>();

        for (int i = 0; i < tweeners.Length; i++)
        {
            float duration = 0.5f * (objects[0].transform.position.x - objects[i].transform.position.x);
            tweeners[i].AddTween(objects[i].transform, objects[i].transform.position, points[0], duration);
        }
    }

    void Update()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (tweeners[i].waiting())
            {
                currentPoints[i] = (currentPoints[i] + 1) % 4;

                float duration = 1.75f;
                if (currentPoints[i] == 1 || currentPoints[i] == 3)
                    duration = 1.25f;

                tweeners[i].AddTween(objects[i].transform, objects[i].transform.position, points[currentPoints[i]], duration);
            }
        }
    }
}
