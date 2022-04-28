using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.IO;
using System.Threading;
using System.Globalization;
using TreeEditor;

/// <summary>
/// Classe des Agents
///
/// Fait par Greg Demirdjian le 12/03/2022.
/// </summary> 
public class Agent : MonoBehaviour {
    public NavMeshAgent AgentMesh;
    public Animator Animation;
    public Light Fov;

    public GameObject AgentCible;
    public List<GameObject> AnimauxEnVisuel;
    public List<string> Preys;

    /// <summary>
    /// Tous les attributs de l'agent sont stockés à l'intérieur
    /// de ce dictionnaire afin de pouvoir y accéder via les arbres
    /// de décisions notamment.
    /// </summary>
    public Dictionary<string, string> Attributes;

    /// <summary>
    /// L'arbre de décisions qui permet à l'agent de savoir quoi faire
    /// à chaque instant.
    /// </summary>
    public ActionTreeNode<AgentAction> _actionTree;

    /// <summary>
    /// L'action courante que l'agent est en train de réaliser.
    /// </summary>
    public ActionTreeNode<AgentAction> _currentAction;

    private const string AGENT_RESOURCE_PATH = "./Assets/Resources/Agents/";

    public DBHelper Db;
    
    /// <summary>
    /// Initialise toutes les valeurs des attributs et récupère les infos de l'agent
    ///
    /// Fait par Greg Demirdjian le 12/03/2022.
    /// Modifiée par EL MONTASER Osmane le 17/04/2022.
    /// </summary> 
    protected void initialisation() {
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        AnimauxEnVisuel = new List<GameObject>();
        Preys = new List<string>();

        Attributes = AgentAttributes.GetAttributesDict();
        Attributes["SpeciesName"] = gameObject.name;
        Attributes["Health"] = "100";
        if(!Attributes["SpeciesName"].Equals("Grass"))
            Attributes["Speed"] = AgentMesh.speed.ToString();
        Attributes["Gender"] = (new System.Random().Next(2) + 1).ToString();
        Attributes["Id"] = Guid.NewGuid().ToString();
        
        Db = new("Data Source=tempDB.db;Version=3");
        Preys = Db.SelectPreysOf(Attributes["SpeciesName"]);

        //Ajout des données dans l'agent.
        Dictionary<string, double> data = Db.SelectSpeciesData(Attributes["SpeciesName"]);
        foreach(KeyValuePair<string, double> entry in data)
            Attributes[entry.Key] = entry.Value.ToString();
        
        ActionTreeNode<string> strATN = 
            ActionTreeParser.ReadFromXmlFile<ActionTreeNode<string>>(AGENT_RESOURCE_PATH +
             Attributes["SpeciesName"] + "/" + Attributes["SpeciesName"] + ".tree");

        _actionTree = ActionTreeParser.ParseStringActionTree(strATN, this);
        _currentAction = _actionTree;

        Attributes["CarcassEnergyContribution"] = (200.0).ToString(); // a changer dans la bdd
        Attributes["Ad"] = (1.0).ToString(); // a changer dans la bdd
        Attributes["MaxEnergyNeeds"] = (1.0).ToString();
    }

    /// <summary>
    /// Start 
    ///
    /// Fait par Greg Demirdjian le 12/03/2022.
    /// </summary> 
    void Start() {

        AgentMesh = gameObject.GetComponent<NavMeshAgent>();
        Animation = gameObject.GetComponent<Animator>();
        Fov = gameObject.GetComponent<Light>();

        initialisation();
    }

    protected void changeAction() {
        int i = 0;

        foreach(ActionTreeNode<AgentAction> action in _currentAction.Children) {
            if(ActionTreeParser.CondTextToBool(_currentAction.TransitionsCond[i], this)) {
                action.Parent = _currentAction;
                action.ParentTransition = _currentAction.TransitionsCond[i];
                
                _currentAction = action;
                break;
            }
            i++;
        }

        if(_currentAction.Parent != null && _currentAction.ParentTransition != null && ActionTreeParser.CondTextToBool(_currentAction.ParentTransition, this, true))
            _currentAction = _currentAction.Parent;
        
    } 
    public void ForceChangeAction(ActionTreeNode<AgentAction> newAction, string transition) {
        newAction.Parent = _currentAction;
        newAction.ParentTransition = transition;
                
        _currentAction = newAction;
    } 

