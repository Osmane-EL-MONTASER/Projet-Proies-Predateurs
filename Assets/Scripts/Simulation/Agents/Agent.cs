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
public class Agent : MonoBehaviour {
    public NavMeshAgent AgentMesh;

    public Light Fov;

    public Animator Animation;

    public List<GameObject> AnimauxEnVisuel;

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

    public int Id {get; set; }

    private List<int> _listeIdEspecesProies {get; set; }

    public GameObject AgentCible;

    public GameObject AgentCibleur;

    private int degatsAttaque {get; set; }


    /// <summary>
    /// Initialise toutes les valeurs des attributs et récupère les infos de l'agent
    ///
    /// Fait par Greg Demirdjian le 12/03/2022.
    /// </summary> 
    private void initialisation()
    {
        AnimauxEnVisuel = new List<GameObject>();
        _besoinHydrique = 0.0;
        _besoinEnergie = 0.0;
        _vitesse = 10.0;
        _veutSeReprod = false;
        _estEnceinte = false;
        Age = 0.0;
        AgeMax = 100.0 ; // à initialiser depuis la bdd
        Pv = 100; // à initialiser depuis la bdd
        BesoinEnergieMax = 200.0 ; // à initialiser depuis la bdd
        BesoinHydriqueMax = 200.0 ; // à initialiser depuis la bdd
        VitesseMax = 10.0 ; // à initialiser depuis la bdd
        EstAdulte = false;
        EnVie = true;
        _enFuite = false;
        _tempsRestantDigestion = 0.0;

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

            _besoinHydrique+=0.00015; // on augmente les besoins hydriques et énergétiques de l'agent.
            _besoinEnergie+=0.0001;
            Age+=0.00001; // on augmente l'âge de l'agent.

            AnimauxEnVisuel = animauxDansFov();
            affecterComportement();
            effectuerComportement();
        }    
        else
        {
            ApportEnergieCarcasse -= Time.deltaTime * 0.05; // la carcasse se déteriore et perd en apport énergétique.

            Debug.Log(CauseDeces);
            if (ApportEnergieCarcasse<2.0) // si la carcasse est presque vide.
                Destroy(this.gameObject); // on détruit l'objet.
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
        if (_besoinHydrique/BesoinHydriqueMax>0.50) // si l'agent est à 50% de ses besoins hydriques max.
            _aSoif = true;
        if (_besoinEnergie/BesoinEnergieMax>0.50) // si l'agent est à 70% de ses besoins énergétiques max.
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
        else if (AgentCible != null)
            chasser();
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
    /// chasser : l'agent chasse un autre agent
    ///
    /// Fait par Greg Demirdjian le 03/04/2022.
    /// </summary> 
    void chasser()
    {
        Agent animalTemp = AgentCible.GetComponent<Agent>();

        AgentMesh.SetDestination(AgentCible.transform.position);
        
        float dist = Vector3.Distance(transform.position, AgentCible.transform.position);
        
        if (dist <= 3.0f)
        {
            if (animalTemp.EnVie) // si la cible est en vie
            {
                animalTemp.Pv-= degatsAttaque; //l'agent attaque la cible
                // rajotuer les anim si dispo
            }
            else if (animalTemp.ApportEnergieCarcasse >= 10.0)
            {
                animalTemp.ApportEnergieCarcasse-= 5.0;
                _besoinEnergie-=5.0;
                if (_besoinEnergie<0.0)
                    _besoinEnergie = 0.0;
            }
        }

        if (animalTemp.ApportEnergieCarcasse < 10.0)
        {
            AgentCible = null;
        }

        if (_besoinEnergie/BesoinEnergieMax<=0.25)
        {
            _aFaim = false;
        }


    }

    /// <summary>
    /// chercherAManger : L'agent cherche d'autres agents et chasse ceux qui sont des potentielles proies
    /// 
    /// Fait par Greg Demirdjian le 03/04/2022.
    /// </summary> 
    void chercherAManger()
    {
        if (AnimauxEnVisuel.Count == 0) // s'il n'y a pas d'animaux que l'agent voit
        {
            AgentMesh.SetDestination(walker()); // il se déplace 
            if (_besoinEnergie/BesoinEnergieMax>0.75)// s'il a très faim
                AgentMesh.speed =  0.75f * (float) VitesseMax; // il se déplace plus vite
        }
        else // si l'agent voit des animaux 
        {
            int rangDistMin = -1;
            for(int i = 1 ; i < AnimauxEnVisuel.Count ; i++) // on cherche l'animal le plus proche parmi 
            {
                float distTemp = Vector3.Distance(transform.position, AnimauxEnVisuel[i].transform.position);
                if ((rangDistMin==-1)||(Vector3.Distance(transform.position, AnimauxEnVisuel[rangDistMin].transform.position) < distTemp))
                {
                    for (int j = 0 ; j < _listeIdEspecesProies.Count ; j++)
                    {
                        Agent animalTemp = AnimauxEnVisuel[i].GetComponent<Agent>();

                        if (_listeIdEspecesProies[j]==animalTemp.Id) // si l'ID de l'animal fait partie des ID des proies de l'agent.
                            rangDistMin=i; // on retient son rang dans la liste des animaux en visuel.
                    }

                }
            }

            if (rangDistMin != -1) // si un des animaux vus est une proie potentielle
            {
                AgentCible = AnimauxEnVisuel[rangDistMin];
            }
            else
            {
                AgentMesh.SetDestination(walker());
            }
                
        }

    }

    /// <summary>
    /// Manger : L'agent mange la proie passée en paramètre; influe sur la digestion et les besoin énergétiques.
    ///
    /// Fait par Greg Demirdjian le 13/03/2022.
    /// </summary> 
    IEnumerator Manger()
    {

        Agent proie = AgentCible.GetComponent<Agent>();;
        AgentMesh.isStopped = true;//l'agent s'arrête pour manger.
        /*Animation.SetBool("Walk", false);
        Animation.SetBool("Run", false);
        Animation.SetBool("Eat", true);*/

        if (_besoinEnergie - proie.ApportEnergieCarcasse < 0) // si les besoins de l'agent sont inférieurs aux apports de la carcasse.
        {
            _besoinEnergie = 0.0; // l'agent récupère les apports jusqu'à ne plus avoir de besoins.
            proie.ApportEnergieCarcasse -= _besoinEnergie;
        }
        else // la carcasse est trop faible en apports pour rassasier complètement l'agent.
        {
            _besoinEnergie -= proie.ApportEnergieCarcasse; // l'agent finit la carcasse.
            proie.ApportEnergieCarcasse = 0.0;
        }

        if (_besoinEnergie/BesoinEnergieMax < 0.20) // si l'agent a suffisamment mangé.
            _aFaim = false; // il n'a plus faim.

        _tempsRestantDigestion = TempsDigestion;

        yield return new WaitForSeconds((float)TempsConsoProie);//le prédateur consomme sa proie pendant un certain temps.


        //Animation.SetBool("Eat", false);
        //Animation.SetBool("Walk", true);

        // modifier _besoinEnergie en conséquence

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
            
            yield return new WaitForSeconds((float)_besoinHydrique/10.0f); //Il boit pendant un certain temps.

            _besoinHydrique = 0.0;

            _aSoif = false;

            //Animation.SetBool("Eat", false);
            //Animation.SetBool("Walk", true);

            AgentMesh.isStopped = false;
        }
    }

