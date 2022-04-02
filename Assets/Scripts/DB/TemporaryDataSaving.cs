using UnityEngine;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;

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
    /// Crée au moment de la création du GameObject de
    /// créer / reset la base de données temporaire.
    /// 
    /// Fait par EL MONTASER Osmane le 01/04/2022.
    /// </summary>
    void Start() {
        string tempPath = "Data Source=tempDB.db;Version=3";
        
        if (!System.IO.File.Exists(tempPath))
            SqliteConnection.CreateFile(tempPath);

        DBInit init = new DBInit(tempPath, "./Assets/Scripts/DB/tables_creation.sql");

        DBHelper dbHelper = new DBHelper(tempPath);
        dbHelper.AddRecord(0.0f, 0.0f);

        dbHelper.Close();

        _agentList = getAllGOAgents();
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
        foreach (GameObject go in _agentList) {
            Agent agent = go.GetComponent<Agent>();
            string name = agent.NomEspece.Split('(')[0];
            
            //Condition à enlever DEBUG ONLY.
            if(name == "Wolf2") {
                
            }
        }
    }

    public void SetSaveFrequency(float frequency) {
        SaveFrequency = frequency;
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