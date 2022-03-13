using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public static int SquareSize = 64;

    /// <summary>
    /// Une référence au terrain de la carte.
    /// </summary>
    public GameObject Terrain;

    void Start() {
        
    }

    void Update() {
        
    }
}
