using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Singleton de Gestion des Global des Agents
///
/// Fait par AVERTY Pierre le 03/04/2022 inspiré par l'ancienne classe GestionAgents.
/// Modifiée le 10/04/2022 par AVERTY Pierre.
///
/// </summary> 
public class AgentManager : MonoBehaviour {
    /// <summary>
    /// Instance du singleton.
    /// </summary>
    private static AgentManager instance = null;
    private List<GameObject> InstanceList = new List<GameObject>();

    /// <summary>
    /// Instance du singleton.
    /// </summary>
    private List<GameObject> AgentList = new List<GameObject>();
    private List<GameObject> GhostList = new List<GameObject>();


    /// <summary>
    /// Type du nouvel agent qui va être ajouté.
    /// </summary>
    public string newAgentType;


    /// <summary>
    /// Les différents préfabs des agents contenue dans le singleton.
    /// </summary>
    public  GameObject Wolf;
    public  GameObject Iguana;
    public  GameObject Elephant;
    public  GameObject Pingouin;
    public  GameObject Snake;
    public  GameObject Lapin;
    public  GameObject Zebra;
    public  GameObject Tiger;
    public  GameObject Alligator;
    public  GameObject Lizard;
    public  GameObject Racoon;
    public  GameObject Tortoise;
    public  GameObject Grass;

    /// <summary>
    /// Les différents préfabs des agents fantôme contenue dans le singleton.
    /// </summary>
    public  GameObject WolfGhost;
    public  GameObject IguanaGhost;
    public  GameObject ElephantGhost;
    public  GameObject PingouinGhost;
    public  GameObject SnakeGhost;
    public  GameObject LapinGhost;
    public  GameObject ZebraGhost;
    public  GameObject TigerGhost;
    public  GameObject AlligatorGhost;
    public  GameObject LizardGhost;
    public  GameObject RacoonGhost;
    public  GameObject TortoiseGhost;

    /// <summary>
    /// Constructeur du singleton GestionAgents.
    /// 
    /// Fait par AVERTY Pierre le 03/04/2022. 
    /// </summary>
    private AgentManager() {

    }

    /// <summary>
    /// Méthode qui se lance au lancement du gameObject.
    /// 
    /// Fait par AVERTY Pierre le 10/04/2022. 
    /// </summary>
    public void Start() {
       GhostList.Add(WolfGhost);
       GhostList.Add(IguanaGhost);
       GhostList.Add(ElephantGhost);
       GhostList.Add(PingouinGhost);
       GhostList.Add(SnakeGhost);
       GhostList.Add(LapinGhost);
       GhostList.Add(ZebraGhost);
       GhostList.Add(AlligatorGhost);
       GhostList.Add(LizardGhost);
       GhostList.Add(RacoonGhost);
       GhostList.Add(TortoiseGhost);

       AgentList.Add(Wolf);
       AgentList.Add(Iguana);
       AgentList.Add(Elephant);
       AgentList.Add(Pingouin);
       AgentList.Add(Snake);
       AgentList.Add(Lapin);
       AgentList.Add(Zebra);
       AgentList.Add(Alligator);
       AgentList.Add(Lizard);
       AgentList.Add(Racoon);
       AgentList.Add(Tortoise);
       AgentList.Add(Grass);
    }

    /// <summary>
    /// Méthode qui gère l'ajout d'un nouveau fantome dans la simulation.
    /// 
    /// Fait par AVERTY Pierre le 03/04/2022.
    /// </summary>
    public void newGhostInSim(){
       GameObject ghost = instanciateGhost();        
    
       ghost = Instantiate(ghost, new Vector3(0f, 0f, 0f), Quaternion.identity);
    } 

    /// <summary>
    /// Méthode qui gère l'ajout d'un nouvel agent suite à la configuration.
    /// 
    /// Fait par AVERTY Pierre le 30/04/2022 et modifiée le 06/05/2022 basé sur une ancienne méthode faite par Greg Demirdjian.
    /// </summary>
    ///
    /// <param name="type">type de l'agent.</param>
    /// <param name="health">Santé de l'agent.</param>
    /// <param name="maxSpeed">Vitesse de l'agent maximum.</param>
    /// <param name="staminaMax">Stamina de l'agent maximum.</param>
    /// <param name="timeToLive">Durée de vie de l'agent.</param>
    /// <param name="n">Nombre d'agents.</param>
    public void initializationAgents(string type, double health, double maxSpeed, double staminaMax, double timeToLive, double n){
        // foreach(GameObject agent in InstanceList){
        //     if(agent.GetComponent<Agent>().Attributes["SpeciesName"].Equals(type)){
        //         InstanceList.Remove(agent);    
        //     }
        // }
        Debug.Log(type);
        for(int i = 0; i < n; i++){
            System.Random rnd = new System.Random();
            GameObject agent = instanciateAgent();

            newAgentType = type;

            Vector3 pos;
            UnityEngine.AI.NavMeshHit hit;
            do {
                float randomX = rnd.Next(0, 1000), randomY = rnd.Next(0, 1000);
                pos = new Vector3(randomX, Terrain.activeTerrain.SampleHeight(new Vector3(randomX, 1f, randomY)), randomY);
            } while (!UnityEngine.AI.NavMesh.SamplePosition(pos, out hit, 10.0f, 1));

        
            agent = Instantiate(agent, hit.position , Quaternion.identity);
            InstanceList.Add(agent);

            agent.name = agent.name.Replace("(Clone)","");
            // Agent script = agent.GetComponent<Agent>();
            // Dictionary<string, string> Attributes = AgentAttributes.GetAttributesDict();
            
            // Attributes["Health"] = health.ToString();
            // Attributes["SpeciesName"] = type;
            // Attributes["MaxStamina"] = staminaMax.ToString();
            // Attributes["MaxSpeed"] = maxSpeed.ToString();
            // Attributes["MaxAge"] = timeToLive.ToString();
            
            // script.Attributes = Attributes;
            // Debug.Log(script.Attributes);
            
        }
    } 
    
