using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe gère le changement de biomes.
/// 
/// Fait par AVERTY Pierre le 14/03/2022.
/// </summary>
public class SetBiome : MonoBehaviour
{
/// <summary>
/// Nouvelle caméra.
/// </summary>
    public GameObject newCamera;

/// <summary>
/// Caméra principale.
/// </summary>
    public GameObject mainCamera;

/// <summary>
/// Caméra du désert.
/// </summary>
    public GameObject desertCamera;

/// <summary>
/// Caméra de la montagne.
/// </summary>
    public GameObject montaignCamera;

/// <summary>
/// Caméra de la foret.
/// </summary>
    public GameObject forestCamera;

    /// <summary>
    /// Executée lors d'un clic sur un des biomes dans le menu configuration.
    ///
    /// Fait par AVERTY Pierre le 13/03/2022.
    /// </summary>
    public void onClick() {
        setNewCamera();
    }

    /// <summary>
    /// Switch la camèra selon le biome choisis. A REFACTORISER DANS ConfigCamera.cs
    ///
    /// Fait par AVERTY Pierre le 13/03/2022.
    /// </summary>
    private void setNewCamera() {
        if(newCamera == mainCamera){
                mainCamera.SetActive(true);
                desertCamera.SetActive(false);
                montaignCamera.SetActive(false);
                forestCamera.SetActive(false);
        } else if (newCamera == desertCamera) {
                mainCamera.SetActive(false);
                desertCamera.SetActive(true);
                montaignCamera.SetActive(false);
                forestCamera.SetActive(false);
        } else if(newCamera == montaignCamera) {
                mainCamera.SetActive(false);
                desertCamera.SetActive(false);
                montaignCamera.SetActive(true);
                forestCamera.SetActive(false);
        } else if(newCamera == forestCamera) {
                mainCamera.SetActive(false);
                desertCamera.SetActive(false);
                montaignCamera.SetActive(false);
                forestCamera.SetActive(true);       
        }
    }
}
