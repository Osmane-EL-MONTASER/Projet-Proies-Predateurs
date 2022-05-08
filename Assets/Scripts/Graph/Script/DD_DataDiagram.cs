using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Classe reprise par HAMICHE Bilal le 29/04, elle permet 
/// Seuls les rectangles sont gérés par RectTransform.
/// 0 est le point d'angle inférieur gauche et 2 le point d'angle supérieur droit.
/// Comme il s'agit d'un rectangle, les valeurs maximale et minimale doivent se produire à ces deux points.
/// Tous les rectangles Rect à l'intérieur, avec le coin inférieur droit de la fenêtre parent comme référence.
/// rect.position est le décalage du coin inférieur gauche de cet objet par rapport au coin inférieur gauche de la fenêtre parent.
/// </summary>
public class DD_CalcRectTransformHelper {

    public static Vector2 CalcAnchorPointPosition(Vector2 anchorMin, Vector2 anchorMax, 
        Vector2 parentSize, Vector2 pivot) {

        Vector2 pos = new Vector2(parentSize.x * anchorMin.x, parentSize.y * anchorMin.y);
        Vector2 size = new Vector2(parentSize.x * anchorMax.x - pos.x,
            parentSize.y * anchorMax.y - pos.y);

        return pos + new Vector2(size.x * pivot.x, size.y * pivot.y);
    }

    public static Vector2 CalcAnchorPosition(Rect rect, Vector2 anchorMin,
        Vector2 anchorMax, Vector2 parentSize, Vector2 pivot) {

        Vector2 anchor = CalcAnchorPointPosition(anchorMin, anchorMax, parentSize, pivot);
        Vector2 pivotPos = new Vector2(rect.x + rect.width * pivot.x,
            rect.y + rect.height * pivot.y);

        return pivotPos - anchor;
    }

    public static Vector2 CalcOffsetMin(Rect rect, Vector2 anchorMin,
        Vector2 anchorMax, Vector2 parentSize) {

        Vector2 anchor0 = new Vector2(parentSize.x * anchorMin.x, parentSize.y * anchorMin.y);
        Vector2 point0 = new Vector2(rect.x, rect.y);

        return point0 - anchor0;
    }

    public static Vector2 CalcOffsetMax(Rect rect, Vector2 anchorMin,
        Vector2 anchorMax, Vector2 parentSize) {

        Vector2 anchor2 = new Vector2(parentSize.x * anchorMax.x, parentSize.y * anchorMax.y);
        Vector2 point2 = new Vector2(rect.x + rect.width, rect.y + rect.height);

        return point2 - anchor2;
    }

    public static Vector2 CalcSizeDelta(Rect rect, Vector2 anchorMin,
        Vector2 anchorMax, Vector2 parentSize) {

        return (CalcOffsetMax(rect, anchorMin, anchorMax, parentSize) - 
            CalcOffsetMin(rect, anchorMin, anchorMax, parentSize));
    }

    public static Vector2 CalcRectSize(Vector2 sizeDelta, Vector2 anchorMin,
        Vector2 anchorMax, Vector2 parentSize) {

        Vector2 anchor0 = new Vector2(parentSize.x * anchorMin.x, parentSize.y * anchorMin.y);
        Vector2 anchor2 = new Vector2(parentSize.x * anchorMax.x, parentSize.y * anchorMax.y);

        return anchor2 - anchor0 + sizeDelta;
    }

    /// <summary>
    /// La position du Rect est le coin inférieur gauche de cette fenêtre par rapport au coin inférieur gauche de la fenêtre parent.
    /// </summary>
    /// <returns>Le Rect retourné est le Rect de cette fenêtre.</returns>
    public static Rect CalcLocalRect(Vector2 anchorMin, Vector2 anchorMax, Vector2 parentSize, 
        Vector2 pivot, Vector2 anchorPosition, Rect rectInRT) {

        Vector2 anchor = CalcAnchorPointPosition(anchorMin, anchorMax, parentSize, pivot);
        Vector2 pivotPos = anchor + anchorPosition;

        return new Rect(pivotPos + rectInRT.position, rectInRT.size);
    }
}

public class DD_RectChangeEventArgs : EventArgs {

    private readonly Vector2 m_Size;

    public DD_RectChangeEventArgs(Vector2 size) {
        m_Size = size;
    }

