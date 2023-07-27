﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private bool coroutine = false;

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
                if (hit.transform.name == this.name && coroutine == false)
                {
                    StartCoroutine("ClickCoroutine");
                }
            }
        }
    }
    IEnumerator ClickCoroutine()
    {
        coroutine = true;
        animator.SetTrigger("OnClick");
        GameManager.Instance.Restart();
        yield return new WaitForSeconds(0.35f);
        coroutine = false;

    }
}
