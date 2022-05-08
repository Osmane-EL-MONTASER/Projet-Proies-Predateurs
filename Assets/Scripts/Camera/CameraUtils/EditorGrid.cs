using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// Classe représentant une grille avec une taille personnalisée
/// et un texture personnalisée.
/// 
/// Fait par EL MONTASER Osmane le 12/03/2022.
/// </summary>
class EditorGrid : MonoBehaviour {
    /// <summary>
    /// Objet représentant la grille qui est composée de plusieurs
    /// petits carrés transparents.
    /// </summary>
    private List<GameObject> _grid { get; set; }

    /// <summary>
    /// La taille de chaque case de la grille. Attention à ce que
    /// la taille ne soit pas trop petite pour éviter le lag sur
    /// de très gros mondes.
    /// </summary>
    public int GridSize = 25;

    /// <summary>
    /// Le terrain sur lequel superposer la grille.
    /// </summary>
    public Terrain TerrainToEdit;

    /// <summary>
    /// La texture des cases de la grille de taille quelconque.
    /// Attention à ce que le material soit set sur Repeat et non
    /// Clamp.
    /// </summary>
    public Material GridMaterial;

    /// <summary>
    /// Le material des cases qui sont survolées par le curseur.
    /// </summary>
    private Material _selectedSquareMaterial;

    /// <summary>
    /// Pour pouvoir remettre la couleur d'origine à l'ancienne
    /// case survolée.
    /// </summary>
    private GameObject _lastHoveredSquare;

    /// <summary>
    /// Permet d'initialiser entre autres les matériaux des cases.
    /// 
    /// Fait par EL MONTASER Osmane le 13/03/2022.
    /// </summary>
    void Start() {
        _selectedSquareMaterial = new Material(GridMaterial);
    }

    /// <summary>
    /// La fonction met à jour la grille et gère les évènements
    /// tel que le survol des cases ou le dessin sur la grille.
    /// 
    /// Fait par EL MONTASER Osmane le 13/03/2022.
    /// </summary>
    void Update() {
        if(_grid != null && _grid.Count() == (TerrainToEdit.terrainData.size.x * TerrainToEdit.terrainData.size.z) / (GridSize * GridSize)) {
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tintHoveredSquare(worldMousePosition);
        }
    }

    /// <summary>
    /// Pouvoir changer la météo sélectionnée dans le panel.
    /// 
    /// Fait par EL MONTASER Osmane le 13/03/2022.
    /// </summary>
    /// <param name="selectedWeather">
    /// La nouvelle météo sélectionnée.
    /// </param>
    public void ChangeSquareColor(WeatherType selectedWeather) {
        _selectedSquareMaterial.color = getSquareColorFromWeather(selectedWeather);
    }

    /// <summary>
    /// Fonction qui permet de générer la grille d'édition.
    /// Si la grille a déjà été créee, elle réaffichera les
    /// cases de la grille.
    /// 
    /// Fait par EL MONTASER Osmane le 12/03/2022.
    /// </summary>
    public void CreateGrid(WeatherType selectedWeather) {
        if(_grid != null && _grid.Count() > 0) {
            ShowGrid();
            return;
        }

        _selectedSquareMaterial.color = getSquareColorFromWeather(selectedWeather);

        float x = TerrainToEdit.terrainData.size.x;
        float y = TerrainToEdit.terrainData.size.y;
        float z = TerrainToEdit.terrainData.size.z;

        _grid = new List<GameObject>();

        for(int i = 0; i < z; i += GridSize) {
            for(int j = 0; j < x; j += GridSize) {
                _grid.Add(new GameObject("Grid"));
                MeshFilter mf = _grid.Last().AddComponent(typeof(MeshFilter)) as MeshFilter;
                MeshRenderer mr = _grid.Last().AddComponent(typeof(MeshRenderer)) as MeshRenderer;
                mr.material = GridMaterial;
                mf.mesh = getPlaneMesh();
                _grid.Last().transform.position = new Vector3(j, y, i);
            }
        }
    }

    /// <summary>
    /// Fonction qui permet de cacher la grille et ses composants.
    /// 
    /// Fait par EL MONTASER Osmane le 12/03/2022.
    /// </summary>
    public void HideGrid() {
        foreach (var square in _grid)
            square.GetComponent<MeshRenderer>().enabled = false;
    }

    /// <summary>
    /// Fonction qui permet de réafficher la grille et ses 
    /// composants.
    /// Fait par EL MONTASER Osmane le 13/03/2022.
    /// </summary>
    public void ShowGrid() {
        foreach (var square in _grid)
            square.GetComponent<MeshRenderer>().enabled = true;
    }
    