    public Vector2 size {
        get { return m_Size; }
    }
}


/// <summary>
/// Classe reprise par HAMICHE Bilal le 29/04, elle per
/// </summary>
public class DD_ZoomEventArgs : EventArgs {

    private float _zoomX;
    private float _zoomY;

    public DD_ZoomEventArgs(float valX, float valY) : base() {
        this._zoomX = valX;
        this._zoomY = valY;
    }

    public float ZoomX {
        get {
            return _zoomX;
        }
    }

    public float ZoomY {
        get {
            return _zoomY;
        }
    }
}

public class DD_MoveEventArgs : EventArgs {

    private float _moveX = 0;
    private float _moveY = 0;

    public DD_MoveEventArgs(float dx, float dy) {

        _moveX = dx;
        _moveY = dy;
    }

    public float MoveX {
        get {
            return _moveX;
        }
    }

    public float MoveY {
        get {
            return _moveY;
        }
    }
}

/// <summary>
/// Classe permettant de gérer les évenement lié à la suppression d'une courbe
/// </summary>
public class DD_PreDestroyLineEventArgs : EventArgs {

    GameObject m_Line = null;

    /// <summary>
    /// Constructeur de la classe DD_PreDestroyLineEventArgs
    /// permettant de généré un événement avant qu'une courbe
    /// soit détruite, et cette événement indiquera quellle courbe sera détruite.
    /// </summary>
    /// <param name="line">Variable de type GameObject désignant la courbe a détruire</param>
    public DD_PreDestroyLineEventArgs(GameObject line) {

        m_Line = null;

        if (null == line)
            return;

        if (null == line.GetComponent<DD_Lines>())
            return;

        m_Line = line;
    }

    public GameObject line {
        get { return m_Line; }
    }
}
/// <summary>
/// 
/// </summary>
public class DD_DataDiagram : MonoBehaviour , IScrollHandler, IDragHandler {

    private readonly Vector2 MinRectSize = new Vector2(100, 80);

    private GameObject m_CoordinateAxis;
    private GameObject lineButtonsContent;


    // Créer un délégué avec un type de retour de void et deux paramètres
    public delegate void RectChangeHandler(object sender, DD_RectChangeEventArgs e);
    public delegate void ZoomHandler(object sender, DD_ZoomEventArgs e);
    public delegate void MoveHandler(object sender, DD_MoveEventArgs e);
    public delegate void PreDestroyLineHandler(object sender, DD_PreDestroyLineEventArgs e);
    
    public event RectChangeHandler RectChangeEvent;
    public event ZoomHandler ZoomEvent;
    public event MoveHandler MoveEvent;
    public event PreDestroyLineHandler PreDestroyLineEvent;

    #region config
    //Nombre maximal de courbe affiché sur le graphe en meme temps
    public int maxLineNum = 5;

    #region used in DD_Lines
    
    //Nombre maximal de point sur la meme courbe, si cette valeur est dépassé, la valeur la plus ancienne est supprimée
    public int m_MaxPointNum = 65535;
    #endregion

    //为了避免需要在CoordinateAxis中进行设置，所以移到这里，本来应该是在CoordinateAxis内
    #region used in DD_CoordinateAxis
    /// <summary>
    /// 矩形框式坐标轴刻度的间距，以公分（CM）为单位 ééééééééééééééééééééééééééééééééééééééééééééé
    /// La distance entre deux valeurs en ordonnées 
    /// </summary>
    public float m_CentimeterPerMark = 2f;

    /// <summary>
    /// Longueur physique par unité de longueur sur l'axe des abscisses standard (non échelonné)
    /// Longueur de l'axe des abscisses en "secondes", longueur physique en "centimètres".
    /// </summary>
    public float m_CentimeterPerCoordUnitX = 2f;

    /// <summary>
    /// Longueur physique par unité de longueur sur l'axe des ordonnées standard (non échelonné)
    /// Longueur physique de l'axe des ordonnées "centimètres".
    /// </summary>
    public float m_CentimeterPerCoordUnitY = 2f;
    #endregion

    #endregion

