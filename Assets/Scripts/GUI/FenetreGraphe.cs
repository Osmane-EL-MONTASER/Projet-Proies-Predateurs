

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;


// regler probleme histo pour s'afficher sur la ligne x
// reprendre bouton plus et moins


/// <summary>
/// Classe codée par Bilal HAMICHE pour le prototype. Elle permet la création de graphes dynamiques. Elle
/// est insiprée d'une série de vidéos qui expliquent comment créer un graphe sur Unity 3D 
/// https://www.youtube.com/playlist?list=PLzDRvYVwl53v5ur4GluoabyckImZz3TVQ
/// </summary>
public class FenetreGraphe : MonoBehaviour
{
    [SerializeField] private Sprite _dotSprite; // pour les points sur le graphe
    private RectTransform _graphContainer; 
	private RectTransform _labelTemplateX;
	private RectTransform _labelTemplateY;
	private RectTransform _dashTemplateX; 
	private RectTransform _dashTemplateY;
	private List<GameObject> _gameObjectList;
	
	// Cached values
	private List<int> valueList;
	private IGraphVisual graphVisual;
	private int maxVisibleValueAmount; 
	private Func<int, string> getAxisLabelX;
	private Func<float, string> getAxisLabelY;
	
	
    private void Awake() {
        _graphContainer = transform.Find("_graphContainer").GetComponent<RectTransform>();
		_labelTemplateX= _graphContainer.Find("_labelTemplateX").GetComponent<RectTransform>();
        _labelTemplateY= _graphContainer.Find("_labelTemplateY").GetComponent<RectTransform>();
        _dashTemplateX= _graphContainer.Find("_labelTemplateX").GetComponent<RectTransform>();
        _dashTemplateY= _graphContainer.Find("_labelTemplateY").GetComponent<RectTransform>();
		_gameObjectList = new List<GameObject>();
		
		
		
		//List<int> valueList = new List<int>() { 300, 208, 156, 45, 30, 22, 17, 15, 13, 17, 25, 37, 40, 36, 33}; // placement des points
		List<int> valueList = new List<int>() { 50, 40, 25, 28, 28, 280, 48, 58, 68, 47, 12, 5, 68, 96, 98 };
		IGraphVisual lineGraphVisual =  new LineGraphVisual( _graphContainer, _dotSprite, Color.green, new Color(1, 1, 1, .5f));
		IGraphVisual barGraphVisual =  new BarChartVisual(_graphContainer, Color.green, .8f);
		//ShowGraph(valueList, barChartVisual, -1, (int _i) => "Jour "+(_i + 1) , (float _f)  => Mathf.RoundToInt(_f) + "$" ); // utile pour placer nb agent en y et en x le temps
		ShowGraph(valueList, lineGraphVisual, -1, (int _i) => "Jour "+(_i + 1) , (float _f)  => Mathf.RoundToInt(_f) + "$" ); // utile pour placer nb agent en y et en x le temps


		//a modif ça
		
		transform.Find("barChartBtn").GetComponent<Button_UI>().ClickFunc = () => {
			SetGraphVisual(barGraphVisual);
		};
		

		//a modif ça
		transform.Find("lineGraphBtn").GetComponent <Button_UI>().ClickFunc = () => {
			SetGraphVisual(lineGraphVisual);
		};

		//transform.Find("decreaseVisibleAmountBtn").GetComponent<Button_UI>().ClickFunc = () => {
			//DecreaseVisibleAmount();
		//};

		transform.Find("increaseVisibleAmountBtn").GetComponent<Button_UI>().ClickFunc = () => {
			IncreaseVisibleAmount();
		};
		/*
		bool useBarChart = true;
		FunctionPeriodic.Create(() => {
			if (useBarChart) {
				ShowGraph(valueList, barGraphVisual, -1, (int _i) => "Jour "+(_i + 1) , (float _f)  => Mathf.RoundToInt(_f) + "$" ); 
			} else {
				ShowGraph(valueList, lineGraphVisual, -1, (int _i) => "Jour "+(_i + 1) , (float _f)  => Mathf.RoundToInt(_f) + "$" );
			}
			useBarChart = !useBarChart;
		}, .5f);
		*/
		/*
		// pour generer des graphes aleatoires
		FunctionPeriodic.Create(() => {
			valueList.Clear();
			for (int i=0; i<15; i++) {
				valueList.Add(UnityEngine.Random.Range(0, 500));
			}
		ShowGraph(valueList, (int _i) => "Jour "+(_i+1) , (float _f)  =>  Mathf.RoundToInt(_f) + "$" ); // utile pour placer nb agent en y et en x le temps	
		}, .5f); // chaque demi seconde changement de graphes
		
		*/

	}

