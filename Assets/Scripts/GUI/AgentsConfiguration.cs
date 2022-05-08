using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Text.RegularExpressions;

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
    /// Input des dégats d'attaque.
    /// </summary>
    public TMP_InputField attackDamage;

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
    /// Dégats d'attaque.
    /// </summary>
    private double _attackDamage;

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

    private static bool _isBDDReset = false;

    public Sprite[] spriteList;




    void Start(){
        string tempPath = "Data Source=tempDB.db;Version=3";
        if(!_isBDDReset) {
            File.Delete("tempDB.db");
            DBInit init = new DBInit("Data Source=tempDB.db;Version=3", "./Assets/Scripts/DB/tables_creation.sql");
            _isBDDReset = true;
        }

        DBHelper _dbHelper = new(tempPath);

        string titleText = gameObject.name;
        if(title != null)
            title.text = "Paramètres " + Regex.Replace(titleText, "[0-9]", "") + " :";

        if(gameObject.name.Contains("Panel")){
            Dictionary<string,string> datas = _dbHelper.SelectSpeciesInfo();
            foreach(KeyValuePair<string,string> entry in datas){
               switch(entry.Value){
                    case "predator":
                        if(gameObject.name.Contains("2"))
                            processTemplates(gameObject.transform.Find("Prédateurs/Container"), "Prédateurs/Container/Template", entry.Key);
                        break;
                    case "prey":
                        if(gameObject.name.Contains("2"))
                            processTemplates(gameObject.transform.Find("Proies/Container"), "Proies/Container/Template", entry.Key);
                        break;
                    case "autotroph":
                        if(!gameObject.name.Contains("2"))
                            processTemplates(gameObject.transform.Find("Autotrophes/Container"), "Autotrophes/Container/Template", entry.Key);
                        break;
               }
            if(gameObject.name.Contains("2")){
                Destroy(gameObject.transform.Find("Prédateurs/Container/Template").gameObject);
                Destroy(gameObject.transform.Find("Proies/Container/Template").gameObject);
            } else {
                Destroy(gameObject.transform.Find("Autotrophes/Container/Template").gameObject);
            }

            }
        }
    }

    private void processTemplates(Transform parent, string path, string name){
        GameObject duplicate = gameObject.transform.Find(path).gameObject;
        Debug.Log(parent);
        duplicate = Instantiate(duplicate, parent);
        duplicate.name = name;
        Image image = duplicate.GetComponent<Image>();
        GameObject child = duplicate.transform.Find("Text (TMP)").gameObject;
        foreach(Sprite sprite in spriteList){
            if(sprite.name == name)
                image.sprite = sprite;
        }
        child.GetComponent<TMP_Text>().text = Regex.Replace(name, "[0-9]", "");
        child.GetComponent<TMP_Text>().enabled = true;
        foreach(Behaviour component in duplicate.GetComponents(typeof(Behaviour))){
            component.enabled = true;
        }
    }
    /// <summary>
    /// Fonction qui permet l'affichage des paramètres que l'on va modifié 
    /// et qui leur ajoute un listener pour pouvoir les modifier. 
    ///
    /// Fait par Pierre AVERTY le 17/04/2022 et modifiée le 29/04/2022.
    /// </summary>
    public void onClick(){
        settings.SetActive(true);
        _selectedAgentType = gameObject.name;
        title.text = "Paramètres " + Regex.Replace(_selectedAgentType, "[0-9]", "") + " :";
        AgentManager.Instance.newAgentType = _selectedAgentType;

        string tempPath = "Data Source=tempDB.db;Version=3";
        DBHelper _dbHelper = new (tempPath);
        Dictionary<string,double> data = _dbHelper.SelectSpeciesData(_selectedAgentType);

        _carcassEnergyContribution = data["CarcassEnergyContribution"];
        _maxWaterNeeds = data["MaxWaterNeeds"];
        _maxEnergyNeeds = data["MaxEnergyNeeds"];
        _maxSpeed = data["MaxSpeed"];
        _gestationPeriod = data["GestationPeriod"];
        _maturityAge = data["MaturityAge"];
        _maxAge = data["MaxAge"];
        _digestionTime = data["DigestionTime"];
        _preyConsumptionTime = data["PreyConsumptionTime"];
        _maxHealth = data["MaxHealth"];
        _maxStamina = data["MaxStamina"];
        _attackDamage = data["Ad"];
        _litterMax = (int) data["LitterMax"];
        _numAgents = 0;

        if(button!=null)
            button.onClick.RemoveAllListeners();
            
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
        attackDamage.onEndEdit.RemoveAllListeners();

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
        attackDamage.text = _attackDamage.ToString();

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
        attackDamage.onEndEdit.AddListener((arg) => setAttackDamage());

        if(button != null)
            button.onClick.AddListener(() => AgentManager.Instance.initializationAgents(_selectedAgentType,  _carcassEnergyContribution, _maxWaterNeeds, _maxEnergyNeeds, _maxSpeed, _gestationPeriod, _maturityAge, _maxAge, _digestionTime, _preyConsumptionTime,
    _maxHealth, _maxStamina, _attackDamage, _litterMax, _numAgents));
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
    /// Fait par AVERTY Pierre le 07/05/2022.
    /// </summary>
    public void setAttackDamage() {
        _attackDamage =  double.Parse(attackDamage.text);
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
