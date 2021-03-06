using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe reprise par HAMICHE Bilal le 30/04 pour le protoype, elle permet de modifier
/// le cadre rectangulaire du graphe en fonction des événements.
/// Cette classe est reprise depuis ce lien https://assetstore.unity.com/packages/tools/gui/dynamic-line-chart-108651
/// </summary>
public class DD_CoordinateRectChangeEventArgs : EventArgs
{

    public Rect viewRectInPixel;

    public DD_CoordinateRectChangeEventArgs(Rect newRect) : base()
    {

        viewRectInPixel = newRect;
    }
}

/// <summary>
/// Classe reprise par HAMICHE Bilal le 30/04 pour le prototype, elle permet de modifier 
/// les valeurs en abscisses et en ordonnées en fonction des événements.
/// Cette classe est reprise depuis ce lien https://assetstore.unity.com/packages/tools/gui/dynamic-line-chart-108651
/// </summary>
public class DD_CoordinateScaleChangeEventArgs : EventArgs
{

    /// <summary>
    /// Variable de type float qui désigne les valeurs en abscisses.
    /// </summary>

    public float scaleX;
    /// <summary>
    /// Variable de type float qui désigne les valeurs en abscisses.
    /// </summary>
    public float scaleY;

    /// <summary>
    /// Constructeur de la classe DD_CoordinateScaleChangeEventArgs permettant de créer les objets scaleX et scaleY.
    /// </summary>
    /// <param name="scaleX">Variable de type float qui représente les valeurs en abscisses. </param>
    /// <param name="scaleY">Variable de type float qui représente les valeurs en ordonnées. </param>
    public DD_CoordinateScaleChangeEventArgs(float scaleX, float scaleY) : base()
    {

        this.scaleX = scaleX;
        this.scaleY = scaleY;
    }
}

/// <summary>
/// Classe reprise par HAMICHE Bilal le 30/04 pour le prototype, elle permet de modifier les événements de la zone de observation actuelle.
/// Cette classe est reprise depuis ce lien https://assetstore.unity.com/packages/tools/gui/dynamic-line-chart-108651
/// </summary>
public class DD_CoordinateZeroPointChangeEventArgs : EventArgs
{

    public Vector2 zeroPoint;

    public DD_CoordinateZeroPointChangeEventArgs(Vector2 zeroPoint) : base()
    {

        this.zeroPoint = zeroPoint;
    }
}

/// <summary>
/// Classe reprise par HAMICHE Bilal le 30/04 pour le prototype, elle permet de gérer l'affichage des axes sur le graphe
/// et qui depend de la classe DD_DrawGraphic.
/// Cette classe est reprise depuis ce lien https://assetstore.unity.com/packages/tools/gui/dynamic-line-chart-108651
/// </summary>
public class DD_CoordinateAxis : DD_DrawGraphic
{


    /// <summary>
    /// Toutes les constantes utilisées dans la classe.
    /// </summary>
    #region const value
    private static readonly string MARK_TEXT_BASE_NAME = "MarkText";
    private static readonly string LINES_BASE_NAME = "Line";
    private static readonly string COORDINATE_RECT = "CoordinateRect";
    private const float INCH_PER_CENTIMETER = 0.3937008f;
    private readonly float[] MarkIntervalTab = { 1, 2, 5 };//tableau supporte pas float
    #endregion

    #region property

    //[SerializeField]
    private DD_DataDiagram m_DataDiagram = null;
    private RectTransform m_CoordinateRectT = null;

    /// <summary>
    /// Préréglage des lignes de pliage, cela permet d'améliorer les performances.
    /// </summary>
    private GameObject m_LinesPreb = null;

    /// <summary>
    /// Préréglage des polices des axes,  cela permet d'améliorer les performances.
    /// </summary>
    private GameObject m_MarkTextPreb = null;

    /// <summary>
    /// Variable de type liste qui recupère toutes les listes qui existent.
    /// </summary>
    private List<GameObject> m_LineList = new List<GameObject>();


    /// <summary>
    /// Variable de type Vector2 qui gere la vitesse du zoom en fonction des défilement.
    /// </summary>
    private Vector2 m_ZoomSpeed = new Vector2(1, 1);