	//pour bouton ajout
	private void IncreaseVisibleAmount()
	{
		ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount + 1, this.getAxisLabelX, this.getAxisLabelY);

	}




	private void DecreaseVisibleAmount()
	{
		ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount - 1, this.getAxisLabelX, this.getAxisLabelY);

	}

	private void SetGraphVisual(IGraphVisual graphVisual) {
		ShowGraph(this.valueList, graphVisual, this.maxVisibleValueAmount, this.getAxisLabelX, this.getAxisLabelY);
		
		
	}
	private void ShowGraph(List<int> valueList, IGraphVisual graphVisual, int maxVisibleValueAmount = -1, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null) {
		this.valueList = valueList;
		this.graphVisual = graphVisual;
		this.getAxisLabelX = getAxisLabelX;
		this.getAxisLabelY = getAxisLabelY;
		
		if (maxVisibleValueAmount <= 0) {
			maxVisibleValueAmount=valueList.Count;
		}
		if (maxVisibleValueAmount > valueList.Count) {
			maxVisibleValueAmount=valueList.Count;
		}
		this.maxVisibleValueAmount = maxVisibleValueAmount;
		
		if (getAxisLabelX == null) {
			getAxisLabelX = delegate(int _i)  { return _i.ToString();};
		}
		
		
		if (getAxisLabelY == null) {
			getAxisLabelY = delegate(float _f) { return Mathf.RoundToInt(_f).ToString(); };
		}
		
		if(maxVisibleValueAmount <= 0) {
			maxVisibleValueAmount = valueList.Count;
		}
		
		foreach (GameObject gameObject in _gameObjectList) {
			Destroy(gameObject);
		}
		
		_gameObjectList.Clear();
		
		float graphWidth = _graphContainer.sizeDelta.x;
		float graphHeight = _graphContainer.sizeDelta.y;
		
		
		float yMaximum = valueList[0]; //ecart entre valeur y genre la ta 0 puis 100 puis 200 si = 100f
		float yMinimum = valueList[0];
		
		for (int i= Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++) {
			int value = valueList[i];
			if (value > yMaximum) {
				yMaximum = value;
			}
			if (value < yMinimum) {
				yMinimum = value;
		}
		}
		float yDifference = yMaximum - yMinimum;
		
		if (yDifference <= 0) {
			yDifference =5f;
			
		}
		
		
		yMaximum = yMaximum + (yDifference * 0.2f); // pour laisser un ecart entre val max et le haut du graphe
		yMinimum = yMinimum - (yDifference * 0.2f); // pour laisser un ecart entre val min et le bas du graphe
		
		
		
		yMinimum = 0f; // graphe commene a 0
		
		//float xSize = 50f; //distance entre chque point
		float xSize = graphWidth / (maxVisibleValueAmount + 1); //permete de rendre x dynamique
		
		
		int xIndex=0;
		
		//LineGraphVisual lineGraphVisual = new LineGraphVisual( _graphContainer, _dotSprite, Color.green, new Color(1, 1, 1, .5f));
		//BarChartVisual barChartVisual = new BarChartVisual(_graphContainer, Color.green, .8f); // design du graphe (couleur, et largeur)
		//GameObject lastDotGameObject = null;
		for (int i= Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++) {
			float xPosition = xSize + xIndex * xSize;
			float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;
			
			_gameObjectList.AddRange(graphVisual.AddGraphVisual(new Vector2(xPosition, yPosition), xSize));
			//_gameObjectList.AddRange(barChartVisual.AddGraphVisual(new Vector2(xPosition, yPosition), xSize));
			
			
			/*
			GameObject dotGameObject = CreateDot(new Vector2(xPosition, yPosition));
			_gameObjectList.Add(dotGameObject);
			if (lastDotGameObject !=null) {
			GameObject dotConnectionGameObject = CreateDotConnection(lastDotGameObject.GetComponent<RectTransform>().anchoredPosition, dotGameObject.GetComponent<RectTransform>().anchoredPosition);
			_gameObjectList.Add(dotConnectionGameObject);
		}
		lastDotGameObject = dotGameObject;
		*/
		
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
	
	int separatorCount = 10; // Nb d'element axe Y
	for (int i=0; i <= separatorCount; i++) {
		RectTransform labelY = Instantiate(_labelTemplateY);
		labelY.SetParent(_graphContainer, false);
		labelY.gameObject.SetActive(true);
		float normalizedValue = i *1f / separatorCount;
		labelY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
		labelY.GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
		_gameObjectList.Add(labelY.gameObject);
		
		
		
		RectTransform dashY = Instantiate(_dashTemplateY);
		dashY.SetParent(_graphContainer, false);
		dashY.gameObject.SetActive(true);
		dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight); //a remttre -20 si bug
		_gameObjectList.Add(dashY.gameObject);
	}
	}	

	
	private interface IGraphVisual { //Interface pour passe de graphe a histo
		List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth);
		
	}
	
	
	 private class BarChartVisual : IGraphVisual{
		
		private RectTransform _graphContainer;
		private Color barColor;
		private float barWidthMultiplier;
		
		//Constructeur de la classe
		public BarChartVisual(RectTransform _graphContainer, Color barColor, float barWidthMultiplier) {
			this._graphContainer = _graphContainer;
			this.barColor = barColor;
			this.barWidthMultiplier = barWidthMultiplier;
			
		}
		
		public List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth) {
			GameObject barGameObject = CreateBar(graphPosition, graphPositionWidth * 0.9f);
			return new List<GameObject>() { barGameObject };
		}
		
		private GameObject CreateBar( Vector2 graphPosition, float barWidth) {
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

  private class LineGraphVisual : IGraphVisual {
	 
	private RectTransform _graphContainer;
	private Sprite _dotSprite;
	private GameObject lastDotGameObject;
	private Color dotColor;
	private Color dotConnectionColor;
		
	public LineGraphVisual(RectTransform _graphContainer, Sprite _dotSprite, Color dotColor, Color dotConnectionColor) {
		this._graphContainer = _graphContainer;
		this._dotSprite = _dotSprite;
		this.dotColor = dotColor;
		this.dotConnectionColor = dotConnectionColor;
		lastDotGameObject = null;
		
	}
	
	
	public List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth) {
		List<GameObject> _gameObjectList = new List<GameObject>(); 
		GameObject dotGameObject = CreateDot(graphPosition);
			_gameObjectList.Add(dotGameObject);
			if (lastDotGameObject !=null) {
			GameObject dotConnectionGameObject = CreateDotConnection(lastDotGameObject.GetComponent<RectTransform>().anchoredPosition, dotGameObject.GetComponent<RectTransform>().anchoredPosition);
			_gameObjectList.Add(dotConnectionGameObject);
			}
		lastDotGameObject=dotGameObject;	
		return _gameObjectList;
	}
		
		
		
		
		
	
	 private GameObject CreateDot(Vector2 anchoredPosition) {
        GameObject gameObject = new GameObject("dot", typeof(Image));
        gameObject.transform.SetParent(_graphContainer, false);
		gameObject.GetComponent<Image>().sprite = _dotSprite;
		gameObject.GetComponent<Image>().color = dotColor;
      
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11,11); // taille des points
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
		return gameObject;
    }

		// pour fait ligne entre deux point
	private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
		GameObject gameObject = new GameObject("dotConnection", typeof(Image));
		gameObject.transform.SetParent(_graphContainer, false);
		gameObject.GetComponent<Image>().color = dotConnectionColor; //en blanc avec transparence
		RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
		Vector2 dir = (dotPositionB - dotPositionA). normalized;
		float distance = Vector2.Distance(dotPositionA, dotPositionB);
		rectTransform.anchorMin = new Vector2(0,0);
		rectTransform.anchorMax = new Vector2(0,0);
		rectTransform.sizeDelta = new Vector2 (distance, 3f);
		rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
		rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y,dir.x)*180/Mathf.PI);
		return gameObject;
		
	
	
 }
}
}

	