    //Verifie avant si Rect existr
    public Rect? rect {
        get {
            RectTransform rectT = gameObject.GetComponent<RectTransform>();
            if (null == rectT)
                return null;

            return rectT.rect;
        }
        set {
            Rect rect = value.Value;
            if (MinRectSize.x > rect.width)
                rect.width = MinRectSize.x;
            if (MinRectSize.y > rect.height)
                rect.height = MinRectSize.y;

            RectTransform rectT = gameObject.GetComponent<RectTransform>();
            if (null == rectT)
                return ;

            rectT.anchoredPosition = DD_CalcRectTransformHelper.CalcAnchorPosition(rect,
                rectT.anchorMin, rectT.anchorMax, transform.parent.GetComponentInParent<RectTransform>().rect.size,
                rectT.pivot);
            rectT.sizeDelta = DD_CalcRectTransformHelper.CalcSizeDelta(rect, 
                rectT.anchorMin, rectT.anchorMax, transform.parent.GetComponentInParent<RectTransform>().rect.size);

            if (null != RectChangeEvent)
                RectChangeEvent(this, new DD_RectChangeEventArgs(rect.size));
        }
    }

    private void Awake() {

        DD_CoordinateAxis coordinateAxis = transform.GetComponentInChildren<DD_CoordinateAxis>();
        if (null == coordinateAxis) {
            m_CoordinateAxis = Instantiate((GameObject)Resources.Load("Prefabs/CoordinateAxis"), gameObject.transform);
            m_CoordinateAxis.name = "CoordinateAxis";
        } else {
            m_CoordinateAxis = coordinateAxis.gameObject;
        }

        DD_LineButtonsContent tempObject = GetComponentInChildren<DD_LineButtonsContent>();
        if(null == tempObject) {
            Debug.LogWarning(this + "Awake Error : null == lineButtonsContent");
            return;
        } else {
            if (null == (lineButtonsContent = tempObject.gameObject)) {
                Debug.LogWarning(this + "Awake Error : null == lineButtonsContent");
                return;
            }
        }
    }
    //// Use this for initialization
    void Start() {

        if (null != RectChangeEvent) {
            try {
                RectChangeEvent(this, new DD_RectChangeEventArgs(gameObject.GetComponent<RectTransform>().rect.size));
            } catch (NullReferenceException e) {
                Debug.LogWarning(e);
            }
        }
    }

    // Update is called once per frame
    void Update () {

    }

    public void OnDrag(PointerEventData eventData) {
        //print(eventData);
        MoveEvent(this, new DD_MoveEventArgs(eventData.delta.x, eventData.delta.y));
    }

    /// <summary>
    /// Méthode permettant de faire défiler la fenetr lorsque l'on bouge la molette de la souris
    /// </summary>
    /// <param name="eventData"> Variable de type PointerEventData qui vérifie si le pointeur de la souris
    /// est sur la fenetre </param>
    public void OnScroll(PointerEventData eventData) {
        
        if (true == Input.GetMouseButton(0)) {
            ZoomEvent(this, new DD_ZoomEventArgs(-eventData.scrollDelta.y, 0));
        } else if (true == Input.GetMouseButton(1)) {
            ZoomEvent(this, new DD_ZoomEventArgs(0, eventData.scrollDelta.y));
        } else {
            ZoomEvent(this, new DD_ZoomEventArgs(-eventData.scrollDelta.y, -eventData.scrollDelta.y));
        }
    }

    private void SetLineButtonColor(GameObject line, Color color) {

        foreach (Transform t in lineButtonsContent.transform) {
            if (line == t.gameObject.GetComponent<DD_LineButton>().line) {
            
                t.gameObject.GetComponent<DD_LineButton>().line = line;
                return;
            }
        }
    }
    /// <summary>
    /// Méthode permettant d'attribuer une couleur pour une courbe donnée
    /// </summary>
    /// <param name="line">Variable de type GameObject désignant la courbe</param>
    /// <param name="color">Variable de type Color représenant la couleur que l'on souhaite attribuer à la courbe</param>
    private void SetLineColor(GameObject line, Color color) {
        //Vérifie que la courbe existe bien
        if (null == line) {
            //Debug.logger(this.ToString() + " SetLineColor error : null == line");
            return;
        }

        DD_Lines lines = line.GetComponent<DD_Lines>();
        if (null == lines) {
            Debug.LogWarning(line.ToString() + " SetLineColor error : null == lines");
            return;
        }

        lines.color = color;

        SetLineButtonColor(line, color);
    }


