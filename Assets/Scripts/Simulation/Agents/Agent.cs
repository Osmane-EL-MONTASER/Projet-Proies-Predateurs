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
    public Animator Animation;
    public GameObject AgentCible;
    public List<GameObject> AnimauxEnVisuel;
    public Light Fov;
    public List<string> preys;

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
    protected TreeEditor.ActionTreeNode<IdleAgentAction> _actionTree;

    /// <summary>
    /// L'action courante que l'agent est en train de réaliser.
    /// </summary>
    protected TreeEditor.ActionTreeNode<AgentAction> _currentAction;
    
    /// <summary>
    /// Initialise toutes les valeurs des attributs et récupère les infos de l'agent
    ///
    /// Fait par Greg Demirdjian le 12/03/2022.
    /// </summary> 
    protected void initialisation() {
        AnimauxEnVisuel = new List<GameObject>();
        preys = new List<string>();

        Attributes = AgentAttributes.GetAttributesDict();
        Attributes["Health"] = "100";
        Attributes["Speed"] = "10";
        Attributes["SpeciesName"] = gameObject.name;
        Attributes["Gender"] = (new System.Random().Next(2) + 1).ToString();
        Attributes["Id"] = Guid.NewGuid().ToString();

        
        //POUR UTILISER LA BDD
        DBHelper db = new("Data Source=tempDB.db;Version=3");
        preys = db.SelectPreysOf(Attributes["SpeciesName"]);

        //Ajout des données dans l'agent.
        Dictionary<string, double> data = db.SelectSpeciesData(Attributes["SpeciesName"]);
        foreach(KeyValuePair<string, double> entry in data)
            Attributes[entry.Key] = entry.Value.ToString();
       /* foreach(string prey in preys)
            Debug.Log(prey);*/

        Attributes["CarcassEnergyContribution"] = (200.0).ToString(); // a changer dans la bdd
        Attributes["Ad"] = (0.1).ToString(); // a changer dans la bdd
    }

    /// <summary>
    /// Start 
    ///
    /// Fait par Greg Demirdjian le 12/03/2022.
    /// </summary> 
    void Start() {
        initialisation();
    }

    /// <summary>
    /// Update : renverra vers les comportements en fonction des valeurs des variables de l'agent.
    ///
    /// Fait par Greg Demirdjian le 13/03/2022.
    /// </summary>    
    void Update() {
        testMort(); // teste si l'agent est en vie ou mort. modifie la variable EnVie
        System.Double newValue;

        // si l'agent est en vie, on peut lui appliquer des comportements.
        if(bool.Parse(Attributes["IsAlive"])) {

            // si l'agent est en digestion
            if(Convert.ToDouble(Attributes["RemainingDigestionTime"]) > 0) {
                newValue = Convert.ToDouble(Attributes["RemainingDigestionTime"]) - 0.2;
                Attributes["RemainingDigestionTime"] = newValue.ToString();
            }   
            
            newValue = Convert.ToDouble(Attributes["WaterNeeds"]) + 0.00015;
            Attributes["WaterNeeds"] = newValue.ToString(); // on augmente les besoins hydriques et énergétiques de l'agent.
            newValue = Convert.ToDouble(Attributes["EnergyNeeds"]) + 0.0001;
            Attributes["EnergyNeeds"] = newValue.ToString();
            newValue = Convert.ToDouble(Attributes["Age"]) + 0.00001;
            Attributes["Age"] = newValue.ToString(); // on augmente l'âge de l'agent.

            AnimauxEnVisuel = animauxDansFov();
            affecterComportement();
            effectuerComportement();
        }    
        else {

            if (Convert.ToDouble(Attributes["Speed"])!=0.0)
            {
                AgentMesh.isStopped = true;
                Attributes["Speed"] = (0.0).ToString();
            }


            newValue = Convert.ToDouble(Attributes["CarcassEnergyContribution"]) - Time.deltaTime * 0.05;
            Attributes["CarcassEnergyContribution"] = newValue.ToString(); // la carcasse se déteriore et perd en apport énergétique.

            if (Convert.ToDouble(Attributes["CarcassEnergyContribution"])<2.0) // si la carcasse est presque vide.
                Destroy(this.gameObject); // on détruit l'objet.
        }        

        /*if((AgentMesh != null) && (AgentMesh.remainingDistance <= AgentMesh.stoppingDistance)) {
            //Animation.SetBool("Running",true);
            //Animation.SetBool("Idle2",true);
            AgentMesh.SetDestination(walker());
        }*/

        //_currentAction.update();
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
        else */if (bool.Parse(Attributes["IsThirsty"]))
            Boire();
        else if (AgentCible != null)
            chasser();
        else if (bool.Parse(Attributes["IsHungry"]))
            chercherAManger();
        else if((AgentMesh != null) && (AgentMesh.remainingDistance <= AgentMesh.stoppingDistance))
            AgentMesh.SetDestination(walker());

    }
    

    /// <summary>
    /// testMort : teste si l'agent est mort.
    ///
    /// Fait par Greg Demirdjian le 13/03/2022.
    /// </summary> 
    protected void testMort(){
        if (Convert.ToDouble(Attributes["EnergyNeeds"]) >= Convert.ToDouble(Attributes["MaxEnergyNeeds"])) {
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
    }

    /// <summary>
    /// Fuite : l'agent fuie. à écrire
    ///
    /// Fait par Greg Demirdjian le 13/03/2022.
    /// </summary> 
    void Fuite() {

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

        if (dist <= 2.5f)
        {
            AgentMesh.isStopped = true;

            if (bool.Parse(animalTemp.Attributes["IsAlive"])) // si la cible est en vie
            {
                animalTemp.Attributes["Health"] = (Convert.ToDouble(animalTemp.Attributes["Health"]) - Convert.ToDouble(Attributes["Ad"])).ToString(); //l'agent attaque la cible
                // rajotuer les anim si dispo
            }
            else if (Convert.ToDouble(animalTemp.Attributes["CarcassEnergyContribution"]) >= 10.0)
            {
                animalTemp.Attributes["CarcassEnergyContribution"] = (Convert.ToDouble(animalTemp.Attributes["CarcassEnergyContribution"]) - 0.5).ToString();
                Attributes["EnergyNeeds"] = (Convert.ToDouble(Attributes["EnergyNeeds"]) - 0.5).ToString();
                if (Convert.ToDouble(Attributes["EnergyNeeds"]) < 0.0)
                    Attributes["EnergyNeeds"] = (0.0).ToString();
            }
        }
        else
        {
            AgentMesh.isStopped = false;
        }

        if (Convert.ToDouble(animalTemp.Attributes["CarcassEnergyContribution"]) < 10.0)
        {
            AgentMesh.isStopped = false;
            AgentCible = null;
            if ((Convert.ToDouble(Attributes["EnergyNeeds"]) / Convert.ToDouble(Attributes["MaxEnergyNeeds"]) <= 0.25)&&(Attributes["IsHungry"]=="true"))
            {
                Attributes["IsHungry"]="false";
            }           
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
            if((AgentMesh != null) && (AgentMesh.remainingDistance <= AgentMesh.stoppingDistance)) 
                AgentMesh.SetDestination(walker());// il se déplace 
            if (Convert.ToDouble(Attributes["EnergyNeeds"]) / Convert.ToDouble(Attributes["MaxEnergyNeeds"]) > 0.75)// s'il a très faim
                AgentMesh.speed = 0.75f * (float)Convert.ToDouble(Attributes["MaxSpeed"]); // il se déplace plus vite
        }
        else // si l'agent voit des animaux 
        {
            int rangDistMin = -1;
            for (int i = 0; i < AnimauxEnVisuel.Count; i++) // on cherche l'animal le plus proche parmi 
            {
                float distTemp = Vector3.Distance(transform.position, AnimauxEnVisuel[i].transform.position);
                if ((rangDistMin == -1) || (Vector3.Distance(transform.position, AnimauxEnVisuel[rangDistMin].transform.position) < distTemp))
                {
                    for (int j = 0; j < preys.Count; j++)
                    {
                        Agent animalTemp = AnimauxEnVisuel[i].GetComponent<Agent>();
                        if (preys[j] == animalTemp.Attributes["SpeciesName"]) // si l'ID de l'animal fait partie des ID des proies de l'agent.
                            rangDistMin = i; // on retient son rang dans la liste des animaux en visuel.
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
    IEnumerator Manger(Agent proie) {
        AgentMesh.isStopped = true;//l'agent s'arrête pour manger.
        /*Animation.SetBool("Walk", false);
        Animation.SetBool("Run", false);
        Animation.SetBool("Eat", true);*/

        // si les besoins de l'agent sont inférieurs aux apports de la carcasse.
        if (Convert.ToDouble(Attributes["EnergyNeeds"]) - Convert.ToDouble(proie.Attributes["CarcassEnergyContribution"]) < 0) {
            Attributes["EnergyNeeds"] = "0"; // l'agent récupère les apports jusqu'à ne plus avoir de besoins.
            proie.Attributes["CarcassEnergyContribution"] = (Convert.ToDouble(proie.Attributes["CarcassEnergyContribution"]) - Convert.ToDouble(Attributes["EnergyNeeds"])).ToString();
        }
        // la carcasse est trop faible en apports pour rassasier complètement l'agent.
        else {
            Attributes["EnergyNeeds"] = (Convert.ToDouble(Attributes["EnergyNeeds"]) - Convert.ToDouble(proie.Attributes["CarcassEnergyContribution"])).ToString(); // l'agent finit la carcasse.
            proie.Attributes["CarcassEnergyContribution"] = "0";
        }

        if (Convert.ToDouble(Attributes["EnergyNeeds"])  / Convert.ToDouble(Attributes["MaxEnergyNeeds"])  < 0.20) // si l'agent a suffisemment mangé.
            Attributes["IsHungry"] = "false"; // il n'a plus faim.

        Attributes["RemainingDigestionTime"] = (Convert.ToDouble(Attributes["DigestionTime"])).ToString();

        yield return new WaitForSeconds((float)Convert.ToDouble(Attributes["PreyConsumptionTime"]));//le prédateur consomme sa proie pendant un certain temps.


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
    IEnumerator Boire() {
        GameObject eauP = null; //Variable permettant de représenter le point d'eau le plus proche.
        System.Double distance; //variable permettant de stocker la distance entre l'agent et un point d'eau.
        System.Double distanceMin = System.Double.PositiveInfinity; ; //variable permettant de stocker la plus petite distance entre l'agent et le point d'eau le plus proche.
        GameObject[] eaux = GameObject.FindGameObjectsWithTag("pointEau"); // On stocke tous les points d'eau du terrain dans un tableau.

        //On recherche le point d'eau le plus proche.
        for (int i = 0; i < eaux.Length; i++) {
            distance = Vector3.Distance(AgentMesh.transform.position, eaux[i].transform.position);
            if (distance < distanceMin) {
                eauP = eaux[i];
                distanceMin = distance;
            }
        }

        AgentMesh.SetDestination(eauP.transform.position); //L'agent se déplace vers le point d'eau le plus proche.

        //Si l'agent est assez proche du point d'eau...
        if (eauP != null && Vector3.Distance(AgentMesh.transform.position, eauP.transform.position) < 1f) {
            AgentMesh.isStopped = true; //Il s'arrête
            //Animation.SetBool("Walk", false);
            //Animation.SetBool("Eat", true);
            
            yield return new WaitForSeconds((float)Convert.ToDouble(Attributes["WaterNeeds"])/10f); //Il boit pendant un certain temps.

            Attributes["WaterNeeds"] = "0";

            Attributes["IsThirsty"] = "false";

            //Animation.SetBool("Eat", false);
            //Animation.SetBool("Walk", true);

            AgentMesh.isStopped = false;
        }
    }

    Vector3 walker() {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 100;
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
        List<GameObject> listeAnimauxEnVisuel = new List<GameObject>(); ;
        GameObject[] listeAnimaux;
        listeAnimaux = GameObject.FindGameObjectsWithTag("Animal");
        foreach (GameObject indexAnimal in listeAnimaux)
        {
            if (Attributes["Id"] != indexAnimal.GetComponent<Agent>().Attributes["Id"]) // on vérifie que l'on ne teste pas sur le meme agent
                if (Vector3.Distance(transform.position, indexAnimal.transform.position) <= Fov.range)// si l'animal est dans la portée de vue
                    if (Vector3.Angle(transform.forward, indexAnimal.transform.position - transform.position) <= Fov.spotAngle / 2)// si l'animal est dans l'angle de vue
                    {
                        listeAnimauxEnVisuel.Add(indexAnimal);
                    }
        }
        return listeAnimauxEnVisuel;
    }

}
