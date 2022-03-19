using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fenetre_Graphe : MonoBehaviour
{
    [SerializeField] private Sprite _circleSprite; // pour les points sur le graphe
    private RectTransform _graphContainer;
	private RectTransform labelTemplateX;
	private RectTransform labelTemplateY;
	
    private void Awake() {
        _graphContainer = transform.Find("_graphContainer").GetComponent<RectTransform>();
		labelTemplateX= _graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY= _graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
		
		List<int> valueList = new List<int>() { 5, 98, 56, 45, 30, 22, 17, 15, 13, 17, 25, 37, 40, 36, 33}; 
		ShowGraph(valueList);
    }
    private GameObject CreateCricle(Vector2 anchoredPosition) {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(_graphContainer, false);
        gameObject.GetComponent<Image>().sprite = _circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
		return gameObject;
    }
	
	private void ShowGraph(List<int> valueList) {
		float graphHeight = _graphContainer.sizeDelta.y;
		float yMaximum = 100f;
		float xSize = 50f; //distance entre chque point
		
		GameObject lastCircleGameObject = null;
		for (int i=0; i < valueList.Count; i++) {
			float xPosition = xSize + i * xSize;
			float yPosition = (valueList[i] / yMaximum) * graphHeight;
			GameObject circleGameObject = CreateCricle(new Vector2(xPosition, yPosition));
			if (lastCircleGameObject !=null) {
			CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
		}
		lastCircleGameObject = circleGameObject;
		
		RectTransform labelX = Instantiate(labelTemplateX);
		labelX.SetParent(_graphContainer);
		labelX.gameObject.SetActive(true);
		labelX.anchoredPosition = new Vector2(xPosition, -20f);
		labelX.GetComponent<Text>().text = i.ToString();
	}
	
	int separatorCount = 10; // Nb d'element axe Y
	for (int i=0; i < separatorCount; i++) {
		RectTransform labelY = Instantiate(labelTemplateY);
		labelY.SetParent(_graphContainer);
		labelY.gameObject.SetActive(true);
		float normalizedValue = i *1f / separatorCount;
		labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
		labelY.GetComponent<Text>().text = Mathf.RoundToInt(normalizedValue * yMaximum).ToString();
	
	
	}
	
	}
	// pour fait ligne entre deuc point
	private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
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
		
	}
}
	