    /// <summary>
    /// Variable de type Vector2 qui gere la vitesse des mouvements sur le graphe.
    /// </summary>
    private Vector2 m_MoveSpeed = new Vector2(1, 1);

    /// <summary>
    /// Intervalle maximal extensible des axes des ordonnées.
    /// </summary>
    private float m_CoordinateAxisMaxWidth = 100;
    private float m_CoordinateAxisMinWidth = 0.1f;

    /// <summary>
    /// Variable de type float représentant l'épaisseur de la ligne .
    /// </summary>
    private float m_RectThickness = 2;

    /// <summary>
    /// Variable de type Color qui permet de mettre la couleur de fond en noir.
    /// </summary>
    private Color m_BackgroundColor = new Color(0, 0, 0, 0.5f);

    /// <summary>
    /// Variable de type Color qui permet de régler la couleur des points.
    /// </summary>
    private Color m_MarkColor = new Color(0.8f, 0.8f, 0.8f, 1);

    /// <summary>
    /// Liste contenant les objets textes
    /// </summary>
    private List<GameObject> m_MarkHorizontalTexts = new List<GameObject>();


    /// <summary>
    /// Hauteur de la police de l'axe des ordonnées en pixels.
    /// </summary>
    private float m_MinMarkTextHeight = 20;


    private float m_PixelPerMark
    {
        get { return INCH_PER_CENTIMETER * m_DataDiagram.m_CentimeterPerMark * Screen.dpi; }
    }

    private Rect m_CoordinateAxisRange
    {
        get
        {
            try
            {
                Vector2 sizePixel = m_CoordinateRectT.rect.size;
                return new Rect(0, 0,
                    sizePixel.x / (m_DataDiagram.m_CentimeterPerCoordUnitX * INCH_PER_CENTIMETER * Screen.dpi),
                    sizePixel.y / (m_DataDiagram.m_CentimeterPerCoordUnitY * INCH_PER_CENTIMETER * Screen.dpi));
            }
            catch (NullReferenceException e)
            {
                Debug.Log(this + " : " + e);
            }
            return new Rect(Vector2.zero, GetComponent<RectTransform>().rect.size);
        }
    }

    /// <summary>
    /// Indique la position du point zéro de la valeur de l'échelle de la zone d'observation actuelle 
    /// et est utilisé pour réaliser le mouvement de l'axe.
    /// </summary>
    private Rect m_CoordinateAxisViewRange = new Rect(1, 1, 1, 1);

    private float m_CoordinateAxisViewSizeX
    {
        get
        {
            try
            {
                return m_CoordinateAxisRange.width * m_CoordinateAxisViewRange.width;
            }
            catch (NullReferenceException e)
            {
                Debug.Log(this + " : " + e);
            }
            return m_CoordinateAxisRange.width;
        }
    }

    private float m_CoordinateAxisViewSizeY
    {
        get
        {
            try
            {
                return m_CoordinateAxisRange.height * m_CoordinateAxisViewRange.height;
            }
            catch (NullReferenceException e)
            {
                Debug.Log(this + " : " + e);
            }
            return m_CoordinateAxisRange.width;
        }
    }

    /// <summary>
    /// Le point indique la position zéro de la valeur de
    /// l'échelle de la zone d'observation actuelle et est utilisé pour déplacer les axes.
    /// </summary>
    public Rect coordinateAxisViewRangeInPixel
    {
        get
        {
            try
            {
                return new Rect(
                    CoordinateToPixel(m_CoordinateAxisViewRange.position - m_CoordinateAxisRange.position),
                    m_CoordinateAxisViewRange.size);
            }
            catch (NullReferenceException e)
            {
                Debug.Log(this + " : " + e);
            }

            return new Rect(CoordinateToPixel(m_CoordinateAxisRange.position),
                m_CoordinateAxisViewRange.size);
        }
    }

    public RectTransform coordinateRectT
    {
        //get { return m_CoordinatePixelRect; }
        get
        {
            try
            {
                return m_CoordinateRectT;
            }
            catch
            {
                return GetComponent<RectTransform>();
            }
        }
    }

    public int lineNum
    {
        get { return m_LineList.Count; }
    }

    #endregion

