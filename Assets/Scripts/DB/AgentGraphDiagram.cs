using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentGraphDiagram : MonoBehaviour {

    List<GameObject> lineList = new List<GameObject>();

    public DD_DataDiagram m_DataDiagram;
    private float h = 0;

    void AddALine(string label) {
        if (null == m_DataDiagram)
            return;

        Color color = Color.HSVToRGB((h += 0.1f) > 1 ? (h - 1) : h, 0.8f, 0.8f);
        GameObject line = m_DataDiagram.AddLine(label, color);

        if (null != line)
            lineList.Add(line);
    }

    // Use this for initialization
    void Start () {
        GameObject dd = GameObject.Find("DataDiagram");
        if(null == dd) {
            Debug.LogWarning("can not find a gameobject of DataDiagram");
            return;
        }
        m_DataDiagram = dd.GetComponent<DD_DataDiagram>();

        m_DataDiagram.PreDestroyLineEvent += (s, e) => { lineList.Remove(e.line); };

        AddALine("Prédateurs");
        AddALine("Proies");
        AddALine("Autotrophes");

        GameObject.Find("Player").GetComponent<DataUpdater>().AgentGraph = m_DataDiagram;
        GameObject.Find("Player").GetComponent<DataUpdater>().PredatorsLine = lineList[0];
        GameObject.Find("Player").GetComponent<DataUpdater>().PreysLine = lineList[1];
        GameObject.Find("Player").GetComponent<DataUpdater>().AutotrophsLine = lineList[2];
    }
}
