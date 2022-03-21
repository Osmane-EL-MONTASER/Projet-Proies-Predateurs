using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classe gère le changement de biomes.
/// 
/// Fait par AVERTY Pierre le 14/03/2022 et modifié le 21/03/2022.
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
    /// Fait par AVERTY Pierre le 13/03/2022 et modifiée le 21/03/2022.
    /// </summary>
    private void setNewCamera() {
        var world = World.Instance;

        if(newCamera == mainCamera){
                world.Name = "Base";

                mainCamera.SetActive(true);
                desertCamera.SetActive(false);
                montaignCamera.SetActive(false);
                forestCamera.SetActive(false);
        } else if (newCamera == desertCamera) {
                world.Name = "Désert";

                mainCamera.SetActive(false);
                desertCamera.SetActive(true);
                montaignCamera.SetActive(false);
                forestCamera.SetActive(false);
        } else if(newCamera == montaignCamera) {
                world.Name = "Montagne";

                mainCamera.SetActive(false);
                desertCamera.SetActive(false);
                montaignCamera.SetActive(true);
                forestCamera.SetActive(false);
        } else if(newCamera == forestCamera) {
                world.Name = "Forêt";

                mainCamera.SetActive(false);
                desertCamera.SetActive(false);
                montaignCamera.SetActive(false);
                forestCamera.SetActive(true);       
        }
    }
}
