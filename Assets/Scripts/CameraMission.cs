using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMission : MonoBehaviour
{
    [SerializeField]
    private GameObject timer;
    [SerializeField]
    private float timervalue = 100;
    public float speed = 80f;
    public float originalscale;

    public void OnButton()
    {
        if(timervalue < originalscale/100*30)
        {
            GameManager.Instance.MissionEnd();
        }
        else
        {
            timervalue = 100;
        }
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 5);
        if (timervalue <= 0)
            timervalue = 100;
        timervalue -= speed * Time.deltaTime;
        timer.transform.localScale = new Vector2(originalscale/100*timervalue, originalscale/100*timervalue);
    }
}
