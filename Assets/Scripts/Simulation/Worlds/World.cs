using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton qui modèlise le monde.
/// 
/// Fait par AVERTY Pierre le 19/03/2022 et modifiée le 20/03/2022.
/// </summary>
public class World  : MonoBehaviour {

    /// <summary>
    /// Instance du singleton.
    /// </summary>
    private static World instance = null;

    /// <summary>
    /// Nom du monde.
    /// </summary>
    private string _name;

    /// <summary>
    /// Zone de météo contenue dans le monde.
    /// </summary>
    public WeatherZoning weatherZone;

    /// <summary>
    /// Météo.
    /// </summary>
    public Weather weatherState;

    /// <summary>
    /// Température de la simulation.
    /// </summary>
    private float _temperature;

    /// <summary>
    /// Taux d'humidité de la simulation.
    /// </summary>
    private int _humidityDegree;

    /// <summary>
    /// Vitesse du vent dans le monde.
    /// </summary>
    private float _windSpeed;

    /// <summary>
    /// Vitesse de la simulation dans le monde.
    /// </summary>
    private float _simulationTimeSpeed; 

    /// <summary>
    /// Constructeur du singleton World.
    /// 
    /// Fait par AVERTY Pierre le 19/03/2022.
    /// </summary>
    private World() {
       
    }

    /// <summary>
    /// Méthode qui crée l'instance du singleton et si elle existe déjà, la retourne.
    /// 
    /// Fait par AVERTY Pierre le 20/03/2022.
    /// </summary>
        public static World Instance {
            get {
                if (instance==null) {
                    instance = new World();
                }
                return instance;
            }
        }

    /// <summary>
    /// Getter et setter du nom.
    /// 
    /// Fait par AVERTY Pierre le 20/03/2022.
    /// </summary>
        public string Name {
            get {
                return _name;
            }
            set {
                _name = value;
            }
        }

    /// <summary>
    /// Getter et setter de la temperature.
    /// 
    /// Fait par AVERTY Pierre le 20/03/2022.
    /// </summary>
        public object Temperature {
            get {
                return _temperature;
            }
            set {
                if(value.GetType() == typeof(float))
                    _temperature = (float) value;
                else 
                    _temperature = 0f;
            }
        }

    /// <summary>
    /// Getter et setter de l'humidité.
    /// 
    /// Fait par AVERTY Pierre le 20/03/2022.
    /// </summary>
        public object Humidity {
            get {
                return _humidityDegree;
            }
            set {
                if(value.GetType() == typeof(int))
                    _humidityDegree = (int) value;
                else 
                    _humidityDegree = 0;
            }
        }

    /// <summary>
    /// Getter et setter de la vitesse du vent.
    /// 
    /// Fait par AVERTY Pierre le 20/03/2022.
    /// </summary>
        public object WindSpeed {
            get {
                return _windSpeed;
            }
            set {
                if(value.GetType() == typeof(float))
                    _windSpeed = (float) value;
                else 
                    _windSpeed = 0f;
            }
        }

    /// <summary>
    /// Getter et setter de la vitesse du temps.
    /// 
    /// Fait par AVERTY Pierre le 20/03/2022.
    /// </summary>
        public object TimeSpeed {
            get {
                return _simulationTimeSpeed;
            }
            set {
                if(value.GetType() == typeof(float))
                    _simulationTimeSpeed = (float) value;
                else 
                    _simulationTimeSpeed = 0f;
            }
        }
}
