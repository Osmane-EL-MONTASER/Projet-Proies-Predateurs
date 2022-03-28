using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

/// <summary>
/// Classe qui gère le zoning et l'affichage des
/// zones possédant des effets météorologiques
/// spéciaux :
/// Pluie, Tempête, Vent, Orage, Sécheresse.
/// 
/// Fait par EL MONTASER Osmane le 11/03/2022.
/// </summary>
public class WeatherZoning : MonoBehaviour {
    /// <summary>
    /// La taille du carré du quadrillage.
    /// /!\ Une petite valeur pourrait causer 
    /// du lag sur de très grandes cartes. /!\
    /// </summary>
    public int SquareSize = 25;

    /// <summary>
    /// Une référence au terrain de la carte.
    /// </summary>
    public Terrain TerrainToEdit;

    /// <summary>
    /// Les nuages de la scène.
    /// </summary>
    public Volume VolumeToEdit;

    /// <summary>
    /// Shader projeté sur les textures de la scène
    /// pour simuler la pluie qui tombe.
    /// </summary>
    public GameObject RainyGround;

    /// <summary>
    /// Pour chaque case de notre quadrillage, je
    /// pré-stock les objets présents dans chaque
    /// zone (agents compris) pour permettre d'agir
    /// sur l'environnement en y accédant directement
    /// avec les coordonnées de la case. Ce qui permet
    /// d'améliorer les performances en évitant de
    /// chercher à chaque Update() parmis tous les
    /// objets de la scène.
    /// </summary>
    public List<TreeInstance>[,] GameObjects { get; set; }

    /// <summary>
    /// Combien de frames il faut pour passer à la
    /// frame suivante de l'animation des nuages.
    /// </summary>
    public float _transitionSpeed = 0.5f;

    /// <summary>
    /// Le monde est décomposé sous forme de 
    /// quadrillage. La taille d'une case est fixe
    /// et modifiable dans l'éditeur de Unity.
    /// </summary>
    private List<Weather>[,] _worldZones { get; set; }

    /// <summary>
    /// Un entier constant définie dans la classe 
    /// WeatherNames.cs.
    /// </summary>
    private int _currentTransitionType;

    /// <summary>
    /// Référence à la transition météo actuelle.
    /// </summary>
    private WeatherTransition _currentWeatherTransition;

    /// <summary>
    /// Temps depuis la dernière frame de transition.
    /// </summary>
    private float _transitionTimeAccumulator;

    /// <summary>
    /// Temps depuis la dernière actualisation des
    /// évènements météo.
    /// </summary>
    private float _weatherUpdateAccumulator;

    /// <summary>
    /// Thread qui permet de mettre à jour la météo
    /// dans chaque zone.
    /// </summary>
    private Thread _updateWeatherThread;

    /// <summary>
    /// Permet de réguler l'accès au tableau des
    /// météos de chaque zone pour les 2 threads.
    /// </summary>
    private Mutex _worldZonesMutex = new Mutex();

    /// <summary>
    /// Ici je lance la fonction de pré-chargement des
    /// arbres par zones pour pouvoir y accéder très
    /// facilement plus tard.
    /// 
    /// Fait par EL MONTASER Osmane le 14/03/2022.
    /// </summary>
    void Start() {
        initializeGameObjetsList();
        initializeWeatherZones();
        loadGameObjects();
        _transitionTimeAccumulator = .0f;
        _weatherUpdateAccumulator = .0f;

        _updateWeatherThread = new Thread(updateWeatherZones);
        _updateWeatherThread.Start();
    }

    void Update() {
        updatePlayerPosition();
        if(_currentWeatherTransition != null && _transitionTimeAccumulator >= _transitionSpeed) {
            makeTransition();
            _transitionTimeAccumulator = .0f;
        }

        _transitionTimeAccumulator++;
        _weatherUpdateAccumulator += Time.deltaTime;
    }

    private void updateWeatherZones() {
        while(true) {
            if(_weatherUpdateAccumulator >= 1f) {
                foreach(var zone in _worldZones) {
                    foreach (var weather in zone){
                        if(weather != null)
                            weather.Update();   
                    }
                }

                _weatherUpdateAccumulator = .0f;
            }
            
        }
    }