    /// <summary>
    /// Méthode qui gère l'ajout d'un nouvel agent dans la simulation.
    /// 
    /// Fait par AVERTY Pierre le 17/04/2022.
    /// </summary>
    ///
    /// <param name="coor">Coordonées de l'agent.</param>
    public void newAgentInSim(Vector3 coor){
       GameObject agent = instanciateAgent();        
       coor.y = 1f;

       agent = Instantiate(agent, coor, Quaternion.identity);
    } 

    /// <summary>
    /// Méthode qui instancie un fantôme.
    /// 
    /// Fait par AVERTY Pierre le 03/04/2022.
    /// </summary>
    public GameObject instanciateGhost(){
       return GhostList.Find(el => el.name  == newAgentType +" ghost");
    } 

    /// <summary>
    /// Méthode qui instancie un agent.
    /// 
    /// Fait par AVERTY Pierre le 17/04/2022.
    /// </summary>
    public GameObject instanciateAgent(){
       return AgentList.Find(el => el.name.ToUpper()  == newAgentType.ToUpper());
    } 

    /// <summary>
    /// Initialise toutes les valeurs des attributs et récupère les infos de l'agent
    ///
    /// Fait par Greg Demirdjian le 12/03/2022.
    /// Modifiée par EL MONTASER Osmane le 17/04/2022 et Pierre Averty le 06/05/2022.
    /// </summary> 
    // private void initialization() {
    //     Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
    //     AnimauxEnVisuel = new List<GameObject>();
    //     Preys = new List<string>();

    //     Attributes = AgentAttributes.GetAttributesDict();
    //     Attributes["SpeciesName"] = gameObject.name;
    //     Attributes["Health"] = "100";
    //     if(!Attributes["SpeciesName"].Equals("Grass") && AgentMesh != null)
    //         Attributes["Speed"] = AgentMesh.speed.ToString();
    //     Attributes["Gender"] = (new System.Random().Next(2) + 1).ToString();
    //     Attributes["Id"] = Guid.NewGuid().ToString();
        
    //     Db = new("Data Source=tempDB.db;Version=3");
    //     Preys = Db.SelectPreysOf(Attributes["SpeciesName"]);

    //     //Ajout des données dans l'agent.
    //     Dictionary<string, double> data = Db.SelectSpeciesData(Attributes["SpeciesName"]);
    //     foreach(KeyValuePair<string, double> entry in data)
    //         Attributes[entry.Key] = entry.Value.ToString();
        
    //     ActionTreeNode<string> strATN = 
    //         ActionTreeParser.ReadFromXmlFile<ActionTreeNode<string>>(AGENT_RESOURCE_PATH +
    //          Attributes["SpeciesName"] + "/" + Attributes["SpeciesName"] + ".tree");

    //     _actionTree = ActionTreeParser.ParseStringActionTree(strATN, this);
    //     _currentAction = _actionTree;

    //     Attributes["CarcassEnergyContribution"] = (200.0).ToString(); // a changer dans la bdd
    //     Attributes["Ad"] = (1.0).ToString(); // a changer dans la bdd
    //     Attributes["MaxEnergyNeeds"] = (1.0).ToString();
    // }

    /// <summary>
    /// Méthode qui crée l'instance du singleton et si elle existe déjà, la retourne.
    /// 
    /// Fait par AVERTY Pierre le 03/04/2022.
    /// </summary>
    public static AgentManager Instance {
        get {
            if (instance==null) {
                instance = new AgentManager();
            }
            return instance;
        }
    }

    /// <summary>
    /// Méthode qui s'assure que le singleton n'est pas supprimé.
    /// 
    /// Fait par AVERTY Pierre le 03/04/2022.
    /// </summary>
    private void Awake(){
        if (instance != null && instance != this){
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }
}
