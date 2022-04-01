using UnityEngine;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;

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
    /// Là où la base de données temporaire a été
    /// créee.
    /// </summary>
    string _dbFilePath;

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
    }

    /// <summary>
    /// Met à jour la base de données à chaque tick
    /// en fonction des agents et des données de
    /// l'environnement de la simulation.
    /// 
    /// Fait par EL MONTASER Osmane le 01/04/2022.
    /// </summary>
    void Update() {

    }
}

//GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>() ;