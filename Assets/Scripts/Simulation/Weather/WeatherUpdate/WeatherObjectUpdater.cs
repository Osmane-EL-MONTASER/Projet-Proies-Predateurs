using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Linq;

/// <summary>
/// Classe qui permet de mettre à jour les objets de
/// la scène depuis le thread principal. Cette classe
/// est nécessaire afin de ne pas surcharger le thread
/// principal à mettre à jour chaque objet de la 
/// simulation. Il est aussi impossible de changer un
/// objet de Unity à l'extérieur du thread principal.
/// 
/// Fait par EL MONTASER Osmane le 28/03/2022.
/// </summary>
public class WeatherObjectUpdater : MonoBehaviour {

    /// <summary>
    /// La liste des objets à mettre à jour lors 
    /// </summary>
    public static List<Dictionary<WeatherUpdatePropertyValued, List<TreeInstance>>> ObjectsToUpdate = new List<Dictionary<WeatherUpdatePropertyValued, List<TreeInstance>>>();
    
    /// <summary>
    /// Ce verrou permet au thread météo et au thread
    /// principal d'éditer la même liste des objets à
    /// mettre à jour.
    /// </summary>
    public static Mutex ObjectsToUpdateMutex = new Mutex();
    
    /// <summary>
    /// Une liste contenant tous les arbres tombés à
    /// cause des conditions météorologiques.
    /// Chaque arbre mort a un temps restant à vivre,
    /// quand il est écoulé, il disparaît de la 
    /// simulation.
    /// </summary>
    private static List<(GameObject, float)> _deadTrees = new List<(GameObject, float)>();

    /// <summary>
    /// Une référence au terrain de la carte.
    /// </summary>
    public Terrain TerrainToEdit;

    void Start() {

    }

    void Update() {
        if(Monitor.TryEnter(ObjectsToUpdateMutex)) {
            try {
                foreach(var item in ObjectsToUpdate.ToList()) {
                    foreach(var weather in item) {
                        switch(weather.Key.Property) {
                            case WeatherUpdateProperty.Fall:
                                TreeInstance tree;
                                try {
                                    tree = weather.Value[(int)weather.Key.Modifier];
                                } catch(System.Exception e) {
                                    ObjectsToUpdate.Remove(item);
                                    break;
                                }
                                makeTreeFall(tree, weather.Key, weather.Value, item);
                                break;
                            default:

                                break;  
                        }
                    }
                }
            } catch (System.Exception e) {}
        }

        updateDeadTrees();
    }

    /// <summary>
    /// Fonction qui permet de faire tomber un arbre.
    /// 
    /// Fait par EL MONTASER Osmane le 31/03/2022.
    /// </summary>
    /// <param name="tree">
    /// L'arbre à faire tomber.
    /// </param>
    private void makeTreeFall(TreeInstance tree, WeatherUpdatePropertyValued key, List<TreeInstance> value, Dictionary<WeatherUpdatePropertyValued, System.Collections.Generic.List<UnityEngine.TreeInstance>> item) {
        TerrainData terrainData = TerrainToEdit.terrainData;
        addDeadTreeToScene(tree);

        ArrayList instances = new ArrayList();
        foreach (var tr in terrainData.treeInstances) {
            if(!Equals(tr, tree))
                instances.Add(tr);
        }
        
        terrainData.treeInstances = (TreeInstance[])instances.ToArray(typeof(TreeInstance));
        value.RemoveAt((int)key.Modifier);
        ObjectsToUpdate.Remove(item);
    }

    /// <summary>
    /// Fonction qui permet d'ajouter un arbre mort dans la
    /// scène à partir du TreeInstance du terrain.
    /// 
    /// Fait par EL MONTASER Osmane le 31/03/2022.
    /// </summary>
    /// <param name="treeToKill">
    /// L'arbre à faire tomber puis disparaître de la scène.
    /// </param>
    private void addDeadTreeToScene(TreeInstance treeToKill) {
        TerrainData terrainData = TerrainToEdit.terrainData;
        Vector3 rawPosition = treeToKill.position;
                                
        Vector3 worldPosition = TerrainToEdit.transform.position;

        //Si le terrain ne se trouve pas à la position 0 et 
        //pour remettre les positions normales non comprises
        //entre 0 et 1 (Voir la documentation Unity).
        Vector3 normalizedPosition = new Vector3(rawPosition.x * TerrainToEdit.terrainData.size.x - worldPosition.x,
                                                rawPosition.y * TerrainToEdit.terrainData.size.y,
                                                rawPosition.z * TerrainToEdit.terrainData.size.z - worldPosition.z);
        
        GameObject newObject;
        newObject = (GameObject)Instantiate(Resources.Load(terrainData.treePrototypes[treeToKill.prototypeIndex].prefab.name + " Falling"));
        _deadTrees.Add((newObject, newObject.GetComponent<Renderer>().bounds.size.y));
        newObject.transform.position = normalizedPosition;
    }

    /// <summary>
    /// Fonction qui permet de vérifier le temps restant à
    /// vivre pour chaque arbre et de le tuer si le temps
    /// tombe à 0.
    /// 
    /// Fait par EL MONTASER Osmane le 31/03/2022.
    /// </summary>
    private void updateDeadTrees() {
        for(int i = 0; i < _deadTrees.Count; i++) {
            var time = _deadTrees[i];
            time.Item2 -= Time.deltaTime;
            _deadTrees[i] = time;

            if(_deadTrees[i].Item2 <= 0) {
                Destroy(_deadTrees[i].Item1);
                _deadTrees.Remove(_deadTrees[i]);
            }
        }
    }
}