/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// trouver comment augmenter taille des label en y et comment augment y maximu jusu'au en haut du graphes
// trouver probleme pour le code de utils

/// <summary>
/// Classe codée par Bilal HAMICHE pour le prototype. Elle permet la création de graphes dynamiques. Elle
/// est insiprée d'une série de vidéos qui expliquent comment créer un graphe sur Unity 3D 
/// https://www.youtube.com/playlist?list=PLzDRvYVwl53v5ur4GluoabyckImZz3TVQ
/// </summary>
public class FenetreGraphe : MonoBehaviour
{
    [SerializeField] private Sprite _circleSprite; // pour les points sur le graphe
    private RectTransform _graphContainer; 
	private RectTransform _labelTemplateX;
	private RectTransform _labelTemplateY;
	private RectTransform _dashTemplateX; 
	private RectTransform _dashTemplateY;
	private List<GameObject> _gameObjectList;
	
    private void Awake() {
        _graphContainer = transform.Find("_graphContainer").GetComponent<RectTransform>();
		_labelTemplateX= _graphContainer.Find("_labelTemplateX").GetComponent<RectTransform>();
        _labelTemplateY= _graphContainer.Find("_labelTemplateY").GetComponent<RectTransform>();
        _dashTemplateX= _graphContainer.Find("_labelTemplateY").GetComponent<RectTransform>();
        _dashTemplateY= _graphContainer.Find("_labelTemplateY").GetComponent<RectTransform>();
		_gameObjectList = new List<GameObject>();
		
		
		
		List<int> valueList = new List<int>() { 156, 45, 30, 22, 17, 15, 13, 17, 25, 37, 40, 201, 158}; // placement des points
		ShowGraph(valueList, (int _i) => "Jour "+(_i+1) , (float _f)  =>  Mathf.RoundToInt(_f) + "$" ); // utile pour placer nb agent en y et en x le temps
		
		// pour generer des graphes aleatoires
		
		FunctionPeriodic.Create(() => {
			valueList.Clear();
			for (int i=0; i<12; i++) {
				valueList.Add(UnityEngine.Random.Range(0, 500));
			}
		ShowGraph(valueList, (int _i) => "Jour "+(_i+1) , (float _f)  =>  Mathf.RoundToInt(_f) + "$" ); // utile pour placer nb agent en y et en x le temps	
		}, .5f); // chaque demi seconde changement de graphes
	
    }
	
