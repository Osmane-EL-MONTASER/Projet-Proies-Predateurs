using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    /// La taille des cases de la grille.
    /// </summary>
    public int GridSize = 1;

    /// <summary>
    /// La texture des cases de la grille de taille quelconque.
    /// Attention à ce que le material soit set sur Repeat et non
    /// Clamp.
    /// </summary>
    public Material GridMaterial;

    void Start() {}

    void Update() {}

    /// <summary>
    /// Fonction qui permet de générer la grille d'édition.
    /// 
    /// Fait par EL MONTASER Osmane le 12/03/2022.
    /// </summary>
    public void CreateGrid() {
        _grid = new List<GameObject>();
        for(int i = 0; i < 1000; i += GridSize) {
            for(int j = 0; j < 1000; j += GridSize) {
                _grid.Add(new GameObject("Grid"));
                MeshFilter mf = _grid.Last().AddComponent(typeof(MeshFilter)) as MeshFilter;
                MeshRenderer mr = _grid.Last().AddComponent(typeof(MeshRenderer)) as MeshRenderer;
                mr.material = GridMaterial;
                mf.mesh = getPlaneMesh();
                _grid.Last().transform.position = new Vector3(j, 300f, i);
            }
        }
    }

    /// <summary>
    /// Fonction qui permet de détruire la grille et ses composants.
    /// 
    /// Fait par EL MONTASER Osmane le 12/03/2022.
    /// </summary>
    public void ClearGrid() {
        foreach (var square in _grid)
            Destroy(square);

        _grid.Clear();
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