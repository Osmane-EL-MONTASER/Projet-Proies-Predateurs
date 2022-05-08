using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

/// <summary>
/// Classe permettant de créer la carte de pré-
/// cipitation grâce à l'algorithme de bruit
/// de perlin. Elle fonctionne en mode multi-
/// threadée pour le calcul de la map avec le
/// bruit de perlin.
/// Attention toutefois à ne pas faire de cartes 
/// trop grandes afin de ne pas faire lagger la
/// simulation.
/// 
/// Fait par EL MONTASER Osmane le 19/03/2022.
/// </summary>
public class Precipitation : MonoBehaviour {
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
    /// La vitesse à laquelle la map de 
    /// précipitations s'acutalise dans le monde.
    /// </summary>
    public float PrecipitationSpeed = 1f;

    public float PrecipitationOffsetX = 0.25f;

    public float PrecipitationOffsetY = 0.25f;

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
    /// des ressources de la map de précipitations
    /// qui sont utilisés à la fois dans _t1 et la
    /// fonction Update().
    /// </summary>
    private static object _applyNoiseLock = new object();

    /// <summary>
    /// Permet d'accéder au tableau depuis le thread
    /// principal sans que le thread de calcul n'y
    /// accède en même temps.
    /// </summary>
    private Mutex _noiseTabMutex = new Mutex();

    /// <summary>
    /// Savoir si la map de précipitations a déjà
    /// été appliquée au monde.
    /// </summary>
    private bool _alreadyApplied;

    /// <summary>
    /// Savoir depuis combien de temps le monde a
    /// eu la mise à jour de la map de précipitations.
    /// </summary>
    private float _timeAccumulator;

    /// <summary>
    /// Permet de créer l'objet qui stockera la map
    /// de précipitations ainsi que de lancer le
    /// thread qui calcul la map de précipitations
    /// en fonction du décalage actuel.
    /// 
    /// Fait par EL MONTASER Osmane le 19/03/2022.
    /// </summary>
    void Start() {
        _alreadyApplied = true;
        _timeAccumulator = PrecipitationSpeed * (ActionNames.TimeSpeed / ActionNames.DAY_DURATION);

        _t1 = new Thread(calcNoise);
        _t1.Start();
    }

    /// <summary>
    /// Fonction qui permet de mettre à jour la map
    /// des précipitations.
    /// 
    /// Fait par EL MONTASER Osmane le 19/03/2022.
    /// </summary>
    void Update() {
        _timeAccumulator += Time.deltaTime * (ActionNames.TimeSpeed / ActionNames.DAY_DURATION);
        if(!_alreadyApplied && _timeAccumulator >= PrecipitationSpeed && Monitor.TryEnter(_applyNoiseLock)) {
            XOrg += PrecipitationOffsetX;
            YOrg += PrecipitationOffsetY;

            _alreadyApplied = true;
            _timeAccumulator = 0f;
            Monitor.Exit(_applyNoiseLock);
        }
    }

    /// <summary>
    /// Permet de savoir s'il y a des précipitations
    /// à l'endroit donné en paramètre.
    /// 
    /// Fait par EL MONTASER Osmane le 19/03/2022.
    /// </summary>
    /// <param name="x">
    /// Coordonnée x de la zone.
    /// </param>
    /// <param name="y">
    /// Coordonnée y de la zone.
    /// </param>
    /// <returns>
    /// Retourne 0 s'il n'y a pas de précipitations.
    /// Sinon un réel entre 0 et 1 correspondant à
    /// l'intensité de la précipitation.
    /// </returns>
    public float GetPrecipitationAt(int x, int y) {
        float value = .0f;

        try {
            if(_noiseTabMutex.WaitOne()) {
                value = _noiseTab[x, y];
                _noiseTabMutex.ReleaseMutex();
            }
        } catch(Exception e) {
            return 0.0f;
        }
        return value;
    }

    /// <summary>
    /// Fonction qui permet de calculer la map
    /// des précipitations.
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

                        if(sample < 0.55f)
                            sample = 0.0f;

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
