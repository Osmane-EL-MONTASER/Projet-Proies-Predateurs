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

    /// <summary>
    /// creerAgents : méthode qui fait apparaitre un nombre d'agents donnés et de l'espèce voulue.
    ///
    /// Fait par Greg Demirdjian et Bilal Hamiche le 19/03/2022.
    /// </summary> 
    public void creerAgents(int nombre, int idAgent)
    {
        for (int i = 0 ; i < nombre ; i++)
        {
            ListeAgents.Add(Instantiate(wolf, new Vector3(572.9277f, 80.1882f, 592.933f), Quaternion.identity));
        }


    }

    void Start()
    {
        ListeAgents = new List<GameObject>();
        creerAgents(100,1);
    }

}