    /// <summary>
    /// Fonction qui permet à la case survolée de changer de
    /// couleur.
    /// 
    /// Fait par EL MONTASER Osmane le 13/03/2022.
    /// </summary>
    /// <param name="worldMousePosition">
    /// La position de la souris dans le monde et pas par rapport
    /// à l'écran.
    /// </param>
    private void tintHoveredSquare(Vector3 worldMousePosition) {
        if(_lastHoveredSquare != null)
            _lastHoveredSquare.GetComponent<Renderer>().material = GridMaterial;

        if(worldMousePosition.x >= 0 && worldMousePosition.x < TerrainToEdit.terrainData.size.x
            && worldMousePosition.z >= 0 && worldMousePosition.z < TerrainToEdit.terrainData.size.z) {
            int xHoveredSquare =  (int)Math.Floor(worldMousePosition.x / GridSize);
            int zHoveredSquare = (int)Math.Floor(worldMousePosition.z / GridSize);
            int hoveredSquareIndex = xHoveredSquare + (zHoveredSquare * (int)(TerrainToEdit.terrainData.size.x / GridSize));

            //Si la case est dans la grille et qu'elle n'a pas été déjà coloriée.
            if(hoveredSquareIndex >= 0 && hoveredSquareIndex < _grid.Count()) {
                if(Input.GetMouseButton(0)) {
                    _grid[hoveredSquareIndex].GetComponent<Renderer>().material.SetColor("_BaseColor", _selectedSquareMaterial.color);
                    _lastHoveredSquare = null;
                } else if(Input.GetMouseButton(1)) {
                    _grid[hoveredSquareIndex].GetComponent<Renderer>().material.SetColor("_BaseColor", GridMaterial.color);
                    _lastHoveredSquare = null;
                } else if(_grid[hoveredSquareIndex].GetComponent<Renderer>().material.color == GridMaterial.color) {
                    _grid[hoveredSquareIndex].GetComponent<Renderer>().material = _selectedSquareMaterial;
                    _lastHoveredSquare = _grid[hoveredSquareIndex];
                }
            }
        }
    }

    /// <summary>
    /// Fonction qui permet de colorier la case de la grille
    /// passée en paramètre.
    /// 
    /// Fait par EL MONTASER Osmane le 14/03/2022.
    /// </summary>
    /// <param name="square">
    /// La case à colorier.
    /// </param>
    private void paintSquare(GameObject square) {
        square.GetComponent<Renderer>().material = _selectedSquareMaterial;
    }

    /// <summary>
    /// Fonction utilitaire qui permet de récupérer la couleur 
    /// à utiliser pour colorier la case en fonction de la météo
    /// sélectionnée.
    ///  
    /// Fait par EL MONTASER Osmane le 13/03/2022.
    /// </summary>
    /// <param name="weather">
    /// La chaîne de caractères correspondant à la météo 
    /// sélectionnée dans le panel.
    /// </param>
    private Color getSquareColorFromWeather(WeatherType weather) {
        switch (weather) {
            case WeatherType.Wind:
                return WeatherNames.WIND_COLOR;
            case WeatherType.Thunderstorm:
                return WeatherNames.THUNDERSTORM_COLOR;
            case WeatherType.Storm:
                return WeatherNames.STORM_COLOR;
            case WeatherType.Drought:
                return WeatherNames.DROUGHT_COLOR;
            case WeatherType.Rain:
                return WeatherNames.RAIN_COLOR;
            default:
                return Color.white;
        }
    }

    /// <summary>
    /// Permet de construire une case de la grille qui s'affiche
    /// à l'écran au moment du changement de vue.
    /// 
    /// Fait par EL MONTASER Osmane le 12/03/2022.
    /// </summary>
    /// <returns>
    /// Retourne un Plane texturé avec une taille de 1x1. Il est
    /// possible de changer sa taille en changeant le Vector3 de
    /// la variable localTranslation de son transform.
    /// </returns>
    private Mesh getPlaneMesh() {
        Mesh plane = new Mesh();

        plane.vertices = new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(GridSize, 0, 0),
            new Vector3(GridSize, 0, GridSize),
            new Vector3(0, 0, GridSize),
        };

        plane.uv = new Vector2[] {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        };

        plane.triangles = new int[]{0, 1, 2, 0, 2, 3};
        plane.RecalculateBounds();
        plane.RecalculateNormals();

        return plane;
    }
}