    /// <summary>
    /// Update : renverra vers les comportements en fonction des valeurs des variables de l'agent.
    ///
    /// Fait par Greg Demirdjian le 13/03/2022.
    /// </summary>    
    void Update() {
        System.Double newValue;

        // si l'agent est en vie, on peut lui appliquer des comportements.
        if(bool.Parse(Attributes["IsAlive"])) {
            changeAction();
            _currentAction.Action.update();
        
            //affecterAnimations();

            testMort(); // teste si l'agent est en vie ou mort. modifie la variable EnVie

            // si l'agent est en digestion
            if(Convert.ToDouble(Attributes["RemainingDigestionTime"]) > 0) {
                newValue = Convert.ToDouble(Attributes["RemainingDigestionTime"]) - 0.2;
                Attributes["RemainingDigestionTime"] = newValue.ToString();
            }   
            
            /*
                newValue = Convert.ToDouble(Attributes["WaterNeeds"]) + 0.00015;
                Attributes["WaterNeeds"] = newValue.ToString(); // on augmente les besoins hydriques et énergétiques de l'agent.
                newValue = Convert.ToDouble(Attributes["EnergyNeeds"]) + 0.0001;
                Attributes["EnergyNeeds"] = newValue.ToString();
                newValue = Convert.ToDouble(Attributes["Age"]) + 0.00001;
                Attributes["Age"] = newValue.ToString(); // on augmente l'âge de l'agent.
            */

            if(!Attributes["SpeciesName"].Equals("Grass")){
                AnimauxEnVisuel = animauxDansFov();
                affecterComportement();
            }
                
            //effectuerComportement();
        }    
        else {
            newValue = Convert.ToDouble(Attributes["CarcassEnergyContribution"]) - Time.deltaTime * 5;
            Attributes["CarcassEnergyContribution"] = newValue.ToString(); // la carcasse se déteriore et perd en apport énergétique.
                // si la carcasse est presque vide.
                if (Convert.ToDouble(Attributes["CarcassEnergyContribution"])<2.0) {
                    Destroy(this.gameObject); // on détruit l'objet.
                }
        }

        /*if((AgentMesh != null) && (AgentMesh.remainingDistance <= AgentMesh.stoppingDistance)) {
            //Animation.SetBool("Running",true);
            //Animation.SetBool("Idle2",true);
            AgentMesh.SetDestination(walker());
        }*/

        //_currentAction.update();
    
    }


    /// <summary>
    /// affecterAnimations : affecte la bonne animation (si dispo) à l'agent
    ///
    /// Fait par Greg Demirdjian le 16/04/2022.
    /// </summary>    
    public void affecterAnimations() {

        if ((AgentMesh.isStopped == true) || (AgentMesh.speed <= 0.0) || (Convert.ToDouble(Attributes["Speed"]) <= 0)) // si l'agent est à l'arrêt 
        {
            Animation.ResetTrigger("WalkTrigger"); // on arrête l'animation de marche

            if (AgentCible != null) // si l'agent chasse
            {
                Animation.ResetTrigger("IdleTrigger");
                Animation.SetTrigger("AttackTrigger"); // on lui attribue l'animation d'attaque (s'il y en a une)
            }
            else // sinon
            {
                Animation.ResetTrigger("AttackTrigger");
                Animation.SetTrigger("IdleTrigger"); // on lui attribue l'animation de base de l'agent (s'il y en a une)
            }

            
        }
        else if (AgentMesh.isStopped == false)
        {
            Animation.SetTrigger("WalkTrigger");
            Animation.ResetTrigger("IdleTrigger");
            Animation.ResetTrigger("AttackTrigger");
        }



    }


    /// <summary>
    /// affecterComportement : teste si les variables de comportemnts doivent être changées.
    ///
    /// Fait par Greg Demirdjian le 12/03/2022.
    /// </summary> 
    protected void affecterComportement() {
        // si l'agent est à 50% de ses besoins hydriques max.
        if(Convert.ToDouble(Attributes["WaterNeeds"]) / Convert.ToDouble(Attributes["MaxWaterNeeds"]) > 0.50) 
            Attributes["IsThirsty"] = "true";
        
        // si l'agent est à 70% de ses besoins énergétiques max.
        if(Convert.ToDouble(Attributes["EnergyNeeds"]) / Convert.ToDouble(Attributes["MaxEnergyNeeds"]) > 0.70) 
            Attributes["IsHungry"] = "true";
        
        //si l'agent dépasse l'age de maturation.
        if((Convert.ToDouble(Attributes["Age"]) >= Convert.ToDouble(Attributes["MaturityAge"])) && (bool.Parse(Attributes["IsAdult"]) == false)) 
            Attributes["IsAdult"] = "true";
    } 

    
    /// <summary>
    /// effectuerComportement : lance les fonctions de comportement si ceux-ci sont actifs.
    ///
    /// Fait par Greg Demirdjian le 13/03/2022.
    /// </summary> 
    protected void effectuerComportement() {
       /* if (_enFuite)
            Fuite();
        else if (bool.Parse(Attributes["IsThirsty"]))
            Boire();
        else*/  
        if((AgentMesh != null) && (AgentMesh.remainingDistance <= AgentMesh.stoppingDistance))
            AgentMesh.SetDestination(walker());

    }
    

