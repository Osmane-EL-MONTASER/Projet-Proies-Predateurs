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
    GameObject temp;

    /// <summary>
    /// creerAgents : méthode qui fait apparaitre un nombre d'agents donnés et de l'espèce voulue.
    ///
    /// Fait par Greg Demirdjian et Bilal Hamiche le 19/03/2022.
    /// </summary> 
    public void creerAgents(int nombre, int idAgent)
    {
        for (int i = 0 ; i < 1 ; i++)
        {
            temp = Instantiate(wolf, new Vector3(572.9277f, 80.1882f, 592.933f), Quaternion.identity) as GameObject;

            temp.name = ("wolf" + 1);
            ListeAgents.Add(wolf);
        }


    }

    void Start()
    {
        ListeAgents = new List<GameObject>();
        creerAgents(1,1);
        Debug.Log(ListeAgents[0].name);
    }

}