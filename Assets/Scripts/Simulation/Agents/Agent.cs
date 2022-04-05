using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


/// <summary>
/// Classe des Agents
///
/// Fait par Greg Demirdjian le 12/03/2022.
/// </summary> 
public class Agent : MonoBehaviour {
    public NavMeshAgent AgentMesh;

    public Terrain Ter;

    public Animator Animation;

    public int Portee {get; set; }

    public double ApportEnergieCarcasse {get; set; }

    public double BesoinHydrique { get; private set; }

    public double BesoinHydriqueMax {get; set; }

    private bool _aSoif;

    public double BesoinEnergie { get; private set; }

    public double BesoinEnergieMax {get; set; }

    private bool _aFaim;

    private double _coordX;
    
    private double _coordY;

    private double _coordZ;

    private double _vitesse;

    public double VitesseMax { get; set; }

    public int Sexe { get; set; }

    private bool _veutSeReprod;

    private bool _estEnceinte;

    public double TempsGrossesse { get; set; }

    public double Age { get; set; }

    public double AgeMaturation { get; set; }

    public double AgeMax { get; set; }

    public bool EstAdulte { get; set; }

    public double TempsDigestion { get; set; }

    private double _tempsRestantDigestion;

    public double TempsConsoProie { get; set; }

    public int Pv { get; set; }

    public bool EnVie { get; set; }

    private bool _enFuite;

    private int _endurance;

    public int EnduranceMax { get; set; }

    public string CauseDeces { get; set; }

    public string NomEspece { get; set; }

    public string Id { get; set; }
    
    /// <summary>
    /// Initialise toutes les valeurs des attributs et récupère les infos de l'agent
    ///
    /// Fait par Greg Demirdjian le 12/03/2022.
    /// </summary> 
    private void initialisation()
    {
        BesoinHydrique = 0.0;
        BesoinEnergie = 0.0;
        BesoinHydriqueMax = 100;
        BesoinEnergieMax = 100;
        _vitesse = 10.0;
        _veutSeReprod = false;
        _estEnceinte = false;
        Age = 0.0;
        EstAdulte = false;
        EnVie = true;
        _enFuite = false;
        _tempsRestantDigestion = 0.0;
        NomEspece = gameObject.name;
        Sexe = new System.Random().Next(2) + 1;
        Id = Guid.NewGuid().ToString();

        //AgentMesh.speed = (float)_vitesse;
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
            if (_tempsRestantDigestion > 0.0) // si l'agent est en digestion
                _tempsRestantDigestion-=0.2; 

            BesoinHydrique+=0.15; // on augmente les besoins hydriques et énergétiques de l'agent.
            BesoinEnergie+=0.1;
            Age+=0.05; // on augmente l'âge de l'agent.

            affecterComportement();
            effectuerComportement();
        }    
        else
        {
            ApportEnergieCarcasse -= Time.deltaTime * 0.5; // la carcasse se déteriore et perd en apport énergétique.

            //if (ApportEnergieCarcasse<2.0) // si la carcasse est presque vide.
                //Destroy(this.gameObject); // on détruit l'objet.
        }        

