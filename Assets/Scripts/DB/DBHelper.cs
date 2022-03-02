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
    private string _dbFilePath { get; set; }

    /// <summary>
    /// La connexion à la base de données SQLite.
    /// </summary>
    private IDbConnection _dbConnection;

    /// <summary>
    /// Constructeur qui s'occupe d'instancier la connexion 
    /// avec la base de données.
    /// 
    /// Fait par EL MONTASER Osmane le 02/03/2022.
    /// </summary>
    /// 
    /// <param name="dbFilePath">Le chemin vers le fichier
    /// de base de données.</param>
    public DBHelper(string dbFilePath) {
        _dbFilePath = dbFilePath;

		_dbConnection = new SqliteConnection(_dbFilePath);
        _dbConnection.Open();
    }

    /// <summary>
    /// Fonction qui permet d'insérer un nouvel enregistrement
    /// d'une simulation dans la BDD entre 2 temps donnés.
    /// 
    /// Fait par EL MONTASER Osmane le 02/03/2022.
    /// </summary>
    /// 
    /// <param name="startTime">Le temps en secondes du début
    /// de l'enregistrement.</param>
    /// <param name="stopTime">Le temps en secondes de fin 
    /// de l'enregistrement.</param>
    public void AddRecord(int startTime, int stopTime) {
        IDbCommand addRecordCommand = _dbConnection.CreateCommand();

        addRecordCommand.CommandText = "INSERT INTO RECORD VALUES(NULL, " + startTime + ", " + stopTime + ");";
        addRecordCommand.ExecuteReader();
    }

    /// <summary>
    /// Fonction qui permet d'insérer un nouveau genre dans
    /// la base de données. Utile lors de l'initialisation de
    /// puisqu'elle ne l'ajoutera pas si il existe déjà.
    /// 
    /// Fait par EL MONTASER Osmane le 02/03/2022.
    /// </summary>
    /// 
    /// <param name="genderLabel">Le libellé du nouveau genre
    /// à ajouter.</param>
    public void AddGender(string genderLabel) {
        IDbCommand addGenderCommand = _dbConnection.CreateCommand();

        addGenderCommand.CommandText = "INSERT OR IGNORE INTO GENDER VALUES(NULL, " + genderLabel + ");";
        addGenderCommand.ExecuteReader();
    }

    /// <summary>
    /// Fonction qui permet d'insérer un nouveau monde avec
    /// ses paramètres principaux.
    /// 
    /// Fait par EL MONTASER Osmane le 02/03/2022.
    /// </summary>
    /// 
    /// <param name="worldTemperature">La température du
    /// monde.</param>
    /// <param name="worldHumidity">Le pourcentage d'humidité
    /// du monde.</param>
    /// <param name="worldWindSpeed">La vitesse du vent globale
    /// du monde.</param>
    public void AddWorld(float worldTemperature, float worldHumidity, int worldWindSpeed) {
        IDbCommand addWorldCommand = _dbConnection.CreateCommand();

        addWorldCommand.CommandText = "INSERT INTO WORLD VALUES(NULL, " 
                                    + worldTemperature + ", " + worldHumidity + ", " + worldWindSpeed + ");";
        addWorldCommand.ExecuteReader();
    }

    /// <summary>
    /// Fonction qui permet d'insérer de nouvelles espèces dans
    /// la base de données. Vu que speciesLabel est UNIQUE, 
    /// tenter d'insérer un doublon n'aura aucun effet dans 
    /// la table.
    ///
    /// Fait par EL MONTASER Osmane le 02/03/2022.
    /// </summary>
    /// 
    /// <param name="speciesLabel">Le libellé de la nouvelle
    /// espèce.</param>
    /// <param name="speciesLabel">L'id de l'espèce parente, si
    /// elle existe.</param>
    public void AddSpecies(string speciesLabel, int speciesParentNum) {
        IDbCommand addSpeciesCommand = _dbConnection.CreateCommand();

        addSpeciesCommand.CommandText = "INSERT OR IGNORE INTO SPECIES VALUES(NULL, " + speciesLabel + ", " + speciesParentNum + ");";
        addSpeciesCommand.ExecuteReader();
    }

    /// <summary>
    /// Fonction qui permet d'ajouter un agent dans la base
    /// de données lié à un enregistrement, une
    /// espèce, un genre.
    /// Pour ajouter ses statistiques à l'instant t, il faut 
    /// utiliser la fonction AddAgentData().
    /// 
    /// Fait par EL MONTASER Osmane le 02/03/2022.
    /// </summary>
    /// 
    /// <param name="agentLabel">Le libellé de l'agent à 
    /// ajouter.</param>
    /// <param name="agentBirthDate">Le temps en secondes
    /// lorsque l'agent est né.</param>
    /// <param name="agentDeathDate">Le temps en secondes
    /// lorsque l'agent est mort. (-1 s'il n'est pas mort)</param>
    /// <param name="recordNum">L'ID de l'enregistrement
    /// lié à cet agent.</param>
    /// <param name="speciesNum">L'ID de l'espèce de 
    /// l'agent.</param>
    /// <param name="genderNum">L'ID du genre de l'agent.</param>
    public void AddAgent(string agentLabel, int agentBirthDate, int agentDeathDate,
                         int recordNum, int speciesNum, int genderNum) {
        IDbCommand addAgentCommand = _dbConnection.CreateCommand();

        addAgentCommand.CommandText = "INSERT OR IGNORE INTO AGENT VALUES(NULL, " 
                                        + agentLabel + ", " + agentBirthDate + ", " + (agentDeathDate == -1 ? "NULL" : agentDeathDate) + ", "
                                        + recordNum + ", " + speciesNum + ", " + genderNum + ");";
        addAgentCommand.ExecuteReader();
    }

    /// <summary>
    /// Fonction qui permet d'ajouter les statistiques à temps
    /// t d'un agent. 
    /// 
    /// Fait par EL MONTASER Osmane le 02/03/2022.
    /// </summary>
    /// 
    /// <param name="dataTime">Le temps t en secondes.</param>
    /// <param name="dataHydrationLevel">Le pourcentage 
    /// d'hydratation de l'agent.</param>
    /// <param name="dataHungerLevel">Le pourcentage de faim de
    /// l'agent.</param>
    /// <param name="dataWorldNum">L'état du monde actuel.</param>
    /// <param name="dataAgentNum">L'ID de l'agent.</param>
    /// <param name="dataPackNum">L'ID de la meute si il en
    /// a une(-1 s'il n'en a pas).</param>
    public void AddAgentData(int dataTime, float dataHydrationLevel, float dataHungerLevel, 
                             int dataWorldNum, int dataAgentNum, int dataPackNum) {
        IDbCommand addAgentDataCommand = _dbConnection.CreateCommand();

        addAgentDataCommand.CommandText = "INSERT OR IGNORE INTO AGENT_DATA VALUES(" 
                                        + dataTime + ", " + dataHydrationLevel + ", " + dataHungerLevel + ", "
                                        + dataWorldNum + ", " + dataAgentNum + ", " + (dataPackNum == -1 ? "NULL" : dataPackNum) + ");";
        addAgentDataCommand.ExecuteReader();
    }

    /// <summary>
    /// Fonction qui permet de fermer la connexion à la
    /// base de données après avoir exécuté toutes les
    /// requêtes nécessaires.
    /// 
    /// Fait par EL MONTASER Osmane le 02/03/2022.
    /// </summary>
    public void Close() {
        _dbConnection.Close();
    }
}