    /// <summary>
    /// testMort : teste si l'agent est mort.
    ///
    /// Fait par Greg Demirdjian le 13/03/2022.
    /// </summary> 
    protected void testMort() {
        if (Convert.ToDouble(Attributes["EnergyNeeds"]) >= Convert.ToDouble(Attributes["MaxEnergyNeeds"]) && !Attributes["SpeciesName"].Equals("Grass")) {
            Attributes["IsAlive"] = "false";
            Attributes["DeathCause"] = "Mort de faim.";
        }

        if (Convert.ToDouble(Attributes["WaterNeeds"]) >= Convert.ToDouble(Attributes["MaxWaterNeeds"])) {
            Attributes["IsAlive"] = "false";
            Attributes["DeathCause"] = "Mort de soif.";
        }

        if (Convert.ToDouble(Attributes["Health"]) <= 0) {
            Attributes["IsAlive"] = "false";
            Attributes["DeathCause"] = "A succombé à ses blessures.";
        }

        if (Convert.ToDouble(Attributes["Age"]) >= Convert.ToDouble(Attributes["MaxAge"])) {
            Attributes["IsAlive"] = "false";
            Attributes["DeathCause"] = "Mort de vieillesse.";
        }

        if (bool.Parse(Attributes["IsAlive"])==false) {
            AgentMesh.speed = 0.0f;
            AgentMesh.isStopped = true;
            Attributes["Speed"] = (0.0).ToString();

            Animation.ResetTrigger("AttackTrigger");
            Animation.ResetTrigger("IdleTrigger");
            Animation.ResetTrigger("WalkTrigger");
            Animation.ResetTrigger("EatTrigger");
            
            Animation.SetTrigger("DeadTrigger");

            //Mise à jour dans la BDD :
            Db.SetDeathToAgent(Attributes["Id"], TemporaryDataSaving.CurrentTime, Attributes["DeathCause"]);
        }
    }

    /// <summary>
    /// Fuite : l'agent fuie. à écrire
    ///
    /// Fait par Greg Demirdjian le 13/03/2022.
    /// </summary> 
    void Fuite() {

    }

    /// <summary>
    /// Boire : l'agent cherche à boire. à écrire 
    /// Inspirée de la fonction Boire du projet de l'an dernier et modifiée.
    ///
    /// Fait par Greg Demirdjian le 13/03/2022.
    /// </summary> 
    void Boire() {

        

        GameObject eauP = null; //Variable permettant de représenter le point d'eau le plus proche.
        double distance; //variable permettant de stocker la distance entre l'agent et un point d'eau.
        double distanceMin = System.Double.PositiveInfinity; ; //variable permettant de stocker la plus petite distance entre l'agent et le point d'eau le plus proche.
        GameObject[] eaux = GameObject.FindGameObjectsWithTag("pointEau"); // On stocke tous les points d'eau du terrain dans un tableau.
        //On recherche le point d'eau le plus proche.
        for (int i = 0 ; i < eaux.Length; i++) {
            distance = Vector3.Distance(AgentMesh.transform.position, eaux[i].transform.position);
            if (distance < distanceMin) 
            {
                eauP = eaux[i];
                distanceMin = distance;
            }
             
        }

        if (eauP == null)
            AgentMesh.SetDestination(walker());
        

        RaycastHit hit;

        Vector3 direc = eauP.transform.position - transform.position;
        direc.x = - direc.x;
        direc.z = - direc.z;

        if (Physics.Raycast(new Vector3(eauP.transform.position.x, eauP.transform.position.y +1.0f, eauP.transform.position.z), direc, out hit, Mathf.Infinity)) 
        {
            AgentMesh.SetDestination(hit.point);
        }
Debug.Log((Vector3.Distance(hit.point, transform.position)));
        //Si l'agent est assez proche du point d'eau...
        if ((eauP != null) && (Vector3.Distance(hit.point, transform.position) < 10.0f))
        { 
            AgentMesh.isStopped = true; //Il s'arrête

            Attributes["WaterNeeds"] = "0";

            Attributes["IsThirsty"] = "false";

            AgentMesh.isStopped = false;
        }
    }

    public Vector3 walker() {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 50;
        randomDirection += transform.position;
        Vector3 finalPosition = Vector3.zero;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 100, 1))
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
        List<GameObject> listeAnimauxEnVisuel = new List<GameObject>(); ;
        GameObject[] listeAnimaux;
        listeAnimaux = GameObject.FindGameObjectsWithTag("Animal");
        foreach (GameObject indexAnimal in listeAnimaux)
        {
            if (Attributes["Id"] != indexAnimal.GetComponent<Agent>().Attributes["Id"]) // on vérifie que l'on ne teste pas sur le meme agent
                if ((((Vector3.Distance(transform.position, indexAnimal.transform.position) <= Fov.range) //si l'animal est dans la portée de vue
                && (Vector3.Angle(transform.forward, indexAnimal.transform.position - transform.position) <= Fov.spotAngle / 2))) //  et si l'animal est dans l'angle de vue
                || ((Vector3.Distance(transform.position, indexAnimal.transform.position)<10.0f))) // ou si l'animal est très proche
                    {
                        listeAnimauxEnVisuel.Add(indexAnimal);
                    }
        }
        return listeAnimauxEnVisuel;
    }

}
