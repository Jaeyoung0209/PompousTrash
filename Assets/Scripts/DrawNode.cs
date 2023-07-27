using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawNode : MonoBehaviour
{
    public Draw brush;
    public GameObject submitbutton;

    private void Start()
    {
        submitbutton.SetActive(false);
    }
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 5);
        if(brush.DrawNodes.Count <= 0)
        {
            submitbutton.SetActive(true);
        }
    }

    private void OnMouseEnter()
    {
        brush.mouseonboard = true;
    }
    private void OnMouseExit()
    {
        brush.mouseonboard = false;
    }
}
