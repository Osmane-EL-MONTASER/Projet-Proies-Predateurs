using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


/// <summary>
/// Classe des Agents
///
/// Fait par Greg Demirdjian le 12/03/2022.
/// </summary> 
public class Agent : MonoBehaviour
{

    public NavMeshAgent agent;

    private Terrain ter;

    private Animator animation;

    private int portee;

    private double besoinHydrique;

    private double besoinHydriqueMax;

    private double besoinEnergie;

    private double besoinEnergieMax;

    private double coordX;
    
    private double coordY;

    private double coordZ;

    private double vitesse;

    private double vitesseMax;

    private int sexe;

    private bool veutSeReprod;

    private bool estEnceinte;

    private double tempsGrossesse;

    private double age;

    private double ageMaturation;

    private double ageMax;

    private bool estAdulte;

    private int tempsDigestion;

    private double tempsRestantDigestion;

    private int tempsConsoProie;

    private int pv;

    private bool enVie;

    private bool enFuite;

    private int endurance;

    private int enduranceMax;

    private string causeDeces;

    private string nomEspece;

    public int id;


    /// <summary>
    /// Initialise toutes les valeurs des attributs et récupère les infos de l'agent
    ///
    /// Fait par Greg Demirdjian le 12/03/2022.
    /// </summary> 
    void initialisation()
    {
        age = 0;
        vitesse = 0.0;
        veutSeReprod = false;
        estEnceinte = false;
        estAdulte = false;
        enVie = true;
        enFuite = false;

        //rajouter la recup d'info depuis la bdd
    }

    /// <summary>
    /// Start 
    ///
    /// Fait par Greg Demirdjian le 12/03/2022.
    /// </summary> 
    void Start()
    {

    }



}