using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Classe qui gère le panneau de configuration des agents.
/// 
/// Fait par AVERTY Pierre le 17/04/2022.
/// </summary>
public class AgentsConfiguration : MonoBehaviour
{
    /// <summary>
    /// Input de la santé.
    /// </summary>
    public TMP_Text title;

    /// <summary>
    /// Input de la santé.
    /// </summary>
    public TMP_InputField health;

    /// <summary>
    /// Input de la quantité d'energie fournie par une carcasse.
    /// </summary>
    public TMP_InputField carcassEnergyContribution;

    /// <summary>
    /// Input du besoin en eau max.
    /// </summary>
    public TMP_InputField maxWaterNeeds;

    /// <summary>
    /// Input du besoin en energie max.
    /// </summary>
    public TMP_InputField maxEnergyNeeds;

    /// <summary>
    /// Input de la vitesse max.
    /// </summary>
    public TMP_InputField maxSpeed;

    /// <summary>
    /// Input de la période de gestation max.
    /// </summary>
    public TMP_InputField gestationPeriod;

    /// <summary>
    /// Input de l'age de maturité.
    /// </summary>
    public TMP_InputField maturityAge;

    /// <summary>
    /// Input de l'age max.
    /// </summary>
    public TMP_InputField maxAge;

    /// <summary>
    /// Input du temps de digestion max.
    /// </summary>
    public TMP_InputField digestionTime;

    /// <summary>
    /// Input du temps de consomation d'une proie.
    /// </summary>
    public TMP_InputField preyConsumptionTime;

    /// <summary>
    /// Input de l'energie max.
    /// </summary>
    public TMP_InputField maxStamina;

    /// <summary>
    /// Input du nombre de porée max.
    /// </summary>
    public TMP_InputField litterMax;

    /// <summary>
    /// Input de la vitesse du nombre d'agents.
    /// </summary>
    public TMP_InputField inputNumAgents;

    /// <summary>
    /// Type d'agent séléctionné.
    /// </summary>
    private string _selectedAgentType;

    /// <summary>
    /// Quantité d'energie fournie par une carcasse.
    /// </summary>
    private double _carcassEnergyContribution;
    
    /// <summary>
    /// Besoin d'eau max.
    /// </summary>
    private double _maxWaterNeeds;

    /// <summary>
    /// Besoin d'energie max.
    /// </summary>
    private double _maxEnergyNeeds;

    /// <summary>
    /// Vitesse max des agents.
    /// </summary>
    private double _maxSpeed;

    /// <summary>
    /// Periode de gestation.
    /// </summary>
    private double _gestationPeriod;

    /// <summary>
    /// Age de maturité.
    /// </summary>
    private double _maturityAge;

    /// <summary>
    /// Age max.
    /// </summary>
    private double _maxAge;

    /// <summary>
    /// Temps de digestion.
    /// </summary>
    private double _digestionTime;

    /// <summary>
    /// Temps de consomation d'une proie.
    /// </summary>
    private double _preyConsumptionTime;

    /// <summary>
    /// Santé max d'un agent.
    /// </summary>
    private double _maxHealth;

    /// <summary>
    /// Energie max d'un agent.
    /// </summary>
    private double _maxStamina;

    /// <summary>
    /// Portée max d'un agent.
    /// </summary>
    private int _litterMax;

    /// <summary>
    /// Nombre d'agents.
    /// </summary>
    private int _numAgents;

    /// <summary>
    /// Panneau de configuration des agents.
    /// </summary>
    public GameObject settings;

    /// <summary>
    /// Bouton de validation.
    /// </summary>
    public Button button;

    // private static bool _isBDDReset = false;


    void Start(){
    }