    Vector3 walker()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 100;
        randomDirection += transform.position;
        Vector3 finalPosition = Vector3.zero;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 100, 1));
            finalPosition = hit.position;

        return finalPosition;   
    }

    /// <summary>
    /// animauxDansFov : détecte les animaux dans le champ de vision de l'agent, le fov est modélisé par un cône
    /// retourne la liste des animaux visibles par l'agent
    ///
    /// Fait par Greg Demirdjian le 03/04/2022.
    /// </summary> 
    List<GameObject> animauxDansFov()
    {
        List<GameObject> listeAnimauxEnVisuel = new List<GameObject>();;
        GameObject[] listeAnimaux;
        listeAnimaux = GameObject.FindGameObjectsWithTag("Animal");

        foreach (GameObject indexAnimal in listeAnimaux)
        {
            if (this.name != indexAnimal.name) // on vérifie que l'on ne teste pas sur le meme agent
                if (Vector3.Distance(transform.position, indexAnimal.transform.position) <= Fov.range)// si l'animal est dans la portée de vue
                    if (Vector3.Angle(transform.forward, indexAnimal.transform.position - transform.position) <= Fov.spotAngle/2)// si l'animal est dans l'angle de vue
                        {
                            listeAnimauxEnVisuel.Add(indexAnimal);
                        }
        }

        return listeAnimauxEnVisuel;
    }

}