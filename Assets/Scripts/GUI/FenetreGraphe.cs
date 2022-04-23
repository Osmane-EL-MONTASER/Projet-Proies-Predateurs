using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;


/// <summary>
/// Classe codée par Bilal HAMICHE pour le prototype. Elle permet la création de graphes dynamiques. Elle
/// est insiprée d'une série de vidéos expliquant comment créer un graphe sur Unity 3D 
/// https://www.youtube.com/playlist?list=PLzDRvYVwl53v5ur4GluoabyckImZz3TVQ
/// </summary>
public class FenetreGraphe : MonoBehaviour
{
	/// <summary>
	/// 
	/// </summary>
	private static FenetreGraphe instance;

	/// <summary>
	/// Les points sur le graphe
	/// </summary>
	[SerializeField] private Sprite _dotSprite;


	/// <summary>
	/// Repreésente le fond du graphe
	/// </summary>
	private RectTransform _graphContainer;

	/// <summary>
	/// Affiche les valeur en ordonnées
	/// </summary>
	private RectTransform _labelTemplateX;

	/// <summary>
	/// Affiche les valeur en abscisse
	/// </summary>
	private RectTransform _labelTemplateY;

	/// <summary>
	/// Représente la ligne en abscise du graphe
	/// </summary>
	private RectTransform _dashTemplateX;

	/// <summary>
	/// Représente la ligne en ordonnées du graphe
	/// </summary>
	private RectTransform _dashTemplateY;

	/// <summary>
	/// Liste contenant les GameObject
	/// </summary>
	private List<GameObject> _gameObjectList;

	/// <summary>
	/// Représente les GameObject des infos bulles
	/// </summary>
	private GameObject _tooltipGameObject;

	/// <summary>
	/// Liste des valeurs en ordonnées du graphe
	/// </summary>
	private List<int> valueList;

	/// <summary>
	/// 
	/// </summary>
	private IGraphVisual graphVisual;

	/// <summary>
	/// Nombre maximum de valeur visible sur le graphe
	/// </summary>
	private int maxVisibleValueAmount;


	private Func<int, string> getAxisLabelX;
	private Func<float, string> getAxisLabelY;


	private void Awake()
	{


		instance = this;
		_graphContainer = transform.Find("_graphContainer").GetComponent<RectTransform>();
		_labelTemplateX = _graphContainer.Find("_labelTemplateX").GetComponent<RectTransform>();
		_labelTemplateY = _graphContainer.Find("_labelTemplateY").GetComponent<RectTransform>();
		_dashTemplateX = _graphContainer.Find("_labelTemplateX").GetComponent<RectTransform>();
		_dashTemplateY = _graphContainer.Find("_labelTemplateY").GetComponent<RectTransform>();
		_tooltipGameObject = _graphContainer.Find("tooltip").gameObject;
		_gameObjectList = new List<GameObject>();


		/* idee : recuper nbloup, nblapin, ... depuis la classe agent
		 * et faire List<int> valueList = new List<int>() { nbloup, nblapin, ... };
		 * ou
		 * faire 3 graphe (un pred, un proi et un autotrophe) avec bouton changement graphe par types
		 * 
		 * ou alors faire 1 graphe avec 3 val : prédateur, proies et autotrophes
		 * */



		//valeur a afficher le graphe (penser a prendre compteur agent)
		List<int> valueList = new List<int>() { 4, 7, 9, 8, 11, 2, 4, 8, 1, 4, 9, 12, 1, 2, 6 };
		IGraphVisual lineGraphVisual = new LineGraphVisual(_graphContainer, _dotSprite, Color.green, new Color(1, 1, 1, .5f));
		IGraphVisual barGraphVisual = new BarChartVisual(_graphContainer, Color.green, .8f);
		ShowGraph(valueList, lineGraphVisual, -1, (int _i) => "Prédateur " + (_i + 1), (float _f) => Mathf.RoundToInt(_f) + ""); // utile pour placer nb agent en y et en x le temps


		/// <summary>
		/// Cela permet d'afficher l'histogramme quand l'utilisateur clique sur l'icone qui correspond
		/// </summary>
		transform.Find("barChartBtn").GetComponent<Button_UI>().ClickFunc = () => {
			SetGraphVisual(barGraphVisual);
		};

		/// <summary>
		/// Cela permet d'afficher le graphe quand l'utilisateur clique sur l'icone qui correspond
		/// </summary>
		transform.Find("lineGraphBtn").GetComponent<Button_UI>().ClickFunc = () => {
			SetGraphVisual(lineGraphVisual);
		};

		/// <summary>
		/// Cela permet de réduire le nombre de valeur en abscisse quand l'utilisateur clique sur l'icone moins
		/// </summary>
		transform.Find("descreaseVisibleAmountBtn").GetComponent<Button_UI>().ClickFunc = () => {
			DecreaseVisibleAmount();
		};

		/// <summary>
		/// Cela permet d'augmenter le nombre de valeur en abscisse quand l'utilisateur clique sur l'icone plus
		/// </summary>
		transform.Find("increaseVisibleAmountBtn").GetComponent<Button_UI>().ClickFunc = () => {
			IncreaseVisibleAmount();
		};

		/// <summary>
		/// Cela permet d'afficher l'info bulle
		/// </summary>
		ShowTootlip("info bulle", new Vector2(100, 100));
	}

