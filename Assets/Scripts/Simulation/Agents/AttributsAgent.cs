using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


/// <summary>
/// Classe avec les attributs des agents
/// Classe codé par HAMICHE Bilal le 09/04/2022
/// </summary> 
public class AttributsAgent : MonoBehaviour {
    public NavMeshAgent AgentMesh;

    public Light Fov;

    public Animator Animation;

    public List<GameObject> AnimauxEnVisuel;
    public bool isGhost;
    public int Portee;

    public double ApportEnergieCarcasse;

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

    public string Id {get; set; }

    public int IdEspece {get; set; }

    private List<int> _listeIdEspecesProies {get; set; }

    public GameObject AgentCible;

    public GameObject AgentCibleur;

    private int degatsAttaque {get; set; }



}