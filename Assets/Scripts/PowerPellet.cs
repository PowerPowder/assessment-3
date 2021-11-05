using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerPellet : MonoBehaviour
{
    bool activated = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated)
        {
            activated = true;

            GameObject.FindGameObjectWithTag("Ghost1").GetComponent<Animator>().Play("Scared");
            GameObject.FindGameObjectWithTag("Ghost2").GetComponent<Animator>().Play("Scared");
            GameObject.FindGameObjectWithTag("Ghost3").GetComponent<Animator>().Play("Scared");
            GameObject.FindGameObjectWithTag("Ghost4").GetComponent<Animator>().Play("Scared");

            GameObject.FindGameObjectWithTag("MusicController").GetComponent<MusicController>().currentTrack = MusicController.Music.Scared;

            StartCoroutine(countDown());

            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private IEnumerator countDown()
    {
        HUDController hud = GameObject.FindGameObjectWithTag("HUDController").GetComponent<HUDController>();
        Text ghostText = hud.ghostScaredTimer.GetComponent<Text>();

        ghostText.text = "10";

        for (int i = 10; i >= 0; i--)
        {
            if (i == 3)
            {
                GameObject.FindGameObjectWithTag("Ghost1").GetComponent<Animator>().Play("Recovering");
                GameObject.FindGameObjectWithTag("Ghost2").GetComponent<Animator>().Play("Recovering");
                GameObject.FindGameObjectWithTag("Ghost3").GetComponent<Animator>().Play("Recovering");
                GameObject.FindGameObjectWithTag("Ghost4").GetComponent<Animator>().Play("Recovering");
            }

            yield return new WaitForSeconds(1f);
            ghostText.text = i + "";
        }

        GameObject.FindGameObjectWithTag("Ghost1").GetComponent<Animator>().Play("Normal");
        GameObject.FindGameObjectWithTag("Ghost2").GetComponent<Animator>().Play("Normal");
        GameObject.FindGameObjectWithTag("Ghost3").GetComponent<Animator>().Play("Normal");
        GameObject.FindGameObjectWithTag("Ghost4").GetComponent<Animator>().Play("Normal");

        GameObject.FindGameObjectWithTag("MusicController").GetComponent<MusicController>().currentTrack = MusicController.Music.Normal;

        Destroy(gameObject);
    }
}