    /// <summary>
    /// Fonction qui permet de mettre à jour la météo
    /// perçue par la caméra en fonction de sa position
    /// dans la simulation.
    /// 
    /// Fait par EL MONTASER Osmane le 20/03/2022.
    /// </summary>
    private void updatePlayerPosition() {
        int xPosGrid = (int)Math.Floor(transform.position.x / SquareSize);
        int yPosGrid = (int)Math.Floor(transform.position.z / SquareSize);

        float precipitationValue = GetComponent<Precipitation>().GetPrecipitationAt(xPosGrid, yPosGrid);
        VolumeProfile profile = VolumeToEdit.sharedProfile;

        if (!profile.TryGet<VolumetricClouds>(out var clouds))
            clouds = profile.Add<VolumetricClouds>(false);

        clouds.cloudPreset.overrideState = true;
        if(precipitationValue == 0) {
            if(_currentTransitionType != WeatherNames.CLOUDY_TO_SUNNY_TRANSITION) {
                _currentWeatherTransition = new CloudyToSunnyTransition();
                _currentTransitionType = WeatherNames.CLOUDY_TO_SUNNY_TRANSITION;
            }

            GameObject.Find("RainParticles").GetComponent<ParticleSystem>().Stop();
            RainyGround.GetComponent<DecalProjector>().enabled = false;
        } else {
            if(_currentTransitionType != WeatherNames.SUNNY_TO_CLOUDY_TRANSITION) {
                _currentWeatherTransition = new SunnyToCloudyTransition();
                _currentTransitionType = WeatherNames.SUNNY_TO_CLOUDY_TRANSITION;
            }
            
            GameObject.Find("RainParticles").GetComponent<ParticleSystem>().Play();
            var emission = GameObject.Find("RainParticles").GetComponent<ParticleSystem>().emission;
            emission.rateOverTime = (float)(Math.Exp((precipitationValue - 0.5f) / 0.45) - 1) * 1000;
            RainyGround.GetComponent<DecalProjector>().enabled = true;
        }
    }

    /// <summary>
    /// Faire la transition des nuages entre plusieurs
    /// types de météo.
    /// 
    /// Fait par EL MONTASER Osmane le 20/03/2022.
    /// </summary>
    private void makeTransition() {
        Dictionary<string,float> nextSettings = _currentWeatherTransition.GetNextCloudSettings();

        if(nextSettings != null) {
            VolumeProfile profile = VolumeToEdit.sharedProfile;

            if (!profile.TryGet<VolumetricClouds>(out var clouds))
                clouds = profile.Add<VolumetricClouds>(false);
                
            clouds.cloudPreset.overrideState = true;
            clouds.cloudPreset.value = VolumetricClouds.CloudPresets.Custom;
            clouds.densityMultiplier.value = nextSettings["cloudDensity"];
            clouds.shapeFactor.value = nextSettings["shapeFactor"];
            clouds.shapeScale.value = nextSettings["shapeScale"];
            clouds.erosionFactor.value = nextSettings["erosionFactor"];
        } else
            _currentWeatherTransition = null;
    }

    /// <summary>
    /// Fonction qui permet d'ajouter un effet météorologique
    /// dans la zone donnée.
    /// 
    /// Fait par EL MONTASER Osmane le 17/03/2022.
    /// </summary>
    /// <param name="x">
    /// La position de la case à l'horizontale.
    /// </param>
    /// <param name="y">
    /// La position de la case à la verticale.
    /// </param>
    /// <param name="weather">
    /// La nouvelle météo de la zone.
    /// </param>
    public void AddWeatherAtZone(int x, int y, Weather weather) {
        if(_worldZonesMutex.WaitOne()) {
            int indexOfWeahter = -1;

            for(int i = 0; i < _worldZones[x, y].Count; i++) {
                if(_worldZones[x, y][i].GetType() == weather.GetType()) {
                    indexOfWeahter = i;
                    break;
                }
            }
            if(indexOfWeahter >= 0)
                _worldZones[x, y][indexOfWeahter] = weather;
            else
                _worldZones[x, y].Add(weather);
        }
    }

    /// <summary>
    /// Permet de supprimer un type de météo s'il existe
    /// à la zone donnée en paramètre.
    /// 
    /// Fait par EL MONTASER Osmane le 25/03/2022.
    /// </summary>
    /// <param name="x">
    /// La position de la case à l'horizontale.
    /// </param>
    /// <param name="y">
    /// La position de la case à la verticale.
    /// </param>
    /// <param name="weatherType">
    /// La météo à supprimer de la case.
    /// </param>
    public void DeleteWeatherAtZone(int x, int y, Weather weatherType) {
        int indexOfWeahter = -1;

        for(int i = 0; i < _worldZones[x, y].Count; i++) {
            if(_worldZones[x, y][i].GetType() == weatherType.GetType()) {
                indexOfWeahter = i;
                break;
            }
        }

        if(indexOfWeahter >= 0)
            _worldZones[x, y].RemoveAt(indexOfWeahter);
    }