    private GameObject CreateCricle(Vector2 anchoredPosition) {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(_graphContainer, false);
        gameObject.GetComponent<Image>().sprite = _circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11,11); // taille des points
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
		return gameObject;
    }
	
	private void ShowGraph(List<int> valueList, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null) {
		
		if (getAxisLabelX == null) {
			getAxisLabelX = delegate(int _i)  { return _i.ToString();};
		}
		
		
		if (getAxisLabelY == null) {
			getAxisLabelY = delegate(float _f) { return Mathf.RoundToInt(_f).ToString(); };
		}
		
		foreach (GameObject gameObject in _gameObjectList) {
			Destroy(gameObject);
		}
		
		_gameObjectList.Clear();
		
		float graphHeight = _graphContainer.sizeDelta.y;
		
		float yMaximum = valueList[0]; //ecart entre valeur y genre la ta 0 puis 100 puis 200 si = 100f
		float yMinimum = valueList[0];
		
		foreach (int value in valueList) {
			if (value > yMaximum) {
				yMaximum = value;
			}
			if (value < yMinimum) {
				yMinimum = value;
		}
		}
		yMaximum = yMaximum + ((yMaximum - yMinimum) * 0.2f); // pour laisser un ecart entre val max et le haut du graphe
		yMinimum = yMinimum - ((yMaximum - yMinimum) * 0.2f); // pour laisser un ecart entre val min et le bas du graphe
		
		
		
		
		
		float xSize = 50f; //distance entre chque point
		
		GameObject lastCircleGameObject = null;
		for (int i=0; i < valueList.Count; i++) {
			float xPosition = xSize + i * xSize;
			float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;
			GameObject circleGameObject = CreateCricle(new Vector2(xPosition, yPosition));
			_gameObjectList.Add(circleGameObject);
			if (lastCircleGameObject !=null) {
			GameObject dotConnectionGameObject = CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
			_gameObjectList.Add(dotConnectionGameObject);
		}
		lastCircleGameObject = circleGameObject;
		
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
		
	}
	
	int separatorCount = 10; // Nb d'element axe Y
	for (int i=0; i <= separatorCount; i++) {
		RectTransform labelY = Instantiate(_labelTemplateY);
		labelY.SetParent(_graphContainer, false);
		labelY.gameObject.SetActive(true);
		float normalizedValue = i *1f / separatorCount;
		labelY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
		labelY.GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
		_gameObjectList.Add(labelY.gameObject);
		
		
		
		RectTransform dashY = Instantiate(_dashTemplateY);
		dashY.SetParent(_graphContainer, false);
		dashY.gameObject.SetActive(true);
		dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight); //a remttre -20 si bug
		_gameObjectList.Add(dashY.gameObject);
	}
	}	

	
	
	// pour fait ligne entre deux point
	private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
		GameObject gameObject = new GameObject("dotConnection", typeof(Image));
		gameObject.transform.SetParent(_graphContainer, false);
		gameObject.GetComponent<Image>().color = new Color(1,1,1, .5f); //en blanc avec transparence
		RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
		Vector2 dir = (dotPositionB - dotPositionA). normalized;
		float distance = Vector2.Distance(dotPositionA, dotPositionB);
		rectTransform.anchorMin = new Vector2(0,0);
		rectTransform.anchorMax = new Vector2(0,0);
		rectTransform.sizeDelta = new Vector2 (distance, 3f);
		rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
		rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y,dir.x)*180/Mathf.PI);
		return gameObject;
		
	}
}

	*/






	









	




