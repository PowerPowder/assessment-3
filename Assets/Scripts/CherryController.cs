using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    [SerializeField]
    private GameObject cherryPrefab;
    private GameObject currentCherry;

    private Tweener tweener;

    private float half_x, half_y;

    void Start()
    {
        tweener = GetComponent<Tweener>();

        Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        half_x = camera.aspect * camera.orthographicSize + 0.25f;
        half_y = camera.orthographicSize + 0.25f;

        StartCoroutine(spawnAndMoveCherry());
    }

    void Update()
    {
        if (currentCherry != null && tweener.waiting())
        {
            Destroy(currentCherry);
            currentCherry = null;
        }
    }

    private IEnumerator spawnAndMoveCherry()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);

            currentCherry = Instantiate(cherryPrefab);
            randomLocation();

            // offset the tween time
            yield return new WaitForSeconds(20f);
            yield return null;
        }
    }

    private void randomLocation()
    {
        // 0: U, 1: R, 2: D, 3: L
        int side = Random.Range(0, 4);
        float new_x = Random.Range(-half_x, half_x);
        float new_y = Random.Range(-half_y, half_y);

        switch (side)
        {
            case 0: currentCherry.transform.position = new Vector3(new_x, half_y, 1); break;
            case 1: currentCherry.transform.position = new Vector3(half_x, new_y, 1); break;
            case 2: currentCherry.transform.position = new Vector3(new_x, -half_y, 1); break;
            case 3: currentCherry.transform.position = new Vector3(-half_x, new_y, 1); break;
        }

        Vector3 endPoint = Vector3.zero - currentCherry.transform.position;
        tweener.AddTween(currentCherry.transform, currentCherry.transform.position, endPoint, 20f);
    }
}