	public static void ShowTooltip_Static(string tooltipText, Vector2 anchoredPosition)
	{
		instance.ShowTootlip(tooltipText, anchoredPosition);

	}

	/// <summary>
	/// Méthode permettant d'afficher les infos bulles lorsqu'on passe la souris sur les points ou les barres
	/// </summary>
	private void ShowTootlip(string tooltipText, Vector2 anchoredPosition)
	{
		_tooltipGameObject.SetActive(true);


		_tooltipGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
		Text tootlipUIText = _tooltipGameObject.transform.Find("text").GetComponent<Text>();
		tootlipUIText.text = tooltipText;

		float textPaddingSize = 4f;
		Vector2 backgroundSize = new Vector2(
			_tooltipGameObject.transform.Find("text").GetComponent<Text>().preferredWidth + textPaddingSize * 2f,
			_tooltipGameObject.transform.Find("text").GetComponent<Text>().preferredHeight + textPaddingSize * 2f
			);

		_tooltipGameObject.transform.Find("background").GetComponent<RectTransform>().sizeDelta = backgroundSize;

		_tooltipGameObject.transform.SetAsLastSibling();
	}

	public static void HideTooltip_Static()
	{
		instance.HideTootlip();

	}

	private void HideTootlip()
	{
		_tooltipGameObject.SetActive(false);
	}

