﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Classe reprise par HAMICHE Bilal le 29/04, elle est utilisée pour gérer les courbes sur le graphe
/// </summary>
public class DD_Lines : DD_DrawGraphic {

    //[SerializeField]
    //Variable de type float qui gère l'épaisseur des courbes
    private float m_Thickness = 5;

    //[SerializeField]
    private bool m_IsShow = true;
    private bool m_CurIsShow = true;//Permet de savoir s'il faut executer UpdateGeometry();

    private List<Vector2> PointList = new List<Vector2>();
    
    private int CurStartPointSN = 0;

    private DD_DataDiagram m_DataDiagram = null;
    private DD_CoordinateAxis m_Coordinate = null;

    [NonSerialized]
    public string lineName = "";

    public float Thickness {
        get { return m_Thickness; }
        set { m_Thickness = value; }
    }

    public bool IsShow {
        get { return m_IsShow; }
        set {
            if(value != m_IsShow) {
                ///Met à jour OnPopulateMesh
                UpdateGeometry();
            }

            m_IsShow = value;
        }
    }

    protected override void Awake() {

        m_DataDiagram = GetComponentInParent<DD_DataDiagram>();
        if (null == m_DataDiagram) {
            Debug.Log(this + "null == m_DataDiagram");
        }

        m_Coordinate = GetComponentInParent<DD_CoordinateAxis>();
        if(null == m_Coordinate) {
            Debug.Log(this + "null == m_Coordinate");
        }

        GameObject parent = gameObject.transform.parent.gameObject;
        if(null == parent) {
            Debug.Log(this + "null == parent");
        }

        RectTransform parentrt = parent.GetComponent<RectTransform>();
        RectTransform localrt = gameObject.GetComponent<RectTransform>();
        if ((null == localrt) || (null == parentrt)) {
            Debug.Log(this + "null == localrt || parentrt");
        }

        //Le point d'ancrage est dans le coin inférieur gauche
        localrt.anchorMin = Vector2.zero;
        localrt.anchorMax = new Vector2(1, 1);
        //Positionne l'axe dans le coin inférieur gauche
        localrt.pivot = Vector2.zero;
        //Positionne les coordonnées de l'axe dans le coin inférieur gauche 
        localrt.anchoredPosition = Vector2.zero;
        //Défini la marge à 0
        localrt.sizeDelta = Vector2.zero;

        if(null != m_Coordinate) {
            m_Coordinate.CoordinateRectChangeEvent += OnCoordinateRectChange;
            m_Coordinate.CoordinateScaleChangeEvent += OnCoordinateScaleChange;
            m_Coordinate.CoordinateeZeroPointChangeEvent += OnCoordinateZeroPointChange;
        }

    }

    private void Update() {

        if (m_CurIsShow == m_IsShow)
            return;

        m_CurIsShow = m_IsShow;

        ///Met à jour OnPopulateMesh
        UpdateGeometry();
    }

    private float ScaleX(float x) {

        if (null == m_Coordinate) {
            Debug.Log(this + "null == m_Coordinate");
            return x;
        }

        return (x / m_Coordinate.coordinateAxisViewRangeInPixel.width);
    }


    private float ScaleY(float y) {

        if (null == m_Coordinate) {
            Debug.Log(this + "null == m_Coordinate");
            return y;
        }

        return (y / m_Coordinate.coordinateAxisViewRangeInPixel.height);
    }

    private int GetStartPointSN(List<Vector2> points, float startX) {

        int ret = 0;
        float x = 0;
        foreach (Vector2 p in points) {
            if(x > startX) {
                return points.IndexOf(p);
            }
            x += p.x;//ScaleX(p.x);
            ret++;
        }

        return ret;
    }

    private void OnCoordinateRectChange(object sender, DD_CoordinateRectChangeEventArgs e) {

        UpdateGeometry();
    }

    private void OnCoordinateScaleChange(object sender, DD_CoordinateScaleChangeEventArgs e) {

        UpdateGeometry();
    }

    private void OnCoordinateZeroPointChange(object sender, DD_CoordinateZeroPointChangeEventArgs e) {

        CurStartPointSN = GetStartPointSN(PointList, m_Coordinate.coordinateAxisViewRangeInPixel.x);
        UpdateGeometry();
    }

    protected override void OnPopulateMesh(VertexHelper vh) {

        vh.Clear();

        if (false == m_IsShow) {
            return;
        }

        float x = 0;
        List<Vector2> points = new List<Vector2>();
        for (int i = CurStartPointSN; i < PointList.Count; i++) {
            points.Add(new Vector2(ScaleX(x), ScaleY(PointList[i].y - m_Coordinate.coordinateAxisViewRangeInPixel.y)));
            x += PointList[i].x;
            if (x >= m_Coordinate.coordinateAxisViewRangeInPixel.width * rectTransform.rect.width)
                break;
        }

        DrawHorizontalLine(vh, points, color, m_Thickness, new Rect(0, 0, rectTransform.rect.width, rectTransform.rect.height));
    }

    public void AddPoint(Vector2 point) {

        PointList.Insert(0, new Vector2(point.x, point.y));

        while (PointList.Count > m_DataDiagram.m_MaxPointNum) {
            PointList.RemoveAt(PointList.Count - 1);
            print(PointList.Count);
        }

        UpdateGeometry();
    }

    public bool Clear() {

        if (null == m_Coordinate) {
            Debug.LogWarning(this + "null == m_Coordinate");
        }

        try {
            m_Coordinate.CoordinateRectChangeEvent -= OnCoordinateRectChange;
            m_Coordinate.CoordinateScaleChangeEvent -= OnCoordinateScaleChange;
            m_Coordinate.CoordinateeZeroPointChangeEvent -= OnCoordinateZeroPointChange;

            PointList.Clear();
        } catch (NullReferenceException e) {
            print(this + " : " + e);
            return false;
        }

        return true;
    }
}
