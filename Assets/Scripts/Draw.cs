using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    private Camera m_camera;
    public GameObject brush;
    public Vector2 topright;
    public Vector2 bottomleft;
    public bool mouseonboard = false;
    public List<GameObject> DrawNodes;

    LineRenderer currentLineRenderer;

    Vector2 lastPos;

    private void Start()
    {
        m_camera = Camera.main;
    }

    private void Update()
    {
        Drawing();
    }

    void Drawing()
    {
        if (mouseonboard)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                CreateBrush();
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                PointToMousePos();
            }
            else
            {
                currentLineRenderer = null;
            }
        }
    }

    void CreateBrush()
    {
        GameObject brushInstance = Instantiate(brush);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        //because you gotta have 2 points to start a line renderer, 
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);

        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);

    }

    void AddAPoint(Vector2 pointPos)
    {
        currentLineRenderer.positionCount += 1;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }

    void PointToMousePos()
    {
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        if (lastPos != mousePos)
        {
            AddAPoint(mousePos);
            lastPos = mousePos;
            if (DrawNodes.Count > 0)
            {
                if (Vector2.Distance(mousePos, DrawNodes[0].transform.position) < 1f)
                {
                    DrawNodes.RemoveAt(0);
                }
            }
        }
    }

}