using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

/// <summary>
/// Classe qui permet d'instancier une connexion
/// à un fichier de base de données quelconque.
/// S'il manque tout ou parti des tables de la
/// base de données, le script les créer si et
/// seulement si il a été ajouté IF NOT EXISTS
/// dans le script de création des tables.
/// 
/// Fait par EL MONTASER Osmane le 02/03/2022.
/// </summary>
public class DBInit {
    /// <summary>
    /// Le chemin vers le fichier de base de 
    /// données SQLite.
    /// "URI=file:./Assets/Scripts/DB/database.db"
    /// </summary>
    private string _dbFilePath { get; set; }

    /// <summary>
    /// Le chemin vers le fichier de création des
    /// tables de la base de données.
    /// "./Assets/Scripts/DB/tables_creation.sql"
    /// </summary>
    private string _dbCreationScriptPath { get; set; }

    /// <summary>
    /// La connexion à la base de données SQLite.
    /// </summary>
    private IDbConnection _dbConnection;

    /// <summary>
    /// Fonction lancée à la création de la classe, elle 
    /// s'occupe de vérifier si la base de données existe 
    /// pas. Au quel cas le script recréera la base de 
    /// données.
    /// 
    /// Fait par EL MONTASER Osmane le 02/03/2022.
    /// </summary>
    /// 
    /// <param name="dbFilePath">Le chemin vers le fichier
    /// de base de données.</param>
    /// <param name="dbCreationScriptPath">Le chemin vers le
    /// fichier de création des tables.</param>
    public DBInit(string dbFilePath, string dbCreationScriptPath) {
        _dbFilePath = dbFilePath;
        _dbCreationScriptPath = dbCreationScriptPath;

		_dbConnection = new SqliteConnection(_dbFilePath);
        _dbConnection.Open();

        CreateDB();

        _dbConnection.Close();
    }

    /// <summary>
    /// Fonction qui permet de créer les tables si elles
    /// existent. Il est nécessaire d'avoir indiqué le
    /// chemin vers ce script dans les propriétés du
    /// script.
    /// 
    /// Fait par EL MONTASER Osmane le 02/03/2022.
    /// </summary>
    private void CreateDB() {
        IDbCommand dbCreationCommand = _dbConnection.CreateCommand();
        
        dbCreationCommand.CommandText = System.IO.File.ReadAllText(_dbCreationScriptPath);
        dbCreationCommand.ExecuteReader();
    }
}