    /// <summary>
    /// Méthode permettant de vérifier sd'ajouter un ou des bouton(s) pour le/les courbes présentes sur le grapheééééééééééééééééééééééééééééééééé
    /// </summary>
    /// <param name="line"> Varaible de type GameObject qui représente la corube </param>
    /// <returns></returns>
    private bool AddLineButton(GameObject line) {

        if (null == lineButtonsContent) {
            Debug.LogWarning(this + "AddLineButton Error : null == lineButtonsContent");
            return false;
        }

        if (lineButtonsContent.transform.childCount >= maxLineNum)
            return false;

        if (null == line) {
            Debug.LogWarning(this + "AddLineButton Error : null == line");
            return false;
        }

        DD_Lines lines = line.GetComponent<DD_Lines>();
        if (null == lines) {
            Debug.LogWarning(this + "AddLineButton Error : null == lines");
            return false;
        }

        GameObject button = Instantiate((GameObject)Resources.Load("Prefabs/LineButton"), lineButtonsContent.transform);
        if (null == button) {
            Debug.LogWarning(this + "AddLineButton Error : null == button");
            return false;
        }

        button.GetComponent<DD_LineButton>().line = line;

        return true;
    }

    /// <summary>
    /// Classe permettant de vérifier si un boutton lié à une courbe est supprimé ou non
    /// </summary>
    /// <param name="line"> Variable de type GameObject désignant la courbe</param>
    /// <returns></returns>
    private bool DestroyLineButton(GameObject line) {
        //Vérifie si le bouton existe ou pas
        if (null == lineButtonsContent) {
            Debug.Log(this + "AddLineButton Error : null == lineButtonsContent");
            return false;
        }

        foreach (Transform t in lineButtonsContent.transform) {
            try {
                if (line == t.gameObject.GetComponent<DD_LineButton>().line) {
                    t.gameObject.GetComponent<DD_LineButton>().DestroyLineButton();
                    Destroy(t.gameObject);
                    return true;
                }
            } catch (NullReferenceException) {
                return false;
            }
        }

        return false;
    }


    /// <summary>
    /// Cette méthode permet d'entrée une donnée sur le graphe
    /// </summary>
    /// <param name="line"> La courbe retournée par la méthode AddLine() </param>
    /// <param name="point">point.x est la valeur de mise à l'échelle de la courbe sur l'axe des abscisses, qui vaut 1 si 
    /// il n'y a pas de mise à l'échelle, point.y est la valeur de la donnée d'entrée</param>
    public void InputPoint(GameObject line, Vector2 point) {

        DD_CoordinateAxis coordinate = m_CoordinateAxis.GetComponent<DD_CoordinateAxis>();
        coordinate.InputPoint(line, point);
    }


    public GameObject AddLine(string name) {

        DD_CoordinateAxis coordinate = m_CoordinateAxis.GetComponent<DD_CoordinateAxis>();

        if(coordinate.lineNum >= maxLineNum) {
            print("coordinate.lineNum > maxLineNum");
            return null;
        }

        if(coordinate.lineNum != lineButtonsContent.transform.childCount) {
            print("coordinate.lineNum != m_LineButtonList.Count");
        }
            
        GameObject line = coordinate.AddLine(name);

        if(false == AddLineButton(line)) {
            if(false == coordinate.RemoveLine(line)) {
                print(this.ToString() + " AddLine error : false == coordinate.RemoveLine(line)");
            }
            line = null;
        }

        return line;
    }

    /// <summary>
    /// Cette méthode permet de créer une nouvelle courbe, elle retourne une varaible de type GameObject
    /// qui est la nouvelle courbe du graphe.
    /// <param name="name"> Le nom de la nouvelle courbe </param>
    /// <param name="color"> La couleur de la nouvelle courbe et du bouton de cette meme courbe </param>
    /// </summary>
    public GameObject AddLine(string name, Color color) {

        GameObject line = AddLine(name);

        SetLineColor(line, color);

        return line;
    }

    public bool DestroyLine(GameObject line) {

        if (null != PreDestroyLineEvent)
            PreDestroyLineEvent(this, new DD_PreDestroyLineEventArgs(line));

        if (false == DestroyLineButton(line))
            return false;

        try {
            if (false == m_CoordinateAxis.GetComponent<DD_CoordinateAxis>().RemoveLine(line))
                return false;
        } catch (NullReferenceException) {
            return false;
        }

        return true;
    }
}

