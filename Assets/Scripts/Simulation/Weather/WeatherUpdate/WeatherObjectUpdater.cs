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

    public static List<Dictionary<WeatherUpdatePropertyValued, List<TreeInstance>>> ObjectsToUpdate = new List<Dictionary<WeatherUpdatePropertyValued, List<TreeInstance>>>();
    public static Mutex ObjectsToUpdateMutex = new Mutex();

    /// <summary>
    /// Une référence au terrain de la carte.
    /// </summary>
    public Terrain TerrainToEdit;

    void Update() {
        if(Monitor.TryEnter(ObjectsToUpdateMutex)) {
            TerrainData terrainData = TerrainToEdit.terrainData;
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
                                Vector3 rawPosition = tree.position;
                                
                                Vector3 worldPosition = TerrainToEdit.transform.position;

                                //Si le terrain ne se trouve pas à la position 0 et 
                                //pour remettre les positions normales non comprises
                                //entre 0 et 1 (Voir la documentation Unity).
                                Vector3 normalizedPosition = new Vector3(rawPosition.x * TerrainToEdit.terrainData.size.x - worldPosition.x,
                                                                        rawPosition.y * TerrainToEdit.terrainData.size.y,
                                                                        rawPosition.z * TerrainToEdit.terrainData.size.z - worldPosition.z);
                                
                                GameObject newObject;
                                newObject = (GameObject)Instantiate(Resources.Load(terrainData.treePrototypes[tree.prototypeIndex].prefab.name + " Falling"));
                                    
                                newObject.transform.position = normalizedPosition;

                                ArrayList instances = new ArrayList();
                                foreach (var tr in terrainData.treeInstances) {
                                    if(!Equals(tr, tree))
                                        instances.Add(tr);
                                }
                                
                                terrainData.treeInstances = (TreeInstance[])instances.ToArray(typeof(TreeInstance));
                                weather.Value.RemoveAt((int)weather.Key.Modifier);
                                ObjectsToUpdate.Remove(item);
                                break;
                            default:

                                break;  
                        }
                    }
                }
            } catch (System.Exception e) {}
        }
    }
}