    #region delegate
    /// <summary>
    /// Créer un délégué avec un type de retour void et deux paramètres d'entrée.
    /// </summary>
    public delegate void CoordinateRectChangeHandler(object sender, DD_CoordinateRectChangeEventArgs e);
    public delegate void CoordinateScaleChangeHandler(object sender, DD_CoordinateScaleChangeEventArgs e);
    public delegate void CoordinateZeroPointChangeHandler(object sender, DD_CoordinateZeroPointChangeEventArgs e);


    public event CoordinateRectChangeHandler CoordinateRectChangeEvent;
    public event CoordinateScaleChangeHandler CoordinateScaleChangeEvent;
    public event CoordinateZeroPointChangeHandler CoordinateeZeroPointChangeEvent;
    #endregion

    protected override void Awake()
    {

        if (null == (m_DataDiagram = GetComponentInParent<DD_DataDiagram>()))
        {
            Debug.Log(this + "Awake Error : null == m_DataDiagram");
            return;
        }

        m_LinesPreb = (GameObject)Resources.Load("Prefabs/Lines");
        if (null == m_LinesPreb)
        {
            Debug.Log("Error : null == m_LinesPreb");
        }

        m_MarkTextPreb = (GameObject)Resources.Load("Prefabs/MarkText");
        if (null == m_MarkTextPreb)
        {
            Debug.Log("Error : null == m_MarkTextPreb");
        }

        try
        {
            m_CoordinateRectT = FindInChild(COORDINATE_RECT).GetComponent<RectTransform>();
            if (null == m_CoordinateRectT)
            {
                Debug.Log("Error : null == m_CoordinateRectT");
                return;
            }
        }
        catch (NullReferenceException e)
        {
            Debug.Log(this + "," + e);
        }


        FindExistMarkText(m_MarkHorizontalTexts);

        GameObject parent = gameObject.transform.parent.gameObject;
        Rect parentRect = parent.GetComponent<RectTransform>().rect;


        m_CoordinateAxisViewRange.position = m_CoordinateAxisRange.position;
        m_CoordinateAxisViewRange.size = new Vector2(1, 1);

        ///Ajout une réponse à un évènement
        m_DataDiagram.RectChangeEvent += OnRectChange;
        m_DataDiagram.ZoomEvent += OnZoom;
        m_DataDiagram.MoveEvent += OnMove;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private GameObject FindInChild(string name)
    {

        foreach (Transform t in transform)
        {
            if (name == t.gameObject.name)
            {
                return t.gameObject;
            }
        }

        return null;
    }

    private void ChangeRect(Rect newRect)
    {

        if (null != CoordinateRectChangeEvent)
            CoordinateRectChangeEvent(this,
                new DD_CoordinateRectChangeEventArgs(new Rect(
                CoordinateToPixel(m_CoordinateAxisRange.position - m_CoordinateAxisViewRange.position),
                newRect.size)));
    }



    /// <summary>
    /// Cette méthode permet de changer les valeurs présentes dans l'axe des abscisses et des ordonnées en fonction du zoom
    /// </summary>
    /// <param name="ZoomX">Varaible de type float désignant la valeur du zoom sur l'axe des abscisses</param>
    /// <param name="ZoomY">Varaible de type float désignant la valeur du zoom sur l'axe des ordonnées</param>
    private void ChangeScale(float ZoomX, float ZoomY)
    {

        Vector2 rangeSize = m_CoordinateAxisRange.size;
        Vector2 viewSize = new Vector2(m_CoordinateAxisViewRange.width * rangeSize.x,
            m_CoordinateAxisViewRange.height * rangeSize.y);

        float YtoXScale = (rangeSize.y / rangeSize.x);
        float zoomXVal = ZoomX * m_ZoomSpeed.x;
        float zoomYVal = (ZoomY * m_ZoomSpeed.y) * YtoXScale;

        viewSize.x += zoomXVal;
        viewSize.y += zoomYVal;

        if (viewSize.x > m_CoordinateAxisMaxWidth)
            viewSize.x = m_CoordinateAxisMaxWidth;

        if (viewSize.x < m_CoordinateAxisMinWidth)
            viewSize.x = m_CoordinateAxisMinWidth;

        if (viewSize.y > m_CoordinateAxisMaxWidth * YtoXScale)
            viewSize.y = m_CoordinateAxisMaxWidth * YtoXScale;

        if (viewSize.y < m_CoordinateAxisMinWidth * YtoXScale)
            viewSize.y = m_CoordinateAxisMinWidth * YtoXScale;

        m_CoordinateAxisViewRange.width = viewSize.x / rangeSize.x;
        m_CoordinateAxisViewRange.height = viewSize.y / rangeSize.y;
    }

    private void OnRectChange(object sender, DD_RectChangeEventArgs e)
    {

        ChangeRect(m_CoordinateRectT.rect);

        ///Met à jour OnPopulateMesh
        UpdateGeometry();
    }


    private void OnZoom(object sender, DD_ZoomEventArgs e)
    {

        if (null != CoordinateScaleChangeEvent)
            CoordinateScaleChangeEvent(this, new DD_CoordinateScaleChangeEventArgs(
                    m_CoordinateAxisViewRange.width, m_CoordinateAxisViewRange.height));

        ChangeScale(e.ZoomX, e.ZoomY);

        ///Met à jour OnPopulateMesh
        UpdateGeometry();
    }

    private void OnMove(object sender, DD_MoveEventArgs e)
    {

        if ((1 > Mathf.Abs(e.MoveX)) && (1 > Mathf.Abs(e.MoveY)))
            return;

        Vector2 coordDis = new Vector2(
            (e.MoveX / m_CoordinateRectT.rect.width) * m_CoordinateAxisViewSizeX,
            (e.MoveY / m_CoordinateRectT.rect.height) * m_CoordinateAxisViewSizeY);

        Vector2 dis = new Vector2(-coordDis.x * m_MoveSpeed.x, -coordDis.y * m_MoveSpeed.y);

        m_CoordinateAxisViewRange.position += dis;
        if (0 > m_CoordinateAxisViewRange.x)
            m_CoordinateAxisViewRange.x = 0;

        if (null != CoordinateeZeroPointChangeEvent)
            CoordinateeZeroPointChangeEvent(this,
                new DD_CoordinateZeroPointChangeEventArgs(CoordinateToPixel(dis)));

        ///Met à jour OnPopulateMesh
        UpdateGeometry();
    }



    private Vector2 CoordinateToPixel(Vector2 coordPoint)
    {

        return new Vector2((coordPoint.x / m_CoordinateAxisRange.width) * m_CoordinateRectT.rect.width,
            (coordPoint.y / m_CoordinateAxisRange.height) * m_CoordinateRectT.rect.height);
    }

    #region draw rect coordinateAxis
    private int CalcMarkNum(float pixelPerMark, float totalPixel)
    {

        return Mathf.CeilToInt(totalPixel / (pixelPerMark > 0 ? pixelPerMark : 1));
    }

    private float CalcMarkLevel(float[] makeTab, int markNum, float viewMarkRange)
    {

        float dis = viewMarkRange / (markNum > 0 ? markNum : 1);
        float markScale = 1;
        float mark = makeTab[0];

        while ((dis < (mark * markScale)) || (dis >= (mark * markScale * 10)))
        {

            if (dis < (mark * markScale))
            {
                markScale /= 10;
            }
            else if (dis >= (mark * markScale * 10))
            {
                markScale *= 10;
            }
            else
            {
                break;
            }
        }

        dis /= markScale;
        for (int i = 1; i < makeTab.Length; i++)
        {
            if (Mathf.Abs(mark - dis) > Mathf.Abs(makeTab[i] - dis))
                mark = makeTab[i];
        }

        return (mark * markScale);
    }

    private float CeilingFormat(float markLevel, float Val)
    {

        return Mathf.CeilToInt(Val / markLevel) * markLevel;
    }

    private float[] CalcMarkVals(float markLevel, float startViewMarkVal, float endViewMarkVal)
    {

        float[] markVals;
        List<float> tempList = new List<float>();
        float tempMarkVal = CeilingFormat(markLevel, startViewMarkVal);

        while (tempMarkVal < endViewMarkVal)
        {
            tempList.Add(tempMarkVal);
            tempMarkVal += markLevel;
        }

        markVals = new float[tempList.Count];
        tempList.CopyTo(markVals);

        return markVals;
    }

    private float MarkValToPixel(float markVal, float startViewMarkVal,
        float endViewMarkVal, float stratCoordPixelVal, float endCoordPixelVal)
    {


        if ((endViewMarkVal <= startViewMarkVal) || (markVal <= startViewMarkVal))
            return stratCoordPixelVal;

        return stratCoordPixelVal +
            ((endCoordPixelVal - stratCoordPixelVal) * ((markVal - startViewMarkVal) / (endViewMarkVal - startViewMarkVal)));
    }

    private float[] MarkValsToPixel(float[] markVals, float startViewMarkVal,
        float endViewMarkVal, float stratCoordPixelVal, float endCoordPixelVal)
    {

        float[] pixelYs = new float[markVals.Length];

        for (int i = 0; i < pixelYs.Length; i++)
        {
            pixelYs[i] = MarkValToPixel(markVals[i], startViewMarkVal,
                endViewMarkVal, stratCoordPixelVal, endCoordPixelVal);
        }

        return pixelYs;
    }

    private void SetMarkText(GameObject markText, Rect rect, string str, bool isEnable)
    {

        if (null == markText)
        {
            Debug.Log("SetMarkText Error : null == markText");
            return;
        }

        RectTransform rectTransform = markText.GetComponent<RectTransform>();
        if (null == rectTransform)
        {
            Debug.Log("SetMarkText Error : null == rectTransform");
            return;
        }

        Text text = markText.GetComponent<Text>();
        if (null == text)
        {
            Debug.Log("SetMarkText Error : null == Text");
            return;
        }

        //Le coin inférieur gauche est le point d'ancrage
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        //L'axe est dans le coin inférieur gauche
        rectTransform.pivot = new Vector2(0, 0);
        //Défini la postion de l'axe par rapport au point d'ancrage
        rectTransform.anchoredPosition = rect.position;
        rectTransform.sizeDelta = rect.size;

        text.text = str;
        text.enabled = isEnable;
    }

    private void ResetMarkText(GameObject markText)
    {

        SetMarkText(markText, new Rect(new Vector2(0, m_CoordinateRectT.rect.y),
            new Vector2(m_CoordinateRectT.rect.x, m_MinMarkTextHeight)), null, false);
    }

    private void ResetAllMarkText(List<GameObject> markTexts)
    {

        if (null == markTexts)
        {
            Debug.Log("DisableAllMarkText Error : null == markTexts");
            return;
        }

        foreach (GameObject g in markTexts)
        {
            ResetMarkText(g);
        }
    }

    private void DrawOneHorizontalMarkText(GameObject markText,
        float markValY, float pixelY, Rect coordinateRect)
    {

        SetMarkText(markText, new Rect(new Vector2(0, pixelY - (m_MinMarkTextHeight / 2)),
            new Vector2(coordinateRect.x - 2, m_MinMarkTextHeight)),
            markValY.ToString(), true);
    }


    private IEnumerator DrawHorizontalTextMark(float[] marksVals, float[] marksPixel, Rect coordinateRect)
    {

        yield return new WaitForSeconds(0);

        while (marksPixel.Length > m_MarkHorizontalTexts.Count)
        {
            GameObject markText = Instantiate(m_MarkTextPreb, transform);
            markText.name = string.Format("{0}{1}", MARK_TEXT_BASE_NAME, m_MarkHorizontalTexts.Count);
            m_MarkHorizontalTexts.Add(markText);
        }

        ResetAllMarkText(m_MarkHorizontalTexts);

        for (int i = 0; i < marksPixel.Length; i++)
        {
            DrawOneHorizontalMarkText(m_MarkHorizontalTexts[i], marksVals[i], marksPixel[i], coordinateRect);
        }

        yield return 0;
    }

    private void DrawOneHorizontalMark(VertexHelper vh, float pixelY, Rect coordinateRect)
    {

        Vector2 startPoint = new Vector2(coordinateRect.x, pixelY);
        Vector2 endPoint = new Vector2(coordinateRect.x + coordinateRect.width, pixelY);

        DrawHorizontalSegmet(vh, startPoint, endPoint, m_MarkColor, m_RectThickness / 2);
    }

    private void DrawHorizontalMark(VertexHelper vh, Rect coordinateRect)
    {

        int markNum = CalcMarkNum(m_PixelPerMark, coordinateRect.height);

        float markLevel = CalcMarkLevel(MarkIntervalTab, markNum, m_CoordinateAxisViewSizeY);

        float[] marksVals = CalcMarkVals(markLevel, m_CoordinateAxisViewRange.y,
            m_CoordinateAxisViewRange.y + m_CoordinateAxisViewSizeY);

        float[] marksPixel = MarkValsToPixel(marksVals, m_CoordinateAxisViewRange.y,
            m_CoordinateAxisViewRange.y + m_CoordinateAxisViewSizeY,
            coordinateRect.y, coordinateRect.y + coordinateRect.height);

        for (int i = 0; i < marksPixel.Length; i++)
        {
            DrawOneHorizontalMark(vh, marksPixel[i], coordinateRect);
        }

        StartCoroutine(DrawHorizontalTextMark(marksVals, marksPixel, coordinateRect));
    }

    private void DrawRect(VertexHelper vh, Rect rect)
    {

        DrawRectang(vh, rect.position, new Vector2(rect.x, rect.y + rect.height),
            new Vector2(rect.x + rect.width, rect.y + rect.height),
            new Vector2(rect.x + rect.width, rect.y), m_BackgroundColor);

    }

    private void DrawRectCoordinate(VertexHelper vh)
    {

        Rect marksRect = new Rect(m_CoordinateRectT.offsetMin, m_CoordinateRectT.rect.size);

        DrawRect(vh, new Rect(marksRect));

        DrawHorizontalMark(vh, marksRect);
    }

    /// <summary>
    /// Méthode qui vérifie si le contrôle d'interface utilisateur de texte à l'échelle a été instancié
    /// à la coordonnée actuelle avant chaque exécution.
    /// Dans le cas où, il existe déjà, il est ajouté à la file d'attente pour être utilisé en premier.
    /// </summary>
    private void FindExistMarkText(List<GameObject> markTexts)
    {

        //Transform tempTrans = null;
        foreach (Transform t in transform)
        {
            if (Regex.IsMatch(t.gameObject.name, MARK_TEXT_BASE_NAME))
            {
                t.gameObject.name = string.Format("{0}{1}", MARK_TEXT_BASE_NAME, m_MarkHorizontalTexts.Count);
                markTexts.Add(t.gameObject);
            }

        }
    }
    #endregion

    protected override void OnPopulateMesh(VertexHelper vh)
    {

        vh.Clear();
        DrawRectCoordinate(vh);
    }

    /// <summary>
    /// Cette méthode permet d'entrée une donnée sur le graphe.
    /// </summary>
    /// <param name="line"> La courbe retournée par la méthode AddLine(). </param>
    /// <param name="point">
    /// point.x est la valeur de mise à l'échelle de la courbe sur l'axe des abscisses, qui vaut 1 si 
    /// il n'y a pas de mise à l'échelle, point.y est la valeur de la donnée d'entrée.
    /// </param>
    public void InputPoint(GameObject line, Vector2 point)
    {

        line.GetComponent<DD_Lines>().AddPoint(CoordinateToPixel(point));
    }

    public GameObject AddLine(string name)
    {

        if (null == m_LinesPreb)
            m_LinesPreb = (GameObject)Resources.Load("Prefabs/Lines");

        try
        {
            m_LineList.Add(Instantiate(m_LinesPreb, m_CoordinateRectT));
        }
        catch (NullReferenceException e)
        {
            Debug.Log(this + "," + e);
            return null;
        }

        m_LineList[m_LineList.Count - 1].GetComponent<DD_Lines>().lineName = name;
        m_LineList[m_LineList.Count - 1].GetComponent<DD_Lines>().color = Color.yellow;
        m_LineList[m_LineList.Count - 1].name = String.Format("{0}{1}", LINES_BASE_NAME,
            m_LineList[m_LineList.Count - 1].GetComponent<DD_Lines>().lineName);

        return m_LineList[m_LineList.Count - 1];
    }

    public bool RemoveLine(GameObject line)
    {

        if (null == line)
            return true;

        if (false == m_LineList.Remove(line))
            return false;

        try
        {
            line.GetComponent<DD_Lines>().Clear();
        }
        catch (NullReferenceException)
        {

        }

        Destroy(line);

        return true;
    }

}
