using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Classe cod?e par Bilal HAMICHE pour le prototype. Elle
/// est insipr?e d'une vid?o qui explique comment cr?er un menu d'options
/// cam?ra. Brackeys, https://www.youtube.com/watch?v=wqm7iyoZSPI.
/// </summary>
public class AfficherMenu : MonoBehaviour
{
    /// <summary>
    /// Variable de type GameObject sui permettra de gerer le menu options
    /// </summary>
    public GameObject MenuOptions;

    /// <summary>
    ///Variable de type bool qui est par defaut false
    /// </summary>
    bool visible = false;

    /// <summary>
    ///Methode pour detecter touche
    /// </summary>
    void Update()
    {

        /// <summary>
        /// Si l'utilisateur appuie sur la touche Echap
        /// le menu option va s'afficher
        /// </summary>
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            visible = !visible;
            MenuOptions.SetActive(visible);

        }
    }

}


















