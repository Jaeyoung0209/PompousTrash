using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class border : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnDamage()
    {
        animator.SetTrigger("OnDamage");
    }
}
