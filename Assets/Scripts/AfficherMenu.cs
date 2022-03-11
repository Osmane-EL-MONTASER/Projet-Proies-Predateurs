using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Classe cod�e par Bilal HAMICHE pour le prototype. Elle
/// est insipr�e d'une vid�o qui explique comment cr�er un menu d'options
/// cam�ra. Brackeys, https://www.youtube.com/watch?v=wqm7iyoZSPI.
/// </summary>
public class AfficherMenu : MonoBehaviour
{

    // Varaible de type GameObject sui permettra de gerer le menu options
    public GameObject MenuOptions;

    //�r defaut on va mettre panel pas visble
    bool visible = false;

    //pour setecter touche
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


















