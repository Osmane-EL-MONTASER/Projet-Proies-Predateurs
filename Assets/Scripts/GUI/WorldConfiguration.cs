using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Classe qui gère le panneau de configuration du monde.
/// 
/// Fait par AVERTY Pierre le 21/03/2022.
/// </summary>
public class WorldConfiguration : MonoBehaviour {

    /// <summary>
    /// Paneau de configuration du monde.
    /// </summary>
    public GameObject panel;

    /// <summary>
    /// Input de la temperature.
    /// </summary>
    public GameObject temperatureInput;

    /// <summary>
    /// Input du taux d'humidité.
    /// </summary>
    public GameObject humidityDegreeInput;

    /// <summary>
    /// Input de la vitesse du vent.
    /// </summary>
    public GameObject windSpeedInput;

    /// <summary>
    /// Input de la vitesse de la simulation.
    /// </summary>
    public GameObject timeSpeedInput;

    /// <summary>
    /// Singleton qui représente le monde.
    /// </summary>
    private World world;

    /// <summary>
    /// Méthode qui s'active à l'initialisation du script.
    /// 
    /// Fait par AVERTY Pierre le 21/03/2022.
    /// </summary>
    void Start() {
        world = World.Instance;

        temperatureInput.GetComponent<TMP_InputField>().onEndEdit.AddListener(setTemperature);
        humidityDegreeInput.GetComponent<TMP_InputField>().onEndEdit.AddListener(setHumidity);
        windSpeedInput.GetComponent<TMP_InputField>().onEndEdit.AddListener(setWindSpeed);
        timeSpeedInput.GetComponent<TMP_InputField>().onEndEdit.AddListener(setTimeSpeed);
    }

    /// <summary>
    /// Méthode qui attribue la temperature au monde.
    /// 
    /// Fait par AVERTY Pierre le 21/03/2022.
    /// </summary>
    private void setTemperature(string tempString) {
        world.Temperature = tempString;
    }

    /// <summary>
    /// Méthode qui attribue l'humidité au monde.
    /// 
    /// Fait par AVERTY Pierre le 21/03/2022.
    /// </summary>
    private void setHumidity(string humidityString) {
        world.Humidity = humidityString;
    }

    /// <summary>
    /// Méthode qui attribue la vitesse du vent au monde.
    /// 
    /// Fait par AVERTY Pierre le 21/03/2022.
    /// </summary>
    private void setWindSpeed(string speedString) {
        world.WindSpeed = speedString;
    }

    /// <summary>
    /// Méthode qui attribue la vitesse du temps au monde.
    /// 
    /// Fait par AVERTY Pierre le 21/03/2022.
    /// </summary>
    private void setTimeSpeed(string speedString) {
        world.TimeSpeed = speedString;
    }
}
