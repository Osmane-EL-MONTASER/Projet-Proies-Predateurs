using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe de Gestion des Agents
///
/// Fait par Greg Demirdjian et Bilal Hamiche le 19/03/2022.
/// </summary> 
public class GestionAgents : MonoBehaviour
{
    public List<GameObject> ListeAgents;

    public GameObject wolf;
    public GameObject Iguana;

    /// <summary>
    /// creerAgents : méthode qui fait apparaitre un nombre d'agents donnés et de l'espèce voulue.
    ///
    /// Fait par Greg Demirdjian et Bilal Hamiche le 19/03/2022.
    /// </summary> 
    public void creerAgents(int nombre, int idAgent)
    {
        for (int i = 0 ; i < nombre ; i++)
        {
            if (idAgent==1)
                ListeAgents.Add(Instantiate(wolf, new Vector3(435f, 1f, 330f), Quaternion.identity));
            else if (idAgent==2)
                ListeAgents.Add(Instantiate(Iguana, new Vector3(445f, 1f, 310f), Quaternion.identity));
        }


    }

    void Start()
    {
        ListeAgents = new List<GameObject>();
        creerAgents(10,1);
    }

}