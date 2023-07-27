using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    public Button startbutton;
    public Button controlbutton;
    public Image controlpannel;
    public Image ClosedLid1;
    public Image ClosedLid2;
    public Image OpenLid1;
    public Image OpenLid2;
    public Button CloseButton;
    private bool controlopen = false;

    
    void Start()
    {
        controlpannel.enabled = false;
        controlpannel.gameObject.SetActive(false);
        OpenLid1.enabled = false;
        OpenLid2.enabled = false;
        controlbutton.onClick.AddListener(CTRbutton);
        startbutton.onClick.AddListener(Startgame);
        CloseButton.onClick.AddListener(close);
    }
    void Startgame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void close()
    {
        controlopen = false;
        startbutton.interactable = true;
        controlbutton.interactable = true;
        controlpannel.enabled = false;
        controlpannel.gameObject.SetActive(false);
    }

    public void buttonhoverstart()
    {
        if (controlopen == false)
        {
            ClosedLid1.enabled = false;
            OpenLid1.enabled = true;
        }
    }
    public void buttonhovercontrol()
    {
        if (controlopen == false)
        {
            ClosedLid2.enabled = false;
            OpenLid2.enabled = true;
        }
    }
    public void buttonnothoverstart()
    {
        if (controlopen == false)
        {
            ClosedLid1.enabled = true;
            OpenLid1.enabled = false;
        }
    }

    public void buttonnothovercontrol()
    {
        if (controlopen == false)
        {
            ClosedLid2.enabled = true;
            OpenLid2.enabled = false;
        }
    }
    void CTRbutton()
    {
        controlopen = true;
        controlbutton.interactable = false;
        startbutton.interactable = false;
        controlpannel.enabled = true;
        controlpannel.gameObject.SetActive(true);
    }
}
