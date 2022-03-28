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
/// Monde principale.
/// </summary>
    public GameObject mainWorld;

/// <summary>
/// Caméra du désert.
/// </summary>
    public GameObject desertCamera;

/// <summary>
/// Monde du désert.
/// </summary>
    public GameObject desertWorld;

/// <summary>
/// Caméra de la montagne.
/// </summary>
    public GameObject montaignCamera;

/// <summary>
/// Monde de la montagne.
/// </summary>
    public GameObject montaignWorld;

/// <summary>
/// Caméra de la foret.
/// </summary>
    public GameObject forestCamera;

/// <summary>
/// Caméra de la foret.
/// </summary>
    public GameObject forestWorld;

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
    /// Fait par AVERTY Pierre le 13/03/2022, modifiée le 21/03/2022 et le 25/03/2022.
    /// </summary>
    private void setNewCamera() {
        var world = World.Instance;
        var camera = ConfigCamera.Instance;

        if(newCamera == mainCamera){
                world.Name = "Base";
                camera.CurrentCamera = mainCamera;

                mainCamera.SetActive(true);
                mainWorld.SetActive(true);
                desertCamera.SetActive(false);
                desertWorld.SetActive(false);
                montaignCamera.SetActive(false);
                montaignWorld.SetActive(false);
                forestCamera.SetActive(false);
                forestWorld.SetActive(false);
        } else if (newCamera == desertCamera) {
                world.Name = "Désert";
                camera.CurrentCamera = desertCamera;
                
                mainCamera.SetActive(false);
                mainWorld.SetActive(false);
                desertCamera.SetActive(true);
                desertWorld.SetActive(true);
                montaignCamera.SetActive(false);
                montaignWorld.SetActive(false);
                forestCamera.SetActive(false);
                forestWorld.SetActive(false);
        } else if(newCamera == montaignCamera) {
                world.Name = "Montagne";
                camera.CurrentCamera = montaignCamera;

                mainCamera.SetActive(false);
                mainWorld.SetActive(false);
                desertCamera.SetActive(false);
                desertWorld.SetActive(false);
                montaignCamera.SetActive(true);
                montaignWorld.SetActive(true);
                forestCamera.SetActive(false);
                forestWorld.SetActive(false);
        } else if(newCamera == forestCamera) {
                world.Name = "Forêt";
                camera.CurrentCamera = forestCamera;

                mainCamera.SetActive(false);
                mainWorld.SetActive(false);
                desertCamera.SetActive(false);
                desertWorld.SetActive(false);
                montaignCamera.SetActive(false);
                montaignWorld.SetActive(false);
                forestCamera.SetActive(true);     
                forestWorld.SetActive(true);
        }
    }
}
