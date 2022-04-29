using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe de Gestion des Agents
///
/// Fait par Greg Demirdjian et Bilal Hamiche le 19/03/2022.
/// Modifiée par AVERTY Pierre le 28/09/2022.
///
/// </summary> 
public class GestionAgents : MonoBehaviour
{

    /// <summary>
    /// Instance du singleton.
    /// </summary>
    private static GestionAgents instance = null;

    public List<GameObject> ListeAgents;

    public GameObject Wolf;
    public GameObject Iguana;
    public GameObject Elephant;
    public GameObject Pingouin;
    public GameObject Snake;
    public GameObject Rabbit;
    public GameObject Zebra;
    public GameObject Tiger;
    public GameObject Alligator;
    public GameObject Lizard;
    public GameObject Racoon;
    public GameObject Tortoise;

    /// <summary>
    /// Constructeur du singleton GestionAgents.
    /// 
    /// Fait par AVERTY Pierre le 28/03/2022.
    /// </summary>
    private GestionAgents() {
       
    }


    /// <summary>
    /// creerAgents : méthode qui fait apparaitre un nombre d'agents donnés et de l'espèce voulue.
    ///
    /// Fait par Greg Demirdjian et Bilal Hamiche le 19/03/2022.
    /// </summary> 
    public void creerAgents(int nombre, int idAgent)
    {
        if ((idAgent <=12)&&(idAgent>0))
            for (int i = 0 ; i < nombre ; i++)
            {
                GameObject tempGo = null;

                if (idAgent==1)
                    tempGo = Instantiate(Wolf, new Vector3(435f, 1f, 330f), Quaternion.identity);
                else if (idAgent==2)
                    tempGo = Instantiate(Iguana, new Vector3(445f, 1f, 310f), Quaternion.identity);
                //else if (idAgent==3)
                //    tempGo = Instantiate(Elephant, new Vector3(445f, 1f, 310f), Quaternion.identity); // le modèle de l'éléphant est dans la mauvaise direction
                else if (idAgent==4)
                    tempGo = Instantiate(Pingouin, new Vector3(445f, 1f, 310f), Quaternion.identity);
                else if (idAgent==5)
                    tempGo = Instantiate(Snake, new Vector3(445f, 1f, 310f), Quaternion.identity);
                else if (idAgent==6)
                    tempGo = Instantiate(Rabbit, new Vector3(445f, 1f, 310f), Quaternion.identity);
                else if (idAgent==7)
                    tempGo = Instantiate(Zebra, new Vector3(445f, 1f, 310f), Quaternion.identity);
                else if (idAgent==8)
                    tempGo = Instantiate(Tiger, new Vector3(445f, 1f, 310f), Quaternion.identity);
                else if (idAgent==9)
                    tempGo = Instantiate(Alligator, new Vector3(445f, 1f, 310f), Quaternion.identity);
                else if (idAgent==10)
                    tempGo = Instantiate(Lizard, new Vector3(445f, 1f, 310f), Quaternion.identity);
                else if (idAgent==11)
                    tempGo = Instantiate(Racoon, new Vector3(445f, 1f, 310f), Quaternion.identity);
                else if (idAgent==12)
                    tempGo = Instantiate(Tortoise, new Vector3(445f, 1f, 310f), Quaternion.identity);

                tempGo.name= tempGo.name.Replace("(Clone)","");

                ListeAgents.Add(tempGo);

            }


    }

    void Start()
    {
        ListeAgents = new List<GameObject>();
        creerAgents(4,1);
        //creerAgents(5,2);
        /*creerAgents(5,4);
        creerAgents(5,5);*/
        creerAgents(7,6);
        /*creerAgents(5,7);
        creerAgents(5,8);
        creerAgents(5,9);
        creerAgents(5,10);
        creerAgents(5,11);
        creerAgents(5,12);*/
    }

    /// <summary>
    /// Méthode qui crée l'instance du singleton et si elle existe déjà, la retourne.
    /// 
    /// Fait par AVERTY Pierre le 28/03/2022.
    /// </summary>
        public static GestionAgents Instance {
            get {
                if (instance==null) {
                    instance = new GestionAgents();
                }
                return instance;
            }
        }

}