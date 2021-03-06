using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Threading;

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
    public  GameObject Rabbit;
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
    public  GameObject RabbitGhost;
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
       GhostList.Add(RabbitGhost);
       GhostList.Add(ZebraGhost);
       GhostList.Add(AlligatorGhost);
       GhostList.Add(LizardGhost);
       GhostList.Add(RacoonGhost);
       GhostList.Add(TortoiseGhost);
       GhostList.Add(TigerGhost);

       AgentList.Add(Wolf);
       AgentList.Add(Iguana);
       AgentList.Add(Elephant);
       AgentList.Add(Pingouin);
       AgentList.Add(Snake);
       AgentList.Add(Rabbit);
       AgentList.Add(Zebra);
       AgentList.Add(Alligator);
       AgentList.Add(Lizard);
       AgentList.Add(Racoon);
       AgentList.Add(Tortoise);
       AgentList.Add(Tiger);
       AgentList.Add(Grass);
    }

    /// <summary>
    /// Méthode qui gère l'ajout d'un nouveau fantome dans la simulation.
    /// 
    /// Fait par AVERTY Pierre le 03/04/2022.
    /// </summary>
    public void newGhostInSim(int n, Dictionary<string, string> Attributes){
       GameObject ghost = instanciateGhost();        
       ghost.GetComponent<Ghost>().n = n;
       ghost.GetComponent<Ghost>().Attributes = Attributes;
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
    public void initializationAgents(string type,  double CarcassEnergyContribution, double MaxWaterNeeds, double MaxEnergyNeeds, double MaxSpeed, double GestationPeriod, double MaturityAge, double MaxAge, double DigestionTime, double PreyConsumptionTime,
    double MaxHealth, double MaxStamina, double AttackDamage, int LitterMax, int n){

        for(int i = 0; i < InstanceList.Count;i++){
            if(InstanceList[i] != null)
                if(InstanceList[i].GetComponent<Agent>().name == type){
                    Destroy(InstanceList[i]); 
                }
        }
    
        string tempPath = "Data Source=tempDB.db;Version=3";

        DBHelper _dbHelper = new (tempPath);
        int id = _dbHelper.SelectSpeciesId(type);
        new Thread(() => {_dbHelper.UpdateSpeciesData(id, CarcassEnergyContribution, MaxWaterNeeds, MaxEnergyNeeds, MaxSpeed, GestationPeriod, MaturityAge, MaxAge, DigestionTime, PreyConsumptionTime, MaxHealth, MaxStamina, AttackDamage, LitterMax); }).Start();

        for(int i = 0; i < n; i++){
            System.Random rnd = new System.Random();
            GameObject agent = instanciateAgent();

            newAgentType = type;

            Vector3 pos;
            UnityEngine.AI.NavMeshHit hit;
            do {
                float randomX = rnd.Next(150, 1000), randomY = rnd.Next(150, 1000);
                pos = new Vector3(randomX, Terrain.activeTerrain.SampleHeight(new Vector3(randomX, 1f, randomY)), randomY);
            } while (!UnityEngine.AI.NavMesh.SamplePosition(pos, out hit, 10.0f, 1));

            agent = Instantiate(agent, hit.position , Quaternion.identity);
            InstanceList.Add(agent);

            agent.name = agent.name.Replace("(Clone)","");            
        }
    } 
    
    /// <summary>
    /// Méthode qui gère l'ajout d'un nouvel agent dans la simulation.
    /// 
    /// Fait par AVERTY Pierre le 17/04/2022.
    /// </summary>
    ///
    /// <param name="coor">Coordonées de l'agent.</param>
    public void newAgentInSim(Vector3 coor, Dictionary<string, string> Attributes){
       GameObject agent = instanciateAgent();        
       coor.y = Terrain.activeTerrain.SampleHeight(coor);

       agent = Instantiate(agent, coor, Quaternion.identity);
       agent.name = agent.name.Replace("(Clone)","");
       agent.GetComponent<Agent>().Attributes = Attributes;
    } 
    

    /// <summary>
    /// Méthode qui instancie un fantôme.
    /// 
    /// Fait par AVERTY Pierre le 03/04/2022.
    /// </summary>
    public GameObject instanciateGhost(){
       return GhostList.Find(el => el.name  == newAgentType.ToLower() +" ghost");
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
