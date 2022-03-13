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

    public NavMeshAgent Agent;

    public Terrain Ter;

    public Animator Animation;

    public int Portee {get; set; }

    public double ApportEnergieCarcasse {get; set; }

    private double _besoinHydrique;

    public double BesoinHydriqueMax {get; set; }

    private bool _aSoif;

    private double _besoinEnergie;

    public double BesoinEnergieMax {get; set; }

    private bool _aFaim;

    private double _coordX;
    
    private double _coordY;

    private double _coordZ;

    private double _vitesse;

    public double VitesseMax {get; set; }

    public int Sexe {get; set; }

    private bool _veutSeReprod;

    private bool _estEnceinte;

    public double TempsGrossesse {get; set; }

    public double Age {get; set; }

    public double AgeMaturation {get; set; }

    public double AgeMax {get; set; }

    public bool EstAdulte {get; set; }

    public double TempsDigestion {get; set; }

    private double _tempsRestantDigestion;

    public double TempsConsoProie {get; set; }

    public int Pv {get; set; }

    public bool EnVie {get; set; }

    private bool _enFuite;

    private int _endurance;

    public int EnduranceMax {get; set; }

    public string CauseDeces {get; set; }

    public string NomEspece {get; set; }

    public int Id {get; set; }


    /// <summary>
    /// Initialise toutes les valeurs des attributs et récupère les infos de l'agent
    ///
    /// Fait par Greg Demirdjian le 12/03/2022.
    /// </summary> 
    private void initialisation()
    {
        _besoinHydrique = 0.0;
        _besoinEnergie = 0.0;
        _vitesse = 0.0;
        _veutSeReprod = false;
        _estEnceinte = false;
        Age = 0.0;
        EstAdulte = false;
        EnVie = true;
        _enFuite = false;
        _tempsRestantDigestion = 0.0;

        Agent.speed = _vitesse;
    }

    /// <summary>
    /// Start 
    ///
    /// Fait par Greg Demirdjian le 12/03/2022.
    /// </summary> 
    void Start()
    {
        initialisation();
    }

    /// <summary>
    /// Update : renverra vers les comportements en fonction des valeurs des variables de l'agent.
    ///
    /// Fait par Greg Demirdjian le 13/03/2022.
    /// </summary>    
    void Update()
    {
        testMort(); // teste si l'agent est en vie ou mort. modifie la variable EnVie

        if (EnVie) // si l'agent est en vie, on peut lui appliquer des comportements.
        {
            _besoinHydrique+=0.15; // on augmente les besoins hydriques et énergétiques de l'agent.
            _besoinEnergie+=0.1;

            affecterComportement();
            effectuerComportement();
        }    
        else
        {
            ApportEnergieCarcasse -= Time.deltaTime * 0.5; // la carcasse se déteriore et perd en apport énergétique.

            if (ApportEnergieCarcasse<2.0) // si la carcasse est presque vide.
                Destroy(this.gameObject); // on détruit l'objet.
        }        


    }

    /// <summary>
    /// affecterComportement : teste si les variables de comportemnts doivent être changées.
    ///
    /// Fait par Greg Demirdjian le 12/03/2022.
    /// </summary> 
    private void affecterComportement()
    {
        if (_besoinHydrique/BesoinHydriqueMax>0.50) // si l'agent est à 50% de ses besoins hydriques max.
            _aSoif = true;
        if (_besoinEnergie/BesoinEnergieMax>0.70) // si l'agent est à 70% de ses besoins énergétiques max.
            _aFaim = true;
        if ((Age>=AgeMaturation)&&(EstAdulte==false)) // si l'agent dépasse l'age de maturation.
            EstAdulte = true;
    } 

    /// <summary>
    /// effectuerComportement : lance les fonctions de comportement si ceux-ci sont actifs.
    ///
    /// Fait par Greg Demirdjian le 13/03/2022.
    /// </summary> 
    private void effectuerComportement()
    {
        if (_enFuite)
            Fuite();
        else if (_aSoif)
            Boire();
        else if (_aFaim)
            Manger();
        
    }

    /// <summary>
    /// testMort : teste si l'agent est mort.
    ///
    /// Fait par Greg Demirdjian le 13/03/2022.
    /// </summary> 
    private void testMort()
    {
        if (_besoinEnergie >= BesoinEnergieMax)
        {
            EnVie = false;
            CauseDeces = "Mort de faim.";
        }

        if (_besoinHydrique >= BesoinHydriqueMax)
        {
            EnVie = false;
            CauseDeces = "Mort de soif.";
        }

        if (Pv <= 0)
        {
            EnVie = false;
            CauseDeces = "Plus de points de vie.";
        }

        if (Age >= AgeMax)
        {
            EnVie = false;
            CauseDeces = "Mort de vieillesse.";
        }
    }

    /// <summary>
    /// Fuite : l'agent fuie. à écrire
    ///
    /// Fait par Greg Demirdjian le 13/03/2022.
    /// </summary> 
    void Fuite()
    {

    }

    /// <summary>
    /// Manger : l'agent cherche à se nourrir. Ici on doit distinguer les agents qui chassent de ceux qui se nourrissent d'autotrophes. à écrire
    ///
    /// Fait par Greg Demirdjian le 13/03/2022.
    /// </summary> 
    void Manger()
    {

    }

    /// <summary>
    /// Boire : l'agent cherche à boire. à écrire 
    ///
    /// Fait par Greg Demirdjian le 13/03/2022.
    /// </summary> 
    void Boire()
    {

    }

}