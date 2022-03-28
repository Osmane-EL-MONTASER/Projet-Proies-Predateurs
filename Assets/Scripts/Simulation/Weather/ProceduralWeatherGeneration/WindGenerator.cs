using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

/// <summary>
/// Classe qui permet de générer du vent
/// grâce à l'algorithme de Perlin Noise et une
/// référence vers le Volume Sky and Wind.
/// 
/// Fait par EL MONTASER Osmane le 21/03/2022.
/// </summary>
public class WindGenerator : MonoBehaviour {
    /// <summary>
    /// Une référence au script Weather Zoning
    /// pour pouvoir mettre à jour la zone
    /// concernée.
    /// </summary>
    public WeatherZoning WeatherZoningScript;

    /// <summary>
    /// Largeur de la carte sur laquelle générer
    /// les précipitations.
    /// Faites attention à sa taille au quel cas
    /// vous subirez du lag.
    /// </summary>
    public int MapWidth;

    /// <summary>
    /// Hauteur de la carte sur laquelle générer
    /// les précipitations.
    /// </summary>
    public int MapHeight;

    /// <summary>
    /// Le décalage horizontal du bruit de perlin. 
    /// A modifier si vous voulez que les 
    /// précipitations bougent et ne soient 
    /// pas fixes.
    /// </summary>
    public float XOrg;

    /// <summary>
    /// Le décalage vertical du bruit de perlin. 
    /// A modifier si vous voulez que les 
    /// précipitations bougent et ne soient 
    /// pas fixes.
    /// </summary>
    public float YOrg;

    /// <summary>
    /// Le nombre de fois que le motif de bruit de
    /// perlin est répété.
    /// </summary>
    public float Scale = 1.0f;

    /// <summary>
    /// La vitesse à laquelle la map de vent
    /// s'acutalise dans le monde.
    /// </summary>
    public float WindSpeed = 0.25f;

    /// <summary>
    /// La distance parcourue par le vent à
    /// chaque tick.
    /// </summary>
    public float WindOffsetX = 0.25f;

    /// <summary>
    /// La distance parcourue par le vent à
    /// chaque tick.
    /// </summary>
    public float WindOffsetY = 0.25f;

    /// <summary>
    /// La taille des zones dans le monde.
    /// </summary>
    public int SquareSize = 25;

    /// <summary>
    /// Bruit de perlin généré par l'algorithme.
    /// </summary>
    private float[,] _noiseTab;

    /// <summary>
    /// Cet objet nous sert à lancer le calcul de
    /// la map dans un autre thread afin de ne pas
    /// bloquer le thread principal et avoir une
    /// simulation fluide.
    /// </summary>
    private Thread _t1;

    /// <summary>
    /// Cet objet permet de bloquer l'utilisation
    /// des ressources de la map du vent
    /// qui sont utilisés à la fois dans _t1 et la
    /// fonction Update().
    /// </summary>
    private static object _applyNoiseLock = new object();

    /// <summary>
    /// Savoir si la map du vent a déjà
    /// été appliquée au monde.
    /// </summary>
    private bool _alreadyApplied;

    /// <summary>
    /// Savoir depuis combien de temps le monde a
    /// eu la mise à jour de la map du vent.
    /// </summary>
    private float _timeAccumulator;

    /// <summary>
    /// Permet d'accéder au tableau depuis le thread
    /// principal sans que le thread de calcul n'y
    /// accède en même temps.
    /// </summary>
    private Mutex _noiseTabMutex = new Mutex();

    /// <summary>
    /// Permet de savoir où était la caméra lors de
    /// la dernière mise à jour des valeurs d'intensité
    /// du vent.
    /// </summary>
    private (int, int) _lastCameraPosition;

    void Start() {
        _alreadyApplied = true;
        _timeAccumulator = WindSpeed;

        _t1 = new Thread(calcNoise);
        _t1.Start();
    }

    void Update() {
        _timeAccumulator += Time.deltaTime;
        
        if(!_alreadyApplied && _timeAccumulator >= WindSpeed && Monitor.TryEnter(_applyNoiseLock)) {
            XOrg += WindOffsetX;
            YOrg += WindOffsetY;

            applyWindValues();

            _alreadyApplied = true;
            _timeAccumulator = 0f;
            Monitor.Exit(_applyNoiseLock);
        }
    }

    /// <summary>
    /// Fonction qui permet d'appliquer les nouvelles
    /// valeurs pour chaque modificateur de chaque
    /// objet pour chaque zone de la simulation par
    /// rapport aux valeurs d'intensité du vent
    /// calculées par le bruit de Perlin.
    /// 
    /// Fait par EL MONTASER Osmane le 21/03/2022.
    /// </summary>
    private void applyWindValues() {
        if(_noiseTabMutex.WaitOne()) {
            for(int i = 0; i < MapHeight; i++) {
                for(int j = 0; j < MapWidth; j++) {
                    if(_noiseTab[j, i] >= 0.5f)
                        WeatherZoningScript.AddWeatherAtZone(j, i, new Wind(WeatherZoningScript.GameObjects[j, i], _noiseTab[j, i]));
                }
            }
        }
    }

    /// <summary>
    /// Fonction qui permet de calculer la map
    /// du vent.
    /// 
    /// Repris de la documentation Unity sur
    /// Mathf.PerlinNoise et repris par EL MONTASER
    /// Osmane le 19/03/2022.
    /// </summary>
    private void calcNoise() {
        while(true) {
            if(_alreadyApplied && Monitor.TryEnter(_applyNoiseLock)) {
                float[,] tempNoiseTab = new float[MapWidth, MapHeight];
                float y = 0.0f;

                while (y < MapHeight) {
                    float x = 0.0f;
                    while (x < MapWidth) {
                        float xCoord = XOrg + x / MapWidth * Scale;
                        float yCoord = YOrg + y / MapHeight * Scale;
                        float sample = Mathf.PerlinNoise(xCoord, yCoord);

                        tempNoiseTab[(int)x, + (int)y] = sample;
                        x++;
                    }
                    y++;
                }

                if(_noiseTabMutex.WaitOne()) {
                    _noiseTab = (float[,])tempNoiseTab.Clone();
                    _noiseTabMutex.ReleaseMutex();
                }

                _alreadyApplied = false;
                Monitor.Exit(_applyNoiseLock);
            }
        }
    }
}