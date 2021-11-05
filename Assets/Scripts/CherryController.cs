using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(test());
    }

    // Update is called once per frame
    void Update()
    {
        randomLocation();
    }

    private IEnumerator test()
    {
        //while (true)

        while (true)
        {
            yield return new WaitForSeconds(10f);
            Debug.Log("spawn cherry");
        }
    }

    private void randomLocation()
    {
        int side = Random.Range(0, 4);
        Debug.Log(side);
        //transform.position
    }
}
