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
    public GameObject Elephant;
    public GameObject Pingouin;
    public GameObject Snake;
    public GameObject Rabbit;
    public GameObject Zebra;


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
            else if (idAgent==3)
                ListeAgents.Add(Instantiate(Elephant, new Vector3(445f, 1f, 310f), Quaternion.identity));
            else if (idAgent==4)
                ListeAgents.Add(Instantiate(Pingouin, new Vector3(445f, 1f, 310f), Quaternion.identity));
            else if (idAgent==5)
                ListeAgents.Add(Instantiate(Snake, new Vector3(445f, 1f, 310f), Quaternion.identity));
            else if (idAgent==6)
                ListeAgents.Add(Instantiate(Rabbit, new Vector3(445f, 1f, 310f), Quaternion.identity));
            else if (idAgent==7)
                ListeAgents.Add(Instantiate(Zebra, new Vector3(445f, 1f, 310f), Quaternion.identity));
        }


    }

    void Start()
    {
        ListeAgents = new List<GameObject>();
        creerAgents(10,1);
        creerAgents(5,4);
        creerAgents(5,5);
        creerAgents(5,6);
        creerAgents(5,7);
    }

}