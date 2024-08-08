using System.Collections.Generic;
using UnityEngine;

public class Whiteboard : MonoBehaviour
{
    public Camera cam;
    public GameObject linePrefab;
    public GameObject canvas;

    private LineRenderer currentLineRenderer;
    private List<Vector3> points;
    private List<GameObject> lines;

    void Start()
    {
        points = new List<Vector3>();
        lines = new List<GameObject>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateLine();
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            if (!points.Contains(mousePos))
            {
                points.Add(mousePos);
                currentLineRenderer.positionCount = points.Count;
                currentLineRenderer.SetPosition(points.Count - 1, mousePos);
            }
        }
    }

    void CreateLine()
    {
        GameObject line = Instantiate(linePrefab, canvas.transform);
        currentLineRenderer = line.GetComponent<LineRenderer>();
        //points.Clear();
        lines.Add(line);
    }

    public void ClearLines()
    {
        foreach (GameObject line in lines)
        {
            Destroy(line);
        }
        lines.Clear();
    }
}
