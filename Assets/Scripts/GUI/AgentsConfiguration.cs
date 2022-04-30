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
    public TMP_InputField inputHealth;

    /// <summary>
    /// Input de l'endurance max.
    /// </summary>
    public TMP_InputField inputMaxStamina;

    /// <summary>
    /// Input de la durée de vie.
    /// </summary>
    public TMP_InputField inputTimeToLive;

    /// <summary>
    /// Input de la vitesse max.
    /// </summary>
    public TMP_InputField inputMaxSpeed;

    /// <summary>
    /// Input de la vitesse du nombre d'agents.
    /// </summary>
    public TMP_InputField inputNumAgents;

    /// <summary>
    /// Type d'agent séléctionné.
    /// </summary>
    private string _selectedAgentType;

    /// <summary>
    /// Santé des agents.
    /// </summary>
    private double _health;
    
    /// <summary>
    /// Stamina des agents.
    /// </summary>
    private double _staminaMax;

    
    /// <summary>
    /// Durée de vie max des agents.
    /// </summary>
    private double _timeToLive;

    
    /// <summary>
    /// Vitesse max des agents.
    /// </summary>
    private double _maxSpeed;

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


    void Start(){
        _health = 0.0;
        _maxSpeed = 0.0;
        _maxSpeed = 0.0;
        _numAgents = 0;
        _timeToLive = 0.0;

        button.onClick.AddListener((arg) => AgentManager.Instance.initializationAgents(_health, _maxSpeed, _staminaMax, _timeToLive, _numAgents));
    }

    /// <summary>
    /// Fonction qui permet l'affichage des paramètres que l'on va modifié 
    /// et qui leur ajoute un listener pour pouvoir les modifier. 
    ///
    /// Fait par Pierre AVERTY le 17/04/2022 et modifiée le 29/04/2022.
    /// </summary>
    public void onClick(){
        settings.SetActive(true);
        TMP_Text title = settings.transform.Find("Texte \"Paramètres\"").gameObject.GetComponent<TMP_Text>();

        inputHealth.onEndEdit.RemoveAllListeners();
        inputMaxSpeed.onEndEdit.RemoveAllListeners();
        inputMaxStamina.onEndEdit.RemoveAllListeners();
        inputNumAgents.onEndEdit.RemoveAllListeners();
        inputTimeToLive.onEndEdit.RemoveAllListeners();

        inputHealth.text = _health.ToString();
        inputMaxSpeed.text = _maxSpeed.ToString();
        inputMaxStamina.text = _staminaMax.ToString();
        inputNumAgents.text = _numAgents.ToString();
        inputTimeToLive.text = _timeToLive.ToString();

        inputHealth.onEndEdit.AddListener((arg) => setHealth());
        inputMaxStamina.onEndEdit.AddListener((arg) => setMaxStamina());
        inputMaxSpeed.onEndEdit.AddListener((arg) => setMaxSpeed());
        inputNumAgents.onEndEdit.AddListener((arg) => setNumAgents());
        inputTimeToLive.onEndEdit.AddListener((arg) => setTimeToLive());
        
        _selectedAgentType = gameObject.name;
        title.text = "Paramètres " + _selectedAgentType + " :"; 
        
        Debug.Log(_selectedAgentType);
    }

    /// <summary>
    /// Méthode qui attribue la santé d'un agent.
    /// 
    /// Fait par AVERTY Pierre le 17/04/2022.
    /// </summary>
    public void setHealth() {
        _health =  double.Parse(inputHealth.text);
        inputHealth.onEndEdit.RemoveListener((arg) => setHealth());
    }

    /// <summary>
    /// Méthode qui attribue l'energie d'un agent.
    /// 
    /// Fait par AVERTY Pierre le 17/04/2022.
    /// </summary>
    public void setMaxStamina() { 
        _staminaMax =  double.Parse(inputMaxStamina.text);
        inputMaxStamina.onEndEdit.RemoveListener((arg) => setMaxStamina());
    }

    /// <summary>
    /// Méthode qui attribue le temps de vie d'un agent.
    /// 
    /// Fait par AVERTY Pierre le 17/04/2022.
    /// </summary>
    public void setTimeToLive() {
        _timeToLive =  double.Parse(inputTimeToLive.text);
        inputTimeToLive.onEndEdit.RemoveListener((arg) => setTimeToLive());
    }

    /// <summary>
    /// Méthode qui attribue la vitesse max d'un agent.
    /// 
    /// Fait par AVERTY Pierre le 17/04/2022.
    /// </summary>
    public void setMaxSpeed() {
         _maxSpeed =  double.Parse(inputMaxSpeed.text);
        inputMaxSpeed.onEndEdit.RemoveListener((arg) => setMaxSpeed());
    }

    /// <summary>
    /// Méthode qui attribue le nombre d'agents.
    /// 
    /// Fait par AVERTY Pierre le 17/04/2022.
    /// </summary>
    public void setNumAgents() {
        _numAgents =  int.Parse(inputNumAgents.text);
        inputNumAgents.onEndEdit.RemoveListener((arg) => setNumAgents());
    }

}