        if ((AgentMesh!=null)&&(AgentMesh.remainingDistance<=AgentMesh.stoppingDistance))
        {
            //Animation.SetBool("Running",true);
            //Animation.SetBool("Idle2",true);
            AgentMesh.SetDestination(walker());
        }
 
    }

    /// <summary>
    /// affecterComportement : teste si les variables de comportemnts doivent être changées.
    ///
    /// Fait par Greg Demirdjian le 12/03/2022.
    /// </summary> 
    private void affecterComportement()
    {
        if (BesoinHydrique/BesoinHydriqueMax>0.50) // si l'agent est à 50% de ses besoins hydriques max.
            _aSoif = true;
        if (BesoinEnergie/BesoinEnergieMax>0.70) // si l'agent est à 70% de ses besoins énergétiques max.
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
            chercherAManger();
        
    }

    /// <summary>
    /// testMort : teste si l'agent est mort.
    ///
    /// Fait par Greg Demirdjian le 13/03/2022.
    /// </summary> 
    private void testMort()
    {
        if (BesoinEnergie >= BesoinEnergieMax)
        {
            EnVie = false;
            CauseDeces = "Mort de faim.";
        }

        if (BesoinHydrique >= BesoinHydriqueMax)
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
    /// chercherAManger : l'agent cherche à se nourrir. Ici on doit distinguer les agents qui chassent de ceux qui se nourrissent d'autotrophes. à écrire.
    /// s'inspirer de la fonction chasser() pour les prédateurs
    /// Fait par Greg Demirdjian le 13/03/2022.
    /// </summary> 
    void chercherAManger()
    {
        
    }

    /// <summary>
    /// Manger : L'agent mange la proie passée en paramètre; influe sur la digestion et les besoin énergétiques.
    ///
    /// Fait par Greg Demirdjian le 13/03/2022.
    /// </summary> 
    IEnumerator Manger(Agent proie)
    {
        AgentMesh.isStopped = true;//l'agent s'arrête pour manger.
        /*Animation.SetBool("Walk", false);
        Animation.SetBool("Run", false);
        Animation.SetBool("Eat", true);*/

        if (BesoinEnergie - proie.ApportEnergieCarcasse < 0) // si les besoins de l'agent sont inférieurs aux apports de la carcasse.
        {
            BesoinEnergie = 0.0; // l'agent récupère les apports jusqu'à ne plus avoir de besoins.
            proie.ApportEnergieCarcasse -= BesoinEnergie;
        }
        else // la carcasse est trop faible en apports pour rassasier complètement l'agent.
        {
            BesoinEnergie -= proie.ApportEnergieCarcasse; // l'agent finit la carcasse.
            proie.ApportEnergieCarcasse = 0.0;
        }

        if (BesoinEnergie/BesoinEnergieMax < 0.20) // si l'agent a suffisemment mangé.
            _aFaim = false; // il n'a plus faim.

        _tempsRestantDigestion = TempsDigestion;

        yield return new WaitForSeconds((float)TempsConsoProie);//le prédateur consomme sa proie pendant un certain temps.


        //Animation.SetBool("Eat", false);
        //Animation.SetBool("Walk", true);

        // modifier BesoinEnergie en conséquence

        AgentMesh.isStopped = false;// le prédateur n'est plus à l'arrêt.

    }

    /// <summary>
    /// Boire : l'agent cherche à boire. à écrire 
    /// Inspirée de la fonction Boire du projet de l'an dernier et modifiée.
    ///
    /// Fait par Greg Demirdjian le 13/03/2022.
    /// </summary> 
    IEnumerator  Boire()
    {
        GameObject eauP = null; //Variable permettant de représenter le point d'eau le plus proche.
        double distance; //variable permettant de stocker la distance entre l'agent et un point d'eau.
        double distanceMin = double.PositiveInfinity; ; //variable permettant de stocker la plus petite distance entre l'agent et le point d'eau le plus proche.
        GameObject[] eaux = GameObject.FindGameObjectsWithTag("pointEau"); // On stocke tous les points d'eau du terrain dans un tableau.

        for (int i = 0; i < eaux.Length; i++) //On recherche le point d'eau le plus proche.
        {
            distance = Vector3.Distance(AgentMesh.transform.position, eaux[i].transform.position);
            if (distance < distanceMin)
            {
                eauP = eaux[i];
                distanceMin = distance;
            }
        }

        AgentMesh.SetDestination(eauP.transform.position); //L'agent se déplace vers le point d'eau le plus proche.

        if (eauP != null && Vector3.Distance(AgentMesh.transform.position, eauP.transform.position) < 1f) //Si l'agent est assez proche du point d'eau...
        {
            AgentMesh.isStopped = true; //Il s'arrête
            //Animation.SetBool("Walk", false);
            //Animation.SetBool("Eat", true);
            
            yield return new WaitForSeconds((float)BesoinHydrique/10.0f); //Il boit pendant un certain temps.

            BesoinHydrique = 0.0;

            _aSoif = false;

            //Animation.SetBool("Eat", false);
            //Animation.SetBool("Walk", true);

            AgentMesh.isStopped = false;
        }
    }

    Vector3 walker()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 100;
        randomDirection += transform.position;
        Vector3 finalPosition = Vector3.zero;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 100, 1));
            finalPosition = hit.position;

        return finalPosition;   
    }

}