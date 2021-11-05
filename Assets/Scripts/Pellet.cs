using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pellet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HUDController hud = GameObject.FindGameObjectWithTag("HUDController").GetComponent<HUDController>();
        string text = hud.score.GetComponent<Text>().text;

        int score = int.Parse(text.Split(' ')[1]) + 10;
        hud.score.GetComponent<Text>().text = "Score: " + score;

        Destroy(gameObject);
    }
}
