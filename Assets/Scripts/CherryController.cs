using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    private Tweener tweener;

    private float half_x, half_y;
    private bool spawn = true;

    void Start()
    {
        tweener = GetComponent<Tweener>();

        Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        half_x = camera.aspect * camera.orthographicSize + 0.25f;
        half_y = camera.orthographicSize + 0.25f;

        StartCoroutine(test());
    }

    void Update()
    {
        /*
        if (spawn)
        {
            spawn = false;
            Debug.Log("boi");
            StartCoroutine(SpawnCherry());
        }

        Debug.Log(tweener.waiting());
        //if (tweener.waiting())
            //spawn = true;
        */
    }

    private IEnumerator test()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);

            randomLocation();

            // offset the tween time
            yield return new WaitForSeconds(20f);

            yield return null;
        }
    }

    private IEnumerator SpawnCherry()
    {
        yield return new WaitForSeconds(10f);
        randomLocation();
    }

    private void randomLocation()
    {
        // 0: U, 1: R, 2: D, 3: L
        int side = Random.Range(0, 4);
        float new_x = Random.Range(-half_x, half_x);
        float new_y = Random.Range(-half_y, half_y);

        switch (side)
        {
            case 0: transform.position = new Vector3(new_x, half_y, 1); break;
            case 1: transform.position = new Vector3(half_x, new_y, 1); break;
            case 2: transform.position = new Vector3(new_x, -half_y, 1); break;
            case 3: transform.position = new Vector3(-half_x, new_y, 1); break;
        }

        Vector3 endPoint = Vector3.zero - transform.position;
        tweener.AddTween(transform, transform.position, endPoint, 20f);
    }
}
