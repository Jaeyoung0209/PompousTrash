using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour
{
    public List<GameObject> Garbage = new List<GameObject>();
    public Vector2 BottomLeft;
    public Vector2 TopRight;
    public float waittime;
    private GameObject player;
    public float startwaittime;
    [SerializeField]
    private Vector2 targetlocation;
    public bool moveable = true;
    [SerializeField]
    private bool aggro = false;
    [SerializeField]
    private bool angry = false;
    private Rigidbody2D rb;
    private Animator animator;
    //private Collider2D collider;
    public float speed;
    [SerializeField]
    private bool throwgarbage = false;
    public int dir = 0;
    public int missiontype;
    private bool resolve = false;
    public float stilltimer = 3f;
    public Vector2 pastlocation;
    [SerializeField]
    private bool UseTimer = true;
    private float angertimer = 6f;
    private bool throwing = false;
    private bool whilethrowing = false;
    private bool walkable = true;

    private void Start()
    {
        pastlocation = transform.position;
        missiontype = Random.Range(1, 3);
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //collider = GetComponent<Collider2D>();
        NewLocation();

        int colour = Random.Range(0, 6);
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (j != colour)
                    transform.GetChild(i).gameObject.transform.GetChild(1).gameObject.transform.GetChild(j).gameObject.SetActive(false);
            }
        }
        ChangeDirection(new Vector2(transform.position.x, transform.position.y - 10));
        animator.SetBool("stop", true);
    }

    void ChangeDirection(Vector2 direction)
    {
        if (Mathf.Abs(transform.position.x - direction.x) > Mathf.Abs(transform.position.y - direction.y))
        {
            if (direction.x < transform.position.x)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.SetActive(false);
                transform.GetChild(3).gameObject.SetActive(false);
                dir = 2;
            }
            else if (direction.x > transform.position.x)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(3).gameObject.SetActive(false);
                dir = 1;
            }
        }
        else {
            if (direction.y < transform.position.y)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(false);
                transform.GetChild(3).gameObject.SetActive(false);
                dir = 0;
            }
            else if (direction.y > transform.position.y)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(false);
                transform.GetChild(3).gameObject.SetActive(true);
                dir = 3;
            }
        }
    }

    void StopAnimation()
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            animator.SetBool(parameter.name, false);
        }
    }

    private void NewLocation()
    {
        if (Random.Range(0, 10) < 5f)
            targetlocation = new Vector2(transform.position.x + Random.Range(-10, 10), transform.position.y);
        else
            targetlocation = new Vector2(transform.position.x, transform.position.y + Random.Range(-10, 10));
    }
    void FixedUpdate()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.position = new Vector3(transform.position.x, transform.position.y, 1);

        if (!UseTimer)
        {
            StopAnimation();
            animator.SetBool("stop", true);
            rb.velocity = Vector2.zero;
        }
        else if (moveable && !aggro && !angry && !resolve)
        {
            WalkAnimation(targetlocation);
            //transform.position = Vector2.MoveTowards(transform.position, targetlocation, speed * Time.deltaTime);
            stilltimer -= Time.deltaTime;
            if (stilltimer <= 0)
            {
                stilltimer = 2f;
                if (Vector2.Distance((Vector2)transform.position, pastlocation) < 0.1f && animator.GetBool("stop") != true)
                {
                    WaitNewLocation();
                }
                else
                {
                    pastlocation = (Vector2)transform.position;
                }
            }
            if (Vector2.Distance(transform.position, targetlocation) < 0.5f)
            {
                if (UseTimer)
                    WaitNewLocation();
            }

            if (Vector2.Distance(transform.position, player.transform.position) < 15f)
            {
                aggro = true;
            }
        }
        else if (moveable && aggro && !angry && !resolve)
        {
            ChangeDirection(player.transform.position);
            transform.GetChild(4).gameObject.SetActive(true);
            angertimer -= Time.deltaTime;
            if (angertimer <= 0)
            {
                angertimer = 6;
                aggro = false;
                angry = true;
                walkable = true;
            }
            if (Vector2.Distance(transform.position, player.transform.position) < 6f)
            {
                StopAnimation();
                rb.velocity = Vector3.zero;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                animator.SetBool("stop", true);
            }
            else
            {
                WalkAnimation(player.transform.position);
            }
        }
        else if (moveable && angry && !resolve)
        {
            transform.GetChild(4).gameObject.SetActive(false);
            ChangeDirection(player.transform.position);

            if (Vector2.Distance(transform.position, player.transform.position) > 14f && walkable)
            {
                StopAnimation();
                walkable = false;
                WalkAnimation(player.transform.position);
                if (Vector2.Distance(transform.position, player.transform.position) < 12f)
                    walkable = false;
            }
            else if ((Vector2.Distance(transform.position, player.transform.position) < 13f && !walkable) || throwing)
            {
                throwing = false;
                StopAnimation();
                animator.SetBool("stop", true);
                StartCoroutine("ThrowCoroutine");
                angry = false;
            }
            else if (walkable)
            {
                WalkAnimation(new Vector2(transform.position.x + 100 * (transform.position.x - player.transform.position.x), transform.position.y + 100 * (transform.position.y - player.transform.position.y)));
                stilltimer -= Time.deltaTime;
                if (stilltimer <= 0)
                {
                    stilltimer = 2f;
                    if (Vector2.Distance((Vector2)transform.position, pastlocation) < 0.1f)
                    {
                        WalkAnimation(player.transform.position);
                        throwing = true;
                    }
                    else
                    {
                        pastlocation = (Vector2)transform.position;
                    }
                }
            }
        }
        else if (moveable && resolve == true)
        {
            ChangeDirection(player.transform.position);

            if (Vector2.Distance(transform.position, player.transform.position) > 30f)
            {
                resolve = false;
                aggro = false;
                angry = false;
                stilltimer = 2f;
            }
            else
            {
                WalkAnimation(new Vector2(transform.position.x + 100 * (transform.position.x - player.transform.position.x), transform.position.y + 100 * (transform.position.y - player.transform.position.y)));
                stilltimer -= Time.deltaTime;
                if (stilltimer <= 0)
                {
                    stilltimer = 2f;
                    if (Vector2.Distance((Vector2)transform.position, pastlocation) < 0.1f)
                    {
                        resolve = false;
                        aggro = false;
                        angry = false;
                        stilltimer = 2f;
                    }
                    else
                    {
                        pastlocation = (Vector2)transform.position;
                    }
                }
            }
        }
        else if (!moveable)
        {
            if (!whilethrowing)
            {
                StopAnimation();
                animator.SetBool("stop", true);
            }
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    IEnumerator ThrowCoroutine()
    {
        moveable = false;
        whilethrowing = true;
        for (int i = 0; i < 2; i++)
        {
            WalkAnimation(player.transform.position);
            StopAnimation();
            animator.SetBool("stop", true);
            yield return new WaitForSeconds(1);
            animator.SetTrigger("throw");
            yield return new WaitForSeconds(1);
            ThrowGarbage();
            animator.SetTrigger("throw2");
            yield return new WaitForSeconds(3);
        }
        moveable = true;
        throwgarbage = false;
        whilethrowing = false;
        resolve = true;
    }

    private void ThrowGarbage()
    {
        if(dir == 0)
        {
            Instantiate(Garbage[Random.Range(0,3)], new Vector2(transform.position.x + 1, transform.position.y + 0.6f), Quaternion.identity);
        }
        else if(dir == 1)
        {
            Instantiate(Garbage[Random.Range(0, 3)], new Vector2(transform.position.x - 0.37f, transform.position.y + 0.6f), Quaternion.identity);
        }
        else if(dir == 2)
        {
            Instantiate(Garbage[Random.Range(0, 3)], new Vector2(transform.position.x + 0.37f, transform.position.y + 0.5f), Quaternion.identity);
        }
        else if(dir == 3)
        {
            Instantiate(Garbage[Random.Range(0, 3)], new Vector2(transform.position.x -1.2f, transform.position.y + 0.36f), Quaternion.identity);
        }
    }

    private void WalkAnimation(Vector2 target)
    {
        if (moveable)
        {
            StopAnimation();
            ChangeDirection(target);
            if (Mathf.Abs(transform.position.x - target.x) > Mathf.Abs(transform.position.y - target.y))
            {
                if (transform.position.x > target.x)
                {
                    animator.SetBool("walkleft", true);
                }
                else
                {
                    animator.SetBool("walkright", true);
                }
            }
            else if (Mathf.Abs(transform.position.x - target.x) <= Mathf.Abs(transform.position.y - target.y))
            {
                if (transform.position.y > target.y)
                {
                    animator.SetBool("walkdown", true);
                }
                else
                {
                    animator.SetBool("walkup", true);
                }
            }
            //transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            //rb.MovePosition(Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime));
            RBmovetowards(target);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void RBmovetowards(Vector3 target)
    {
        var direction = (Vector2)(target - transform.position).normalized;
        rb.velocity = (direction * speed);
    }

    private void WaitNewLocation()
    {
        StopAnimation();
        animator.SetBool("stop", true);
        StartCoroutine("NewLocationCoroutine");
    }
    
    IEnumerator NewLocationCoroutine()
    {
        UseTimer = false;
        yield return new WaitForSeconds(2);
        NewLocation();
        UseTimer = true;
    }

    public void resolved() {
        resolve = true;
    }
}