    /// <summary>
    /// Fonction qui permet l'affichage des paramètres que l'on va modifié 
    /// et qui leur ajoute un listener pour pouvoir les modifier. 
    ///
    /// Fait par Pierre AVERTY le 17/04/2022 et modifiée le 29/04/2022.
    /// </summary>
    public void onClick(){
        settings.SetActive(true);
        title.text = "Paramètres " + _selectedAgentType + " :";

        // string tempPath = "Data Source=tempDB.db;Version=3";
        // if(!_isBDDReset) {
        //     File.Delete("tempDB.db");
        //     DBInit init = new DBInit("Data Source=tempDB.db;Version=3", "./Assets/Scripts/DB/tables_creation.sql");
        //     _isBDDReset = true;
        // }

        // DBHelper _dbHelper = new (tempPath);
        // Dictionary<string,double> data = _dbHelper.SelectSpeciesData(_selectedAgentType);

        // _carcassEnergyContribution = data[""];
        // _maxWaterNeeds = 0.0;
        // _maxEnergyNeeds = 0.0;
        // _maxSpeed = 0.0;
        // _gestationPeriod = 0.0;
        // _maturityAge = 0.0;
        // _maxAge = 0.0;
        // _digestionTime = 0.0;
        // _preyConsumptionTime = 0.0;
        // _maxHealth = 0.0;
        // _maxStamina = 0.0;
        // _litterMax = 0;
        // _numAgents = 0;
        
        health.onEndEdit.RemoveAllListeners();
        carcassEnergyContribution.onEndEdit.RemoveAllListeners();
        maxWaterNeeds.onEndEdit.RemoveAllListeners();
        maxEnergyNeeds.onEndEdit.RemoveAllListeners();
        maxSpeed.onEndEdit.RemoveAllListeners();
        gestationPeriod.onEndEdit.RemoveAllListeners();
        maturityAge.onEndEdit.RemoveAllListeners();
        maxAge.onEndEdit.RemoveAllListeners();
        digestionTime.onEndEdit.RemoveAllListeners();
        preyConsumptionTime.onEndEdit.RemoveAllListeners();
        maxStamina.onEndEdit.RemoveAllListeners();
        litterMax.onEndEdit.RemoveAllListeners();
        inputNumAgents.onEndEdit.RemoveAllListeners();

        health.text = _maxHealth.ToString();
        carcassEnergyContribution.text = _carcassEnergyContribution.ToString();
        maxWaterNeeds.text = _maxWaterNeeds.ToString();
        maxEnergyNeeds.text = _maxEnergyNeeds.ToString();
        maxSpeed.text = _maxSpeed.ToString();
        gestationPeriod.text = _gestationPeriod.ToString();
        maturityAge.text = _maturityAge.ToString();
        maxAge.text = _maxAge.ToString();
        digestionTime.text = _digestionTime.ToString();
        preyConsumptionTime.text = _preyConsumptionTime.ToString();
        maxStamina.text = _maxStamina.ToString();
        litterMax.text = _litterMax.ToString();
        inputNumAgents.text = _numAgents.ToString();

        health.onEndEdit.AddListener((arg) => setHealth());
        carcassEnergyContribution.onEndEdit.AddListener((arg) => setCarcassEnergy());
        maxWaterNeeds.onEndEdit.AddListener((arg) => setMaxWaterNeeds());
        maxEnergyNeeds.onEndEdit.AddListener((arg) => setMaxEnergyNeeds());
        maxSpeed.onEndEdit.AddListener((arg) => setMaxSpeed());
        gestationPeriod.onEndEdit.AddListener((arg) => setGestationPeriod());
        maturityAge.onEndEdit.AddListener((arg) => setMaturityAge());
        maxAge.onEndEdit.AddListener((arg) => setMaxAge());
        digestionTime.onEndEdit.AddListener((arg) => setDigestionTime());
        preyConsumptionTime.onEndEdit.AddListener((arg) => setPreyCosumption());
        maxStamina.onEndEdit.AddListener((arg) => setMaxStamina());
        litterMax.onEndEdit.AddListener((arg) => setLitterMax());
        inputNumAgents.onEndEdit.AddListener((arg) => setNumAgents());
        
        _selectedAgentType = gameObject.name;
        AgentManager.Instance.newAgentType = _selectedAgentType;
        
        button.onClick.AddListener(() => AgentManager.Instance.initializationAgents(_selectedAgentType,  _carcassEnergyContribution, _maxWaterNeeds, _maxEnergyNeeds, _maxSpeed, _gestationPeriod, _maturityAge, _maxAge, _digestionTime, _preyConsumptionTime,
    _maxHealth, _maxStamina, _litterMax, _numAgents));

    }
    /// <summary>
    /// Fait par AVERTY Pierre le 07/05/2022.
    /// </summary>
    public void setHealth() {
        _maxHealth =  double.Parse(health.text);
    }
    /// <summary>
    /// Fait par AVERTY Pierre le 07/05/2022.
    /// </summary>
    public void setCarcassEnergy() {
        _carcassEnergyContribution =  double.Parse(carcassEnergyContribution.text);
    }
    /// <summary>
    /// Fait par AVERTY Pierre le 07/05/2022.
    /// </summary>
    public void setMaxWaterNeeds() {
        _maxWaterNeeds =  double.Parse(maxWaterNeeds.text);
    }
    /// <summary>
    /// Fait par AVERTY Pierre le 07/05/2022.
    /// </summary>
    public void setMaxEnergyNeeds() {
        _maxEnergyNeeds =  double.Parse(maxEnergyNeeds.text);
    }

    /// <summary>
    /// Fait par AVERTY Pierre le 07/05/2022.
    /// </summary>
    public void setMaxSpeed() {
        _maxSpeed =  double.Parse(maxSpeed.text);
    }
    /// <summary>
    /// Fait par AVERTY Pierre le 07/05/2022.
    /// </summary>
    public void setGestationPeriod() {
        _gestationPeriod =  double.Parse(gestationPeriod.text);
    }
    /// <summary>
    /// Fait par AVERTY Pierre le 07/05/2022.
    /// </summary>
    public void setMaturityAge() {
        _maturityAge =  double.Parse(maturityAge.text);
    }
    /// <summary>
    /// Fait par AVERTY Pierre le 07/05/2022.
    /// </summary>
    public void setDigestionTime() {
        _digestionTime =  double.Parse(digestionTime.text);
    }
    /// <summary>
    /// Fait par AVERTY Pierre le 07/05/2022.
    /// </summary>
    public void setPreyCosumption() {
        _preyConsumptionTime =  double.Parse(preyConsumptionTime.text);
    }
    /// <summary>
    /// Fait par AVERTY Pierre le 07/05/2022.
    /// </summary>
    public void setMaxStamina() {
        _maxStamina =  double.Parse(maxStamina.text);
    }
    /// <summary>
    /// Fait par AVERTY Pierre le 07/05/2022.
    /// </summary>
    public void setLitterMax() {
        _litterMax =  int.Parse(litterMax.text);
    }
    /// <summary>
    /// Fait par AVERTY Pierre le 07/05/2022.
    /// </summary>
    public void setMaxAge() {
        _maxAge =  double.Parse(maxAge.text);
    }

    /// <summary>
    /// Méthode qui attribue le nombre d'agents.
    /// 
    /// Fait par AVERTY Pierre le 17/04/2022.
    /// </summary>
    public void setNumAgents() {
        _numAgents =  int.Parse(inputNumAgents.text);
    }

}
