using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartEffect : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Heart()
    {
        animator.SetTrigger("HeartEffect");
    }
}
