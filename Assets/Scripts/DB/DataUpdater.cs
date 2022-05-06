using UnityEngine;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;


/// <summary>
/// Classe qui permet de récupérer les données de
/// chaque agent de la scène et de les enregistrer
/// dans la base de données temporairement avant
/// que l'utilisateur choisisse ou non d'enregistrer
/// les données.
/// 
/// Fait par EL MONTASER Osmane le 01/04/2022.
/// </summary>
public class DataUpdater : MonoBehaviour {
    /// <summary>
    /// La fréquence de sauvegarde des données de
    /// la simulation.
    /// </summary>
    public float SaveFrequency;

    /// <summary>
    /// La liste des game objects ayant le script
    /// des agents.
    /// </summary>
    private List<Agent> _agentList;

    /// <summary>
    /// Le temps écoulé depuis la dernière mise à
    /// jour du script.
    /// </summary>
    private float _saveFrequencyAccumulator;

    /// <summary>
    /// Le temps actuel au moment de la sauvegarde.
    /// </summary>
    public static float CurrentTime;

    /// <summary>
    /// Le numéro d'enregistrement des mesures.
    /// </summary>
    private int _recordNumber;

    /// <summary>
    /// L'instance de DBHelper utilisée afin de 
    /// manipuler la base de données.
    /// </summary>
    private DBHelper _dbHelper;

    private static bool _isBDDReset = false;

    /// <summary>
    /// Correspond à une liste de points valués
    /// formant la courbe du nombre de prédateurs
    /// en fonction du temps.
    /// </summary>
    public GameObject PredatorsLine;

    /// <summary>
    /// Correspond à une liste de points valués
    /// formant la courbe du nombre de proies
    /// en fonction du temps.
    /// </summary>
    public GameObject PreysLine;

    /// <summary>
    /// Correspond à une liste de points valués
    /// formant la courbe du nombre d'autotrophes
    /// en fonction du temps.
    /// </summary>
    public GameObject AutotrophsLine;

    /// <summary>
    /// Le graphe qui traque le nombre d'agents par
    /// catégorie dans la simulation en temps réel.
    /// </summary>
    public DD_DataDiagram AgentGraph;


    /// <summary>
    /// Crée au moment de la création du GameObject de
    /// créer / reset la base de données temporaire.
    /// 
    /// Fait par EL MONTASER Osmane le 01/04/2022.
    /// </summary>
    void Start() {
        string tempPath = "Data Source=tempDB.db;Version=3";
        if(!_isBDDReset) {
            File.Delete("tempDB.db");
            DBInit init = new DBInit("Data Source=tempDB.db;Version=3", "./Assets/Scripts/DB/tables_creation.sql");
            _isBDDReset = true;
        }

        _dbHelper = new DBHelper(tempPath);
        _recordNumber = _dbHelper.AddRecord(0.0f, 0.0f);

        _agentList = getAllGOAgents();
        
        foreach(Agent agent in _agentList) {
            if(agent.Attributes == null)
                agent.initialisation();
            string name = agent.gameObject.name;
            
            _dbHelper.AddAgent(agent.Attributes["Id"], name, .0f, -1.0f, _recordNumber, _dbHelper.SelectSpeciesId(name), Convert.ToInt32(agent.Attributes["Gender"]));
        }
        CurrentTime = .0f;
    }

    /// <summary>
    /// Met à jour la base de données à chaque tick
    /// en fonction des agents et des données de
    /// l'environnement de la simulation.
    /// 
    /// Fait par EL MONTASER Osmane le 01/04/2022.
    /// </summary>
    void Update() {
        if(_saveFrequencyAccumulator >= SaveFrequency) {
            saveAgentData(CurrentTime);
            _saveFrequencyAccumulator = .0f;
            CurrentTime += SaveFrequency;
        }

        _saveFrequencyAccumulator += Time.deltaTime;
    }

    //TEMP
    int oldPredCounter;
    int oldPreyCounter;
    int oldAutotrophCounter;

    /// <summary>
    /// Fonction qui permet de sauvegarder les données de
    /// chaque agent de la simulation dans la base de données
    /// temporaire.
    /// 
    /// Fait par EL MONTASER Osmane le 03/04/2022.
    /// </summary>
    /// <param name="time">
    /// Le temps t corrspondant à la mesure actuellement
    /// enregistrée.
    /// </param>
    private void saveAgentData(float time) {
        _dbHelper.UpdateRecord(_recordNumber, time);
        int predCounter = 0;
        int preyCounter = 0;
        int autotrophCounter = 0;

        AgentGraph.InputPoint(PredatorsLine, new Vector2(1, oldPredCounter));
        AgentGraph.InputPoint(PreysLine, new Vector2(1, oldPreyCounter));
        AgentGraph.InputPoint(AutotrophsLine, new Vector2(1, oldAutotrophCounter));

        //TEMPORAIRE
        string[] predators = {"Wolf2"};
        string[] preys = {"Rabbit"};
        string[] autotrophs = {"Grass"}; 
        _agentList = getAllGOAgents();

        foreach (Agent agent in _agentList) {
                if(agent != null && bool.Parse(agent.Attributes["IsAlive"])) {
                    string name = agent.Attributes["SpeciesName"].Split('(')[0];
                    if(Array.Exists(predators, n => n.Equals(name)))
                        predCounter++;
                    if(Array.Exists(preys, n => n.Equals(name)))
                        preyCounter++;
                    if(Array.Exists(autotrophs, n => n.Equals(name)))
                        autotrophCounter++;
                }
        }
        
        oldPredCounter = predCounter;
        oldPreyCounter = preyCounter;
        oldAutotrophCounter = autotrophCounter;

        new Thread(() => {
            foreach (Agent agent in _agentList) {
                if(agent != null) {
                    _dbHelper.AddAgentData(time, Convert.ToDouble(agent.Attributes["WaterNeeds"]) / Convert.ToDouble(agent.Attributes["MaxWaterNeeds"]), Convert.ToDouble(agent.Attributes["EnergyNeeds"]) / Convert.ToDouble(agent.Attributes["MaxEnergyNeeds"]), "test", agent.Attributes["Id"], -1);
                } else 
                    _agentList.Remove(agent);
            }
        }).Start();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="frequency"></param>
    public void SetSaveFrequency(float frequency) {
        SaveFrequency = frequency;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="agent"></param>
    public void AddNewAgent(Agent agent) {

    }

    /// <summary>
    /// Fonction qui permet de récupérer tous les game
    /// object des agents.
    /// 
    /// Fait par EL MONTASER Osmane le 02/04/2022.
    /// </summary>
    /// <returns>
    /// La liste des game objects ayant le script des
    /// agents.
    /// </returns>
    private List<Agent> getAllGOAgents() {
        GameObject[] allAnimals = GameObject.FindGameObjectsWithTag("Animal");
        List<Agent> objectList = new List<Agent>();

        //Récupérer uniquement les objets qui possèdent le script des agents.
        foreach(GameObject go in allAnimals)
            objectList.Add(go.GetComponent<Agent>());
        
        return objectList;
    }
}