	private void SetGetAxislabelX(Func<int, string> getAxisLabelX)
	{
		ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount, getAxisLabelX, this.getAxisLabelY);
	}
	private void SetGetAxislabelY(Func<float, string> getAxisLabelY)
	{
		ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount, this.getAxisLabelX, getAxisLabelY);
	}
	/// <summary>
	/// Méthode permettant d'augmenter le nombre de valeur en abscisse 
	/// </summary>
	private void IncreaseVisibleAmount()
	{
		ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount + 1, this.getAxisLabelX, this.getAxisLabelY);
	}

	/// <summary>
	///Méthode permettant de réduire le nombre de valeur en abscisse quand l'utilisateur clique sur l'icone plus
	/// </summary>
	private void DecreaseVisibleAmount()
	{
		ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount - 1, this.getAxisLabelX, this.getAxisLabelY);
	}
	private void SetGraphVisual(IGraphVisual graphVisual)
	{
		ShowGraph(this.valueList, graphVisual, this.maxVisibleValueAmount, this.getAxisLabelX, this.getAxisLabelY);


	}
	/// <summary>
	/// Méthode permettant d'afficher le graphe et l'histogramme
	/// </summary>
	private void ShowGraph(List<int> valueList, IGraphVisual graphVisual, int maxVisibleValueAmount = -1, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
	{
		this.valueList = valueList;
		this.graphVisual = graphVisual;
		this.getAxisLabelX = getAxisLabelX;
		this.getAxisLabelY = getAxisLabelY;

		if (maxVisibleValueAmount <= 0)
		{
			maxVisibleValueAmount = valueList.Count;
		}
		if (maxVisibleValueAmount > valueList.Count)
		{
			maxVisibleValueAmount = valueList.Count;
		}
		this.maxVisibleValueAmount = maxVisibleValueAmount;

		if (getAxisLabelX == null)
		{
			getAxisLabelX = delegate (int _i) { return _i.ToString(); };
		}


		if (getAxisLabelY == null)
		{
			getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
		}

		if (maxVisibleValueAmount <= 0)
		{
			maxVisibleValueAmount = valueList.Count;
		}

		foreach (GameObject gameObject in _gameObjectList)
		{
			Destroy(gameObject);
		}

		_gameObjectList.Clear();

		/// <summary>
		/// Cela permet de régler la largeur du graphe en fonction du nombre de valeurs en ordonnées et ainsi éviter d'avoir des valeurs en dehors du _graphContainer
		/// </summary>
		float graphWidth = _graphContainer.sizeDelta.x;

		/// <summary>
		/// Cela permet de régler la hauteur du graphe en fonction du nombre de valeurs en abscisses et ainsi éviter d'avoir des valeurs en dehors du _graphContainer
		/// </summary>
		float graphHeight = _graphContainer.sizeDelta.y;


		float yMaximum = valueList[0]; //ecart entre valeur y genre la ta 0 puis 100 puis 200 si = 100f
		float yMinimum = valueList[0];

		for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
		{
			int value = valueList[i];
			if (value > yMaximum)
			{
				yMaximum = value;
			}
			if (value < yMinimum)
			{
				yMinimum = value;
			}
		}
		float yDifference = yMaximum - yMinimum;

		if (yDifference <= 0)
		{
			yDifference = 5f;

		}

		/// <summary>
		/// Cela permet de laisser un ecart entre la valeur maximal en ordonnées  et le sommet du graphe
		/// </summary>
		yMaximum = yMaximum + (yDifference * 0.2f);

		/// <summary>
		/// Cela permet de laisser un ecart entre la valeur minimal en ordonnées et le bas du graphe
		/// </summary>
		yMinimum = yMinimum - (yDifference * 0.2f);


		/// <summary>
		/// On initialise yMinimum a 0f pour qu'il commence a 0
		/// </summary>
		yMinimum = 0f;

		//float xSize = 50f; //distance entre chque point

		/// <summary>
		/// Cela permet de rendre x dynamique
		/// </summary>
		float xSize = graphWidth / (maxVisibleValueAmount + 1);


		int xIndex = 0;


		for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
		{
			float xPosition = xSize + xIndex * xSize;
			float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;

			string tooltIpText = getAxisLabelY(valueList[i]);
			_gameObjectList.AddRange(graphVisual.AddGraphVisual(new Vector2(xPosition, yPosition), xSize, tooltIpText));


			RectTransform labelX = Instantiate(_labelTemplateX);
			labelX.SetParent(_graphContainer, false);
			labelX.gameObject.SetActive(true);
			labelX.anchoredPosition = new Vector2(xPosition, -7f); //placement des 0 1 2 3 4 en dessous de la ligne x
			labelX.GetComponent<Text>().text = getAxisLabelX(i);
			_gameObjectList.Add(labelX.gameObject);

			RectTransform dashX = Instantiate(_dashTemplateX);
			dashX.SetParent(_graphContainer);
			dashX.gameObject.SetActive(true);
			dashX.anchoredPosition = new Vector2(xPosition, -7f); // placement des  valeur 100 en x en dessosu de chaque point
			_gameObjectList.Add(dashX.gameObject);

			xIndex++;
		}


		/// <summary>
		/// Nombre de valeurs sur l'axe des ordonnées
		/// </summary>
		int separatorCount = 10;
		for (int i = 0; i <= separatorCount; i++)
		{
			RectTransform labelY = Instantiate(_labelTemplateY);
			labelY.SetParent(_graphContainer, false);
			labelY.gameObject.SetActive(true);
			float normalizedValue = i * 1f / separatorCount;
			labelY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
			labelY.GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
			_gameObjectList.Add(labelY.gameObject);



			RectTransform dashY = Instantiate(_dashTemplateY);
			dashY.SetParent(_graphContainer, false);
			dashY.gameObject.SetActive(true);
			dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
			_gameObjectList.Add(dashY.gameObject);
		}
	}



	/// <summary>
	///Interface permettant de passer du graphe à l'histogramme, elle est implémenté par la classe BarChartVisual et LineGraphVisual
	/// </summary>
	private interface IGraphVisual
	{
		List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth, string tooltipText);

	}

	/// <summary>
	/// Classe qui contient toutes les méthodes propres à l'histogramme, elle implémente l'interface IGraphVisual
	/// </summary>
	private class BarChartVisual : IGraphVisual
	{

		private RectTransform _graphContainer;

		/// <summary>
		/// La couleur des barres de l'histogramme
		/// </summary>
		private Color barColor;
		private float barWidthMultiplier;


		/// <summary>
		/// Constructeur de la classe BarChartVisual
		/// </summary>
		public BarChartVisual(RectTransform _graphContainer, Color barColor, float barWidthMultiplier)
		{
			this._graphContainer = _graphContainer;
			this.barColor = barColor;
			this.barWidthMultiplier = barWidthMultiplier;

		}

		public List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth, string tooltipText)
		{
			GameObject barGameObject = CreateBar(graphPosition, graphPositionWidth * 0.9f);
			Button_UI barButtonUI = barGameObject.AddComponent<Button_UI>();

			barButtonUI.MouseOverOnceFunc += () =>
			{
				ShowTooltip_Static(tooltipText, graphPosition);

			};

			barButtonUI.MouseOverOnceFunc += () =>
			{
				HideTooltip_Static();

			};


			return new List<GameObject>() { barGameObject };
		}

		/// <summary>
		/// Méthode pour créer des barres sur l'histogramme
		/// </summary>
		private GameObject CreateBar(Vector2 graphPosition, float barWidth)
		{
			GameObject gameObject = new GameObject("bar", typeof(Image));
			gameObject.transform.SetParent(_graphContainer, false);
			gameObject.GetComponent<Image>().color = barColor;
			RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
			rectTransform.sizeDelta = new Vector2(barWidth * barWidthMultiplier, graphPosition.y);
			rectTransform.anchorMin = new Vector2(0, 0);
			rectTransform.anchorMax = new Vector2(0, 0);
			rectTransform.pivot = new Vector2(0.5f, 0f);

			return gameObject;
		}
	}


	/// <summary>
	/// Classe qui contient toutes les méthodes propres au graphe, elle implémente l'interface IGraphVisual
	/// </summary>
	private class LineGraphVisual : IGraphVisual
	{

		private RectTransform _graphContainer;
		private Sprite _dotSprite;
		private GameObject lastDotGameObject;
		private Color dotColor;
		private Color dotConnectionColor;

		public LineGraphVisual(RectTransform _graphContainer, Sprite _dotSprite, Color dotColor, Color dotConnectionColor)
		{
			this._graphContainer = _graphContainer;
			this._dotSprite = _dotSprite;
			this.dotColor = dotColor;
			this.dotConnectionColor = dotConnectionColor;
			lastDotGameObject = null;

		}


		public List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth, string tooltipText)
		{
			List<GameObject> _gameObjectList = new List<GameObject>();
			GameObject dotGameObject = CreateDot(graphPosition);

			Button_UI dotButtonUI = dotGameObject.AddComponent<Button_UI>();

			dotButtonUI.MouseOverOnceFunc += () =>
			{
				ShowTooltip_Static(tooltipText, graphPosition);

			};

			dotButtonUI.MouseOverOnceFunc += () =>
			{
				HideTooltip_Static();

			};

			_gameObjectList.Add(dotGameObject);
			if (lastDotGameObject != null)
			{
				GameObject dotConnectionGameObject = CreateDotConnection(lastDotGameObject.GetComponent<RectTransform>().anchoredPosition, dotGameObject.GetComponent<RectTransform>().anchoredPosition);
				_gameObjectList.Add(dotConnectionGameObject);
			}
			lastDotGameObject = dotGameObject;
			return _gameObjectList;
		}





		/// <summary>
		/// Méthode pour créer des points
		/// </summary>
		private GameObject CreateDot(Vector2 anchoredPosition)
		{
			GameObject gameObject = new GameObject("dot", typeof(Image));
			gameObject.transform.SetParent(_graphContainer, false);
			gameObject.GetComponent<Image>().sprite = _dotSprite;
			gameObject.GetComponent<Image>().color = dotColor;

			RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = anchoredPosition;
			rectTransform.sizeDelta = new Vector2(11, 11); // taille des points
			rectTransform.anchorMin = new Vector2(0, 0);
			rectTransform.anchorMax = new Vector2(0, 0);
			return gameObject;
		}

		/// <summary>
		///	Méthode Pour tracer une ligne entre deux points du graphe
		/// </summary>
		private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
		{
			GameObject gameObject = new GameObject("dotConnection", typeof(Image));
			gameObject.transform.SetParent(_graphContainer, false);
			gameObject.GetComponent<Image>().color = dotConnectionColor; //en blanc avec transparence
			RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
			Vector2 dir = (dotPositionB - dotPositionA).normalized;
			float distance = Vector2.Distance(dotPositionA, dotPositionB);
			rectTransform.anchorMin = new Vector2(0, 0);
			rectTransform.anchorMax = new Vector2(0, 0);
			rectTransform.sizeDelta = new Vector2(distance, 3f);
			rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
			rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * 180 / Mathf.PI);
			return gameObject;



		}
	}
}


