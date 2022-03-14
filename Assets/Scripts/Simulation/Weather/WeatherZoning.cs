using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Classe qui gère le zoning et l'affichage des
/// zones possédant des effets météorologiques
/// spéciaux :
/// Pluie, Tempête, Vent, Orage, Sécheresse.
/// 
/// Fait par EL MONTASER Osmane le 11/03/2022.
/// </summary>
public class WeatherZoning : MonoBehaviour {
    /// <summary>
    /// Le monde est décomposé sous forme de 
    /// quadrillage. La taille d'une case est fixe
    /// et modifiable dans l'éditeur de Unity.
    /// </summary>
    private Weather[][] _worldZones { get; set; }

    /// <summary>
    /// La taille du carré du quadrillage.
    /// /!\ Une petite valeur pourrait causer 
    /// du lag sur de très grandes cartes. /!\
    /// </summary>
    public int SquareSize = 25;

    /// <summary>
    /// Une référence au terrain de la carte.
    /// </summary>
    public Terrain TerrainToEdit;

    /// <summary>
    /// Pour chaque case de notre quadrillage, je
    /// pré-stock les objets présents dans chaque
    /// zone (agents compris) pour permettre d'agir
    /// sur l'environnement en y accédant directement
    /// avec les coordonnées de la case. Ce qui permet
    /// d'améliorer les performances en évitant de
    /// chercher à chaque Update() parmis tous les
    /// objets de la scène.
    /// </summary>
    private List<GameObject>[,] _gameObjects;

    /// <summary>
    /// Ici je lance la fonction de pré-chargement des
    /// arbres par zones pour pouvoir y accéder très
    /// facilement plus tard.
    /// 
    /// Fait par EL MONTASER Osmane le 14/03/2022.
    /// </summary>
    void Start() {
        initializeGameObjetsList();
        loadGameObjects();
    }

    void Update() {}

    /// <summary>
    /// Fonction qui permet d'initialiser la liste des
    /// GameObjects par rapport à la taille du terrain.
    /// 
    /// Fait par EL MONTASER Osmane le 14/03/2022.
    /// </summary>
    private void initializeGameObjetsList() {
        _gameObjects = new List<GameObject>[(int)(TerrainToEdit.terrainData.size.x / SquareSize), (int)(TerrainToEdit.terrainData.size.z / SquareSize)]; 

        for(int i = 0; i < _gameObjects.GetLength(0); i++)
            for(int j = 0; j < _gameObjects.GetLength(1); j++)
                _gameObjects[i, j] = new List<GameObject>();
    }

    /// <summary>
    /// Cette fonction permet d'ajouter par zone (ex :
    /// de ce à quoi cela ressemblera dans la mémoire
    /// pour la zone en bas à gauche :
    /// zone[0][0] <=> _gameObjects[0][0] = {
    ///     TreeInstance tree1,
    ///     TreeInstance tree2,
    ///     Agent agent1,
    ///     Rocher rocher1...
    /// } )
    /// 
    /// Cela permet d'éviter plus tard une recherche
    /// parmis tous les objets de la scène alors que
    /// l'on peut avoir des milliers d'objets...
    /// 
    /// Fait par EL MONTASER Osmane le 14/03/2022.
    /// </summary>
    private void loadGameObjects() {
        TerrainData terrainData = TerrainToEdit.terrainData;

        foreach(TreeInstance tree in terrainData.treeInstances) {
            (int, int) gridCoordinates = getZoneCoordinatesFromRawPosition(tree.position);
            _gameObjects[gridCoordinates.Item1, gridCoordinates.Item2].Add(terrainData.treePrototypes[tree.prototypeIndex].prefab);
        }
    }

    /// <summary>
    /// Permet de convertir la coordonnée Vector3 d'un 
    /// GameObject en une coordonnée de la grille.
    /// 
    /// Fait par EL MONTASER Osmane le 14/03/2022.
    /// </summary>
    /// <param name="rawPosition">
    /// La position en Vector3 de l'objet.
    /// </param>
    /// <returns>
    /// Les coordonnées correpondantes dans la grille.
    /// </returns>
    private (int, int) getZoneCoordinatesFromRawPosition(Vector3 rawPosition) {
        (int, int) gridCoordinates;
        Vector3 worldPosition = TerrainToEdit.transform.position;

        //Si le terrain ne se trouve pas à la position 0 et 
        //pour remettre les positions normales non comprises
        //entre 0 et 1 (Voir la documentation Unity).
        Vector3 normalizedPosition = new Vector3(rawPosition.x * TerrainToEdit.terrainData.size.x - worldPosition.x,
                                                 rawPosition.y * TerrainToEdit.terrainData.size.y,
                                                 rawPosition.z * TerrainToEdit.terrainData.size.z - worldPosition.z);
        gridCoordinates.Item1 = (int)Math.Floor(normalizedPosition.x / SquareSize);
        gridCoordinates.Item2 = (int)Math.Floor(normalizedPosition.z / SquareSize);

        return gridCoordinates;
    }
}
