using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraseButton : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.name == this.name)
                {
                    animator.SetTrigger("OnClick");
                    LineRenderer[] Line = GameObject.FindObjectsOfType<LineRenderer>();
                    for (int i = 0; i < Line.Length; i++)
                    {
                        if (Line[i].gameObject.name != "brush")
                            Destroy(Line[i].gameObject);
                    }
                }
            }
        }
    }
}
