using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public bool controlable = true;
    public float speed = 5f;
    private Vector2 direction;
    public float life = 100f;
    [SerializeField]
    private border border;
    [SerializeField]
    private HeartEffect hearteffect;



    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ChangeDirection(Vector2.down);
    }

    public void heal(float healval)
    {
        life += healval;
        if (life > 100)
            life = 100;
        hearteffect.Heart();
    }

    void StopAnimation()
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            animator.SetBool(parameter.name, false);
        }
    }

    void ChangeDirection(Vector2 direction)
    {
        StopAnimation();
        if (direction.y > 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
            animator.SetBool("WalkUp", true);
        }
        else if(direction.y < 0)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
            animator.SetBool("WalkDown", true);
        }
        else if(direction.x > 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(false);
            animator.SetBool("WalkRight", true);
        }
        else if(direction.x < 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(true);
            animator.SetBool("WalkLeft", true);
        }
    }
    void Update()
    {
        if (controlable)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            direction = new Vector2(Input.GetAxis("Horizontal"), 0);
            float vertical = Input.GetAxis("Vertical");
            if (vertical != 0)
            {
                direction = new Vector2(0, Input.GetAxis("Vertical"));
            }
            ChangeDirection(direction);
            
            if (direction == Vector2.zero)
            {
                StopAnimation();
                animator.SetBool("Stop", true);
            }
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            StopAnimation();
            animator.SetBool("Stop", true);
        }
        life -= 0.8f * Time.deltaTime;
        transform.GetChild(4).gameObject.transform.GetChild(1).gameObject.transform.GetChild(3).gameObject.transform.localScale = new Vector2((float)1.49 / 100 * life, 1);
        if (life <= 0)
        {
            Dead();
        }
    }
    private void FixedUpdate()
    {
        if(controlable)
            rb.MovePosition((Vector2)transform.position + speed * Time.deltaTime * direction);
    }

    public void OnDamage(float damage)
    {
        if (controlable)
        {
            life -= damage;
            border.OnDamage();
        }
    }
    private void Dead()
    {
        GameManager.Instance.PlayerDead();
    }
}
