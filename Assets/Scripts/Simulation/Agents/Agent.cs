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
    /// Une référence à l'agent avec lequel il veut actuellement
    /// se reproduire.
    /// </summary>
    private GameObject _mate;

    /// <summary>
    /// Permet de récupérer depuis combien de temps nous avons pas
    /// vérifier si l'agent était bloqué.
    /// </summary>
    private float _lastWalkerUpdateAcc;

    /// <summary>
    /// La dernière position avant la dernière mise à jour.
    /// </summary>
    private Vector3 _lastPositionUpdate;

    /// <summary>
    /// Initialise toutes les valeurs des attributs et récupère les infos de l'agent
    ///
    /// Fait par Greg Demirdjian le 12/03/2022.
    /// Modifiée par EL MONTASER Osmane le 17/04/2022.
    /// </summary> 
    public void initialisation() {
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        AnimauxEnVisuel = new List<GameObject>();
        Preys = new List<string>();

        Attributes = AgentAttributes.GetAttributesDict();
        Attributes["SpeciesName"] = gameObject.name;
        if(!Attributes["SpeciesName"].Equals("Grass") && AgentMesh != null)
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
        Attributes["Health"] = Attributes["MaxHealth"];
        _lastPositionUpdate = transform.position;
        _lastWalkerUpdateAcc = 0.0f;
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

        if(_currentAction.Parent != null && _currentAction.ParentTransition != null && ActionTreeParser.CondTextToBool(_currentAction.ParentTransition, this, true)) {
            _currentAction = _currentAction.Parent;
        }
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
        if(Attributes != null) {

            if(bool.Parse(Attributes["IsPregnant"]))
                updatePregnancy();
            
            if(AgentMesh != null) {
                Attributes["Speed"] = ((float.Parse(Attributes["MaxSpeed"]) * (ActionNames.TimeSpeed / ActionNames.DAY_DURATION))).ToString();
                AgentMesh.speed = float.Parse(Attributes["Speed"]);
                AgentMesh.acceleration = float.Parse(Attributes["Speed"]);
                AnimauxEnVisuel = animauxDansFov();
                _lastWalkerUpdateAcc += (ActionNames.TimeSpeed / ActionNames.DAY_DURATION) * Time.deltaTime;

                //Vérifier si l'agent cherche à atteindre une destination impraticable.
                if(_lastWalkerUpdateAcc >= 1.0f && !(AgentMesh.remainingDistance >= 0.0f)) {
                    //Si l'agent est resté à la même position depuis 1 seconde.
                    if(Vector3.Distance(AgentMesh.destination, _lastPositionUpdate) <= AgentMesh.speed)
                        walker();
                    _lastWalkerUpdateAcc = 0.0f;
                    _lastPositionUpdate = transform.position;
                }
            }

            System.Double newValue;

            // si l'agent est en vie, on peut lui appliquer des comportements.
            if(bool.Parse(Attributes["IsAlive"])) {
                changeAction();
                _currentAction.Action.update();
            
                //affecterAnimations();

                testMort(); // teste si l'agent est en vie ou mort. modifie la variable EnVie

                // si l'agent est en digestion
                /*
                    if(Convert.ToDouble(Attributes["RemainingDigestionTime"]) > 0) {
                        newValue = Convert.ToDouble(Attributes["RemainingDigestionTime"]) - 0.2;
                        Attributes["RemainingDigestionTime"] = newValue.ToString();
                    }  
                */ 
                
                newValue = Convert.ToDouble(Attributes["Age"]) + (Time.deltaTime * (ActionNames.DAY_DURATION / ActionNames.TimeSpeed)) / 86400;
                Attributes["Age"] = newValue.ToString(); // on augmente l'âge de l'agent.
            } else {
                newValue = Convert.ToDouble(Attributes["CarcassEnergyContribution"]) - Time.deltaTime * 5;
                Attributes["CarcassEnergyContribution"] = newValue.ToString(); // la carcasse se déteriore et perd en apport énergétique.
                    // si la carcasse est presque vide.
                    if (Convert.ToDouble(Attributes["CarcassEnergyContribution"])<2.0) {
                        Destroy(this.gameObject); // on détruit l'objet.
                    }
            }
        }
    }

    private void updatePregnancy() {
        Attributes["GestationTimer"] = (Convert.ToDouble(Attributes["GestationTimer"]) + Time.deltaTime / ActionNames.TimeSpeed).ToString();

        if(Convert.ToDouble(Attributes["GestationTimer"]) >= Convert.ToDouble(Attributes["GestationPeriod"])) {
            //ACCOUCHEMENT
            System.Random rnd = new System.Random();
            float randomX = rnd.Next((int)transform.position.x - 1, (int)transform.position.x + 1);
            float randomY = rnd.Next((int)transform.position.z - 1, (int)transform.position.z + 1);
            int nbChildren = rnd.Next(1, int.Parse(Attributes["LitterMax"]));
            for(int i = 0; i < nbChildren; i++) {
                GameObject go = GameObject.Instantiate(gameObject, 
                    new Vector3(randomX, 
                    Terrain.activeTerrain.SampleHeight(new Vector3(randomX, 1f, randomY)),
                    randomY), Quaternion.identity);
                go.name = go.name.Split("(")[0];
                go.transform.localScale = go.transform.localScale * 0.5f;
                go.GetComponent<Agent>().initialisation();
                go.GetComponent<Agent>().Attributes["EnergyNeeds"] = "0.0";
                go.GetComponent<Agent>().Attributes["Stamina"] = "0.38";
                GameObject.Find("Player").GetComponent<DataUpdater>().AddNewAgent(go.GetComponent<Agent>());
            }
            Attributes["IsPregnant"] = "false";
            Attributes["GestationTimer"] = "0";
        }
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
    /// testMort : teste si l'agent est mort.
    ///
    /// Fait par Greg Demirdjian le 13/03/2022.
    /// </summary> 
    protected void testMort() {
        if(Attributes["SpeciesName"].Equals("Grass")) {
            if (Convert.ToDouble(Attributes["Health"]) <= 0) {
                Attributes["IsAlive"] = "false";
                Attributes["DeathCause"] = "A succombé à ses blessures.";
            }

            if (Convert.ToDouble(Attributes["Age"]) >= Convert.ToDouble(Attributes["MaxAge"])) {
                Attributes["IsAlive"] = "false";
                Attributes["DeathCause"] = "Mort de vieillesse.";
            }
        } else {
            if (Convert.ToDouble(Attributes["EnergyNeeds"]) >= Convert.ToDouble(Attributes["MaxEnergyNeeds"]) && !Attributes["SpeciesName"].Equals("Grass")) {
                Attributes["IsAlive"] = "false";
                Attributes["DeathCause"] = "Mort de faim.";
            }

            if (Convert.ToDouble(Attributes["WaterNeeds"]) >= 1) {
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
        }

        if (bool.Parse(Attributes["IsAlive"])==false) {
            if(!Attributes["SpeciesName"].Equals("Grass")) {
                AgentMesh.speed = 0.0f;
                AgentMesh.isStopped = true;
                Attributes["Speed"] = (0.0).ToString();

                Animation.ResetTrigger("AttackTrigger");
                Animation.ResetTrigger("IdleTrigger");
                Animation.ResetTrigger("WalkTrigger");
                Animation.ResetTrigger("EatTrigger");
                
                Animation.SetTrigger("DeadTrigger");
            }
            //Mise à jour dans la BDD :
            Db.SetDeathToAgent(Attributes["Id"], DataUpdater.CurrentTime, Attributes["DeathCause"]);
        }
    }

    public void walker() {
        NavMeshPath path = new NavMeshPath();
        Vector3 randomDirection;
        Vector3 finalPosition;

        do {
            randomDirection = UnityEngine.Random.insideUnitSphere * 150;
            randomDirection += transform.position;
            finalPosition = Vector3.zero;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 100, 1))
                finalPosition = hit.position;
        } while(!NavMesh.CalculatePath(transform.position, finalPosition, NavMesh.AllAreas, path));

        AgentMesh.SetDestination(finalPosition);
    }

    /// <summary>
    /// animauxDansFov : détecte les animaux dans le champ de vision de l'agent, le fov est modélisé par un cône
    /// retourne la liste des animaux visibles par l'agent
    ///
    /// Fait par Greg Demirdjian le 03/04/2022.
    /// </summary> 
    public List<GameObject> animauxDansFov()
    {
        List<GameObject> listeAnimauxEnVisuel = new List<GameObject>(); ;
        GameObject[] listeAnimaux;
        listeAnimaux = GameObject.FindGameObjectsWithTag("Animal");
        foreach (GameObject indexAnimal in listeAnimaux)
        {
            if (indexAnimal.GetComponent<Agent>().Attributes != null && Attributes != null && Attributes["Id"] != indexAnimal.GetComponent<Agent>().Attributes["Id"]) // on vérifie que l'on ne teste pas sur le meme agent
                if ((((Vector3.Distance(transform.position, indexAnimal.transform.position) <= Fov.range) //si l'animal est dans la portée de vue
                && (Vector3.Angle(transform.forward, indexAnimal.transform.position - transform.position) <= Fov.spotAngle / 2))) //  et si l'animal est dans l'angle de vue
                || ((Vector3.Distance(transform.position, indexAnimal.transform.position)<10.0f))) // ou si l'animal est très proche
                    {
                        listeAnimauxEnVisuel.Add(indexAnimal);
                    }
        }
        return listeAnimauxEnVisuel;
    }

    public void SetMate(GameObject mate) {
        _mate = mate;
    }

    public GameObject GetMate() {
        return _mate;
    }

    public string GetCurrentAction() {
        return _currentAction.Action.GetType().Name;
    }
}
