using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStartButton : MonoBehaviour
{
    [SerializeField]
    private NPC parentnpc;
    private Animator animator;
    private bool coroutine = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (parentnpc.missiontype == 1)
        {
            transform.GetChild(3).gameObject.SetActive(true);
            transform.GetChild(4).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(3).gameObject.SetActive(false);
            transform.GetChild(4).gameObject.SetActive(true);
        }
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
        yield return new WaitForSeconds(0.45f);
        coroutine = false;
        GameManager.Instance.MissionStart(parentnpc.missiontype, parentnpc);
        gameObject.SetActive(false);
    }
}
