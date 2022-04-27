using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Classe codée par Bilal HAMICHE pour le prototype. Elle
/// est insiprée d'une vidéo qui explique comment créer un menu d'options
/// caméra. Brackeys, https://www.youtube.com/watch?v=wqm7iyoZSPI.
/// </summary>
public class Resolution : MonoBehaviour
{
	/// <summary>
    /// Variable de type Dropdown sui permet de sélectionner la résolution
    /// </summary>
    public Dropdown DropdownResolution;


	/// <summary>
    /// Méthode permettant de gérer la résolution
    /// </summary>
    public void SetResolution()
    {
        switch (DropdownResolution.value)
        {
            case 0:
                Screen.SetResolution(640, 360, true);
                break;

            case 1:
                Screen.SetResolution(1920, 1080, true);
                break;

        }

    }

}
