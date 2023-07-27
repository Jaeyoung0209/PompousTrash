using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage : MonoBehaviour
{
    private GameObject player;
    public float speed = 10f;
    private Animator animator;
    [SerializeField]
    private float delay = 1f;
    private Vector2 target;
    private bool collisionbool = false;
    private void Start()
    {
        player = GameObject.Find("Player");
        target = player.transform.position;
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        if (delay >= 0)
        {
            delay -= Time.deltaTime;
            return;
        }
        else
        {
            if (Vector2.Distance(transform.position, target) > 1f)
                transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            else
            {
                StartCoroutine("DeadCoroutine");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && collisionbool == false)
        {
            collisionbool = true;
            PlayerControl control = player.GetComponent<PlayerControl>();
            control.OnDamage(10);
            StartCoroutine("DeadCoroutine");
        }
    }

    IEnumerator DeadCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        animator.SetTrigger("garbagedie");
        Destroy(this.gameObject);
    }


}
