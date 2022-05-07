using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

/// <summary>
/// Classe qui permet de générer du brouillard
/// grâce à l'algorithme de Perlin Noise et une
/// référence vers le Volume Sky and Fog.
/// 
/// Fait par EL MONTASER Osmane le 21/03/2022.
/// </summary>
public class FogGenerator : MonoBehaviour {
    /// <summary>
    /// Une référence à l'objet de la scène contenant
    /// le ciel, le brouillard et les nuages 
    /// volumétriques.
    /// </summary>
    public Volume VolumeToEdit;

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
    /// La vitesse à laquelle la map de brouillard
    /// s'acutalise dans le monde.
    /// </summary>
    public float FogSpeed = 2.5f;

    /// <summary>
    /// La distance parcourue par le brouillard à
    /// chaque tick.
    /// </summary>
    public float FogOffsetX = 0.25f;

    /// <summary>
    /// La distance parcourue par le brouillard à
    /// chaque tick.
    /// </summary>
    public float FogOffsetY = 0.25f;

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
    /// des ressources de la map du brouillard
    /// qui sont utilisés à la fois dans _t1 et la
    /// fonction Update().
    /// </summary>
    private static object _applyNoiseLock = new object();

    /// <summary>
    /// Savoir si la map du brouillard a déjà
    /// été appliquée au monde.
    /// </summary>
    private bool _alreadyApplied;

    /// <summary>
    /// Savoir depuis combien de temps le monde a
    /// eu la mise à jour de la map du brouillard.
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
    /// du brouillard.
    /// </summary>
    private (int, int) _lastCameraPosition;

    void Start() {
        _alreadyApplied = true;
        _timeAccumulator = FogSpeed / ActionNames.TimeSpeed;

        _t1 = new Thread(calcNoise);
        _t1.Start();
    }

    void Update() {
        _timeAccumulator += Time.deltaTime / ActionNames.TimeSpeed;
        applyFogValues();
        
        if(!_alreadyApplied && _timeAccumulator >= FogSpeed && Monitor.TryEnter(_applyNoiseLock)) {
            XOrg += FogOffsetX;
            YOrg += FogOffsetY;

            _alreadyApplied = true;
            _timeAccumulator = 0f;
            Monitor.Exit(_applyNoiseLock);
        }
    }

    /// <summary>
    /// Fonction qui permet d'appliquer les nouvelles
    /// valeurs d'intensité du brouillard calculées
    /// par le bruit de Perlin.
    /// 
    /// Fait par EL MONTASER Osmane le 21/03/2022.
    /// </summary>
    private void applyFogValues() {
        int xPosGrid = (int)Math.Floor(transform.position.x / SquareSize);
        int yPosGrid = (int)Math.Floor(transform.position.z / SquareSize);

        if(!_alreadyApplied || (xPosGrid, yPosGrid) != _lastCameraPosition) {
            _lastCameraPosition = (xPosGrid, yPosGrid);

            VolumeProfile profile = VolumeToEdit.sharedProfile;

            if (!profile.TryGet<Fog>(out var fog))
                fog = profile.Add<Fog>(false);
            
            fog.tint.overrideState = true;
            float intensity = .0f;
            
            if(_noiseTabMutex.WaitOne()) {
                intensity = _noiseTab[xPosGrid, yPosGrid];
                _noiseTabMutex.ReleaseMutex();
            }
            
            if(intensity < 0.3f)
                fog.tint.value = new Color(191, 191, 191) * .0f;
            else
                fog.tint.value = new Color(191, 191, 191) * ((intensity - 0.3f) * 0.05f);
        }
    }

    /// <summary>
    /// Fonction qui permet de calculer la map
    /// du brouillard.
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