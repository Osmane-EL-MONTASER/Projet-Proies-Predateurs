using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Classe qui gère le panneau de configuration du monde.
/// 
/// Fait par AVERTY Pierre le 21/03/2022 et modifiée le 25/02/2022.
/// </summary>
public class WorldConfiguration : MonoBehaviour {

    /// <summary>
    /// Singleton qui représente le monde.
    /// </summary>
    private World world;

    /// <summary>
    /// Input de la température.
    /// </summary>
    public TMP_InputField inputTemp;

    /// <summary>
    /// Input de l'humidité.
    /// </summary>
    public TMP_InputField inputHumidity;

    /// <summary>
    /// Input de la vitesse du vent.
    /// </summary>
    public TMP_InputField inputWindSpeed;

    /// <summary>
    /// Input de la vitesse du temps.
    /// </summary>
    public TMP_InputField inputTimeSpeed;

    /// <summary>
    /// Méthode qui s'active à l'initialisation du script.
    /// 
    /// Fait par AVERTY Pierre le 21/03/2022 et modifiée le 25/02/2022.
    /// </summary>
    void Start() {
        world = World.Instance;
    }

    /// <summary>
    /// Méthode qui attribue la temperature au monde.
    /// 
    /// Fait par AVERTY Pierre le 21/03/2022 et modifiée le 25/03/2022.
    /// </summary>
    public void setTemperature() {
        world.Temperature =  float.Parse(inputTemp.text);
        inputTemp.text = "";
    }

    /// <summary>
    /// Méthode qui attribue l'humidité au monde.
    /// 
    /// Fait par AVERTY Pierre le 21/03/2022 et modifiée le 25/03/2022.
    /// </summary>
    public void setHumidity() {
        world.Humidity = int.Parse(inputHumidity.text);
        inputHumidity.text = "";
    }

    /// <summary>
    /// Méthode qui attribue la vitesse du vent au monde.
    /// 
    /// Fait par AVERTY Pierre le 21/03/2022 et modifiée le 25/03/2022.
    /// </summary>
    public void setWindSpeed() {
        world.WindSpeed = float.Parse(inputWindSpeed.text);
        inputWindSpeed.text = "";
    }

    /// <summary>
    /// Méthode qui attribue la vitesse du temps au monde.
    /// 
    /// Fait par AVERTY Pierre le 21/03/2022 et modifiée le 25/03/2022.
    /// </summary>
    public void setTimeSpeed() {
        world.TimeSpeed = float.Parse(inputTimeSpeed.text);
        inputTimeSpeed.text = "";
    }
}
