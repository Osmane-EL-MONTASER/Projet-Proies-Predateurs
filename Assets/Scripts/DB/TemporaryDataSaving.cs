using UnityEngine;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System;


/// <summary>
/// Classe qui permet de récupérer les données de
/// chaque agent de la scène et de les enregistrer
/// dans la base de données temporairement avant
/// que l'utilisateur choisisse ou non d'enregistrer
/// les données.
/// 
/// Fait par EL MONTASER Osmane le 01/04/2022.
/// </summary>
public class TemporaryDataSaving : MonoBehaviour {
    /// <summary>
    /// La fréquence de sauvegarde des données de
    /// la simulation.
    /// </summary>
    public float SaveFrequency;

    /// <summary>
    /// La liste des game objects ayant le script
    /// des agents.
    /// </summary>
    private List<GameObject> _agentList;

    /// <summary>
    /// Le temps écoulé depuis la dernière mise à
    /// jour du script.
    /// </summary>
    private float _saveFrequencyAccumulator;

    /// <summary>
    /// Le temps actuel au moment de la sauvegarde.
    /// </summary>
    private float _time;

    /// <summary>
    /// Le numéro d'enregistrement des mesures.
    /// </summary>
    private int _recordNumber;

    /// <summary>
    /// L'instance de DBHelper utilisée afin de 
    /// manipuler la base de données.
    /// </summary>
    private DBHelper _dbHelper;

    /// <summary>
    /// Crée au moment de la création du GameObject de
    /// créer / reset la base de données temporaire.
    /// 
    /// Fait par EL MONTASER Osmane le 01/04/2022.
    /// </summary>
    void Start() {
        string tempPath = "Data Source=tempDB.db;Version=3";

        _dbHelper = new DBHelper(tempPath);
        _recordNumber = _dbHelper.AddRecord(0.0f, 0.0f);

        _agentList = getAllGOAgents();

        foreach(GameObject go in _agentList) {
            Agent agent = go.GetComponent<Agent>();
            string name = agent.Attributes["SpeciesName"].Split('(')[0];
            
            _dbHelper.AddAgent(agent.Attributes["Id"], name, .0f, -1.0f, _recordNumber, _dbHelper.SelectSpeciesId(name), Convert.ToInt32(agent.Attributes["Gender"]));
        }
        _time = .0f;
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
            saveAgentData(_time);
            _saveFrequencyAccumulator = .0f;
            _time += SaveFrequency;
        }

        _saveFrequencyAccumulator += Time.deltaTime;
    }

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
        foreach (GameObject go in _agentList) {
            Agent agent = go.GetComponent<Agent>();
            string name = agent.Attributes["SpeciesName"].Split('(')[0];
            
            _dbHelper.AddAgentData(time, Convert.ToDouble(agent.Attributes["WaterNeeds"]) / Convert.ToDouble(agent.Attributes["MaxWaterNeeds"]), Convert.ToDouble(agent.Attributes["EnergyNeeds"]) / Convert.ToDouble(agent.Attributes["MaxEnergyNeeds"]), "test", agent.Attributes["Id"], -1);
        }
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
    public void AddNewAgent(GameObject agent) {

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
    private List<GameObject> getAllGOAgents() {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        List<GameObject> objectList = new List<GameObject>();

        //Récupérer uniquement les objets qui possèdent le script des agents.
        foreach(GameObject go in allObjects)
            if(go.GetComponent<Agent>() != null)
                objectList.Add(go);
        
        return objectList;
    }
}