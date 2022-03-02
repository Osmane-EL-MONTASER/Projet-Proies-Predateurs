using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

/// <summary>
/// Classe qui contient toutes les fonctions que
/// l'on peut réaliser sur la base de données
/// du projet Proies Prédateurs.
/// 
/// Fait par EL MONTASER Osmane le 02/03/2022.
/// </summary>
public class DBHelper {
    /// <summary>
    /// Le chemin vers le fichier de base de 
    /// données SQLite.
    /// "URI=file:./Assets/Scripts/DB/database.db"
    /// </summary>
    private string dbFilePath { get; set; }

    /// <summary>
    /// La connexion à la base de données SQLite.
    /// </summary>
    private IDbConnection dbConnection;

    /// <summary>
    /// Fonction lancée à la création de la classe, elle 
    /// s'occupe d'instancier la connexion avec la base
    /// de données.
    /// 
    /// Fait par EL MONTASER Osmane le 02/03/2022.
    /// </summary>
    /// 
    /// <param name="dbFilePath">Le chemin vers le fichier
    /// de base de données.</param>
    public DBHelper(string dbFilePath) {
        this.dbFilePath = dbFilePath;

		dbConnection = new SqliteConnection(dbFilePath);
        dbConnection.Open();
    }
}