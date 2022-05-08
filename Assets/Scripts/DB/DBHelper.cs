using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System;

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
    /// <returns>
    /// L'id du record qui vient d'être ajouté.
    /// </returns>
    public int AddRecord(float startTime, float stopTime) {
        IDbCommand addRecordCommand = _dbConnection.CreateCommand();
        IDbCommand selectRecordId = _dbConnection.CreateCommand();
        int id = -1;

        addRecordCommand.CommandText = "INSERT INTO RECORD VALUES('" + Guid.NewGuid().ToString() + "', " + startTime + ", " + stopTime + ");";
        addRecordCommand.ExecuteReader();

        selectRecordId.CommandText = "SELECT last_insert_rowid();";
        IDataReader rdr = selectRecordId.ExecuteReader();
        rdr.Read();

        id = rdr.GetInt32(0);

        return id;
    }

    /// <summary>
    /// Pour pouvoir modifier le temps d'arrêt de la mesure.
    /// 
    /// Fait par EL MONTASER Osmane le 03/04/2022.
    /// </summary>
    /// <param name="recordNum">
    /// Le numéro de l'enregistrement à modifier.
    /// </param>
    /// <param name="stopTime">
    /// Le temps en secondes auquel les mesures de la 
    /// simulation s'arrête.
    /// </param>
    public void UpdateRecord(int recordNum, float stopTime) {
        IDbCommand updateRecordCommand = _dbConnection.CreateCommand();

        updateRecordCommand.CommandText = "UPDATE RECORD SET record_stop_time = " + stopTime + " WHERE ROWID = " + recordNum + ";";
        updateRecordCommand.ExecuteReader();
    }

    public void SetDeathToAgent(string agentNum, float deathTime, string deathCause) {
        IDbCommand updateRecordCommand = _dbConnection.CreateCommand();

        updateRecordCommand.CommandText = "UPDATE AGENT SET agent_death_date = " 
                                        + deathTime + " WHERE agent_num = '" + agentNum + "';";
        updateRecordCommand.ExecuteReader();
        updateRecordCommand.Dispose();
        updateRecordCommand.CommandText = "UPDATE AGENT SET agent_death_cause = '" 
                                        + deathCause + "' WHERE agent_num = '" + agentNum + "';";
        updateRecordCommand.ExecuteReader();
        updateRecordCommand.Dispose();
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

        addSpeciesCommand.CommandText = "INSERT OR IGNORE INTO SPECIES VALUES(NULL, '" + speciesLabel + "', " + speciesParentNum + ");";
        addSpeciesCommand.ExecuteReader();
    }

    /// <summary>
    /// Permet de récupérer l'Id d'une espèce avec son
    /// nom.
    /// 
    /// Fait par EL MONTASER Osmane le 03/04/2022.
    /// </summary>
    /// <param name="speciesLabel">
    /// Le nom de l'espèce.
    /// </param>
    /// <returns>
    /// L'id de l'espèce dans la BDD.
    /// </returns>
    public int SelectSpeciesId(string speciesLabel) {
        IDbCommand selectSpeciesIdCommand = _dbConnection.CreateCommand();
        int id = -1;

        selectSpeciesIdCommand.CommandText = "SELECT species_num FROM SPECIES WHERE species_label = '" + speciesLabel + "';";
        IDataReader rdr = selectSpeciesIdCommand.ExecuteReader();
        rdr.Read();

        id = rdr.GetInt32(0);

        return id;
    }

    /// <summary>
    /// Permet de récupérer l'Id d'une espèce avec son
    /// nom.
    /// 
    /// Fait par EL MONTASER Osmane le 03/04/2022.
    /// </summary>
    /// <param name="speciesLabel">
    /// Le nom de l'espèce.
    /// </param>
    /// <returns>
    /// L'id de l'espèce dans la BDD.
    /// </returns>
    public Dictionary<string, double> SelectSpeciesData(string speciesLabel) {
        IDbCommand selectSpeciesIdCommand = _dbConnection.CreateCommand();
        Dictionary<string, double> speciesData = new();

        selectSpeciesIdCommand.CommandText = "SELECT species_carcass_energy_contribution, species_max_water_needs," +
                                             "species_max_energy_needs, species_max_speed, species_gestation_period," +
                                             "species_maturity_age, species_max_age, species_digestion_time, " + 
                                             "species_prey_consumption_time, species_range, species_max_health," +
                                             "species_damage, species_litter_max, species_stamina_max FROM SPECIES WHERE species_label = '" + speciesLabel + "';";
        IDataReader rdr = selectSpeciesIdCommand.ExecuteReader();
        while(rdr.Read())
            for(int i = 0; i < 14; i++){
                speciesData.Add(getSelectedSpeciesDataString(i),rdr.GetDouble(i));
            }
                

        return speciesData;
    }


    /// <summary>
    /// Permet de récupérer les infos des especes grâve à son nom
    /// 
    /// Fait par AVERTY Pierre le 08/05/2022.
    /// </summary>
    /// <returns>
    /// Les infos.
    /// </returns>
    public Dictionary<string,string> SelectSpeciesInfo() {
        IDbCommand selectSpeciesIdCommand = _dbConnection.CreateCommand();
        Dictionary<string,string> speciesData = new();

        selectSpeciesIdCommand.CommandText = "SELECT species_label, species_type FROM SPECIES;";
        IDataReader rdr = selectSpeciesIdCommand.ExecuteReader();
        while(rdr.Read())
            speciesData.Add(rdr[0].ToString(), rdr[1].ToString());

        return speciesData;
    }

    public void UpdateSpeciesData(int speciesNum, double CarcassEnergyContribution, double MaxWaterNeeds, double MaxEnergyNeeds, double MaxSpeed, double GestationPeriod, double MaturityAge, double MaxAge, double DigestionTime, double PreyConsumptionTime,
    double MaxHealth, double MaxStamina, double AttackDamage, int LitterMax) {
         IDbCommand updateSpeciesCommand = _dbConnection.CreateCommand();
        
        updateSpeciesCommand.CommandText = "UPDATE SPECIES SET species_carcass_energy_contribution = " 
                                        + CarcassEnergyContribution.ToString().Replace(",",".") + ", species_max_water_needs = " 
                                        + MaxWaterNeeds.ToString().Replace(",",".") + ", species_max_energy_needs = " 
                                        + MaxEnergyNeeds.ToString().Replace(",",".") + ", species_max_speed = " 
                                        + MaxSpeed.ToString().Replace(",",".") + ", species_gestation_period = " 
                                        + GestationPeriod.ToString().Replace(",",".") + ", species_maturity_age = " 
                                        + MaturityAge.ToString().Replace(",",".") + ", species_max_age = " 
                                        + MaxAge.ToString().Replace(",",".") + ", species_digestion_time = " 
                                        + DigestionTime.ToString().Replace(",",".") + ", species_prey_consumption_time = " 
                                        + PreyConsumptionTime.ToString().Replace(",",".") + ", species_max_health = " 
                                        + MaxHealth.ToString().Replace(",",".") + ", species_damage = "
                                        + AttackDamage.ToString().Replace(",",".") + ", species_stamina_max = "
                                        + MaxStamina.ToString().Replace(",",".") + ", species_litter_max = " 
                                        + LitterMax
                                        + " WHERE species_num = '" + speciesNum + "';";

        updateSpeciesCommand.ExecuteReader();
        updateSpeciesCommand.Dispose();
     }

    private string getSelectedSpeciesDataString(int index) {
        switch (index) {
            case 0:
                return "CarcassEnergyContribution";
                break;
            case 1:
                return "MaxWaterNeeds";
                break;
            case 2:
                return "MaxEnergyNeeds";
                break;
            case 3:
                return "MaxSpeed";
                break;
            case 4:
                return "GestationPeriod";
                break;
            case 5:
                return "MaturityAge";
                break;
            case 6:
                return "MaxAge";
                break;
            case 7:
                return "DigestionTime";
                break;
            case 8:
                return "PreyConsumptionTime";
                break;
            case 9:
                return "Range";
                break;
            case 10:
                return "MaxHealth";
                break;
            case 11:
                return "Ad";
                break;
            case 12:
                return "MaxStamina";
                break;
            case 13:
                return "LitterMax";
                break;
            default:
                return "Unkown";
        }
    }

    /// <summary>
    /// Permet de récupérer la liste des proies d'une espèce
    /// passée en paramètre.
    /// 
    /// Fait par EL MONTASER Osmane le 15/04/2022.
    /// </summary>
    /// <param name="speciesLabel">
    /// Le nom de l'espèce à ajouter.
    /// </param>
    /// <returns>
    /// La liste des proies de l'espèce.
    /// </returns>
    public List<string> SelectPreysOf(string speciesLabel) {
        IDbCommand selectSpeciesIdCommand = _dbConnection.CreateCommand();
        List<string> preys = new();

        selectSpeciesIdCommand.CommandText = "SELECT species_label FROM SPECIES S JOIN (SELECT * FROM PREY_LIST WHERE predator_species_num = (SELECT species_num FROM SPECIES WHERE species_label = '" + speciesLabel + "')) ON prey_species_num = species_num;";
        IDataReader rdr = selectSpeciesIdCommand.ExecuteReader();
        while(rdr.Read())
            preys.Add(rdr.GetString(0));

        return preys;
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
    /// <param name="agentId">L'Id de l'agent unique à ajouter
    /// contenu dans l'objet Agent.</param>
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
    public void AddAgent(string agentId, string agentLabel, float agentBirthDate, float agentDeathDate,
                         int recordNum, int speciesNum, int genderNum) {
        IDbCommand addAgentCommand = _dbConnection.CreateCommand();

        addAgentCommand.CommandText = "INSERT OR IGNORE INTO AGENT VALUES('" + agentId + "', '" 
                                        + agentLabel + "', " + agentBirthDate + ", " + (agentDeathDate == -1.0f ? "NULL" : agentDeathDate) + ", NULL, (SELECT record_num FROM RECORD WHERE ROWID = " + recordNum + "), " + speciesNum + ", " + genderNum + ");";
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
    public void AddAgentData(float dataTime, double dataHydrationLevel, double dataHungerLevel, 
                             string dataWorldNum, string dataAgentNum, int dataPackNum) {
        IDbCommand addAgentDataCommand = _dbConnection.CreateCommand();

        addAgentDataCommand.CommandText = "INSERT INTO AGENT_DATA VALUES(" 
                                        + dataTime + ", " + dataHydrationLevel + ", " + dataHungerLevel + ", '"
                                        + dataWorldNum + "', '" + dataAgentNum + "', " + (dataPackNum == -1 ? "NULL" : dataPackNum) + ");";

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