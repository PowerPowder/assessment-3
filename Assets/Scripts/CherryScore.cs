using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CherryScore : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HUDController hud = GameObject.FindGameObjectWithTag("HUDController").GetComponent<HUDController>();
        string text = hud.score.GetComponent<Text>().text;

        int score = int.Parse(text.Split(' ')[1]) + 100;
        hud.score.GetComponent<Text>().text = "Score: " + score;

        CherryController cc = GameObject.FindGameObjectWithTag("CherryController").GetComponent<CherryController>();
        cc.StopCherry();
    }
}