    /// <summary>
    /// Fonction qui permet de rénitialiser la météo dans
    /// la zone donnée.
    /// 
    /// Fait par EL MONTASER Osmane le 17/03/2022.
    /// </summary>
    /// <param name="x">
    /// La position à l'horizontale de la case.
    /// </param>
    /// <param name="y">
    /// La position à la verticale de la case.
    /// </param>
    public void ResetWeatherAtZone(int x, int y) {
        _worldZones[x, y] = new List<Weather>();
    }

    /// <summary>
    /// Fonction qui permet d'initialiser les zones de
    /// météo.
    /// 
    /// Fait par EL MONTASER Osmane le 17/03/2022.
    /// </summary>
    private void initializeWeatherZones() {
        _worldZones = new List<Weather>[GameObjects.GetLength(0), GameObjects.GetLength(1)];

        for(int i = 0; i < _worldZones.GetLength(0); i++)
            for(int j = 0; j < _worldZones.GetLength(1); j++)
                _worldZones[i, j] = new List<Weather>();
    }

    /// <summary>
    /// Fonction qui permet d'initialiser la liste des
    /// GameObjects par rapport à la taille du terrain.
    /// 
    /// Fait par EL MONTASER Osmane le 14/03/2022.
    /// </summary>
    private void initializeGameObjetsList() {
        GameObjects = new List<TreeInstance>[(int)(TerrainToEdit.terrainData.size.x / SquareSize), (int)(TerrainToEdit.terrainData.size.z / SquareSize)]; 

        for(int i = 0; i < GameObjects.GetLength(0); i++)
            for(int j = 0; j < GameObjects.GetLength(1); j++)
                GameObjects[i, j] = new List<TreeInstance>();
    }

    /// <summary>
    /// Cette fonction permet d'ajouter par zone (ex :
    /// de ce à quoi cela ressemblera dans la mémoire
    /// pour la zone en bas à gauche :
    /// zone[0][0] <=> GameObjects[0][0] = {
    ///     TreeInstance tree1,
    ///     TreeInstance tree2,
    ///     Agent agent1,
    ///     Rocher rocher1...
    /// } )
    /// 
    /// Cela permet d'éviter plus tard une recherche
    /// parmis tous les objets de la scène alors que
    /// l'on peut avoir des milliers d'objets...
    /// 
    /// Fait par EL MONTASER Osmane le 14/03/2022.
    /// </summary>
    private void loadGameObjects() {
        TerrainData terrainData = TerrainToEdit.terrainData;

        foreach(TreeInstance tree in terrainData.treeInstances) {
            (int, int) gridCoordinates = getZoneCoordinatesFromRawPosition(tree.position);
            GameObjects[gridCoordinates.Item1, gridCoordinates.Item2].Add(tree);
        }
    }

    /// <summary>
    /// Permet de convertir la coordonnée Vector3 d'un 
    /// GameObject en une coordonnée de la grille.
    /// 
    /// Fait par EL MONTASER Osmane le 14/03/2022.
    /// </summary>
    /// <param name="rawPosition">
    /// La position en Vector3 de l'objet.
    /// </param>
    /// <returns>
    /// Les coordonnées correpondantes dans la grille.
    /// </returns>
    private (int, int) getZoneCoordinatesFromRawPosition(Vector3 rawPosition) {
        (int, int) gridCoordinates;
        Vector3 worldPosition = TerrainToEdit.transform.position;

        //Si le terrain ne se trouve pas à la position 0 et 
        //pour remettre les positions normales non comprises
        //entre 0 et 1 (Voir la documentation Unity).
        Vector3 normalizedPosition = new Vector3(rawPosition.x * TerrainToEdit.terrainData.size.x - worldPosition.x,
                                                 rawPosition.y * TerrainToEdit.terrainData.size.y,
                                                 rawPosition.z * TerrainToEdit.terrainData.size.z - worldPosition.z);
        gridCoordinates.Item1 = (int)Math.Floor(normalizedPosition.x / SquareSize);
        gridCoordinates.Item2 = (int)Math.Floor(normalizedPosition.z / SquareSize);

        return gridCoordinates;
    }
}
