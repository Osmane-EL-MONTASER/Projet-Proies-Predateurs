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
    private double _health;
    private double _staminaMax;
    private double _timeToLive;
    private double _maxSpeed;
    private int _numAgents;
    private bool _isFocused;


    void Start(){
        _isFocused = false;

        inputHealth.text = "0";
        inputMaxSpeed.text = "0";
        inputMaxStamina.text = "0";
        inputNumAgents.text = "0";
        inputTimeToLive.text = "0";
    }
    public void onClick(){
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
