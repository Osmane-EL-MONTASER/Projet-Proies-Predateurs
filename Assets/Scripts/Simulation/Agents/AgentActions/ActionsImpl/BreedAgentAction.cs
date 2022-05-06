using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

/// <summary>
/// Classe qui permet de déterminer toutes les
/// actions à faire lorsque l'agent est dans l'état
/// "Idle".
/// Lorsque l'agent est dans cet état, il ne bouge
/// plus et est dans l'attente d'une nouvelle
/// action à réaliser.
/// 
/// Fait par EL MONTASER Osmane le 17/04/2022. 
/// </summary>
public class BreedAgentAction : AgentAction {

    private GameObject _child;
    private Vector3 oldLocalScale;

    /// <summary>
    /// Permet d'initialiser l'attribut _agent.
    /// </summary>
    /// <param name="agent">
    /// L'agent sur lequel l'action est réalisée.
    /// </param>
    public BreedAgentAction(Agent agent) : base(agent) { }

    /// <summary>
    /// Dans cet fonction, il est fait en sorte que 
    /// l'agent cherche de la nourriture en fonction de
    /// son régime alimentaire.
    /// 
    /// Fait par EL MONTASER Osmane le 17/04/2022.
    /// </summary>
    public override void update() {
        breed();
        //throw new NotImplementedException();
    }

    /// <summary>
    /// Permet aux agents de se reproduire.
    /// Les agents autotrophes se reproduisent tout seuls,
    /// tandis que les autres agents ont besoin d'un partenaire.
    ///
    /// Fait par EL MONTASER Osmane le 28/04/2022.
    /// </summary> 
    private void breed() {
        if(_agent.Attributes["SpeciesName"].Equals("Grass") && _child == null) {
            System.Random rnd = new System.Random();
            float randomX = rnd.Next((int)_agent.transform.position.x - 5, (int)_agent.transform.position.x + 5);
            float randomY = rnd.Next((int)_agent.transform.position.z - 5, (int)_agent.transform.position.z + 5);
            GameObject go = GameObject.Instantiate(GameObject.Find("GestionAgents").GetComponent<GestionAgents>().Grass, 
                new Vector3(randomX, 
                Terrain.activeTerrain.SampleHeight(new Vector3(randomX, 1f, randomY)),
                randomY), Quaternion.identity);
            go.name = "Grass";
            oldLocalScale = go.transform.localScale;
            go.transform.localScale = new Vector3(0, 0, 0);
            _child = go;
        }else if(!_agent.Attributes["SpeciesName"].Equals("Grass") && _child == null) {
            System.Random rnd = new System.Random();
            float randomX = rnd.Next((int)_agent.transform.position.x - 1, (int)_agent.transform.position.x + 1);
            float randomY = rnd.Next((int)_agent.transform.position.z - 1, (int)_agent.transform.position.z + 1);
            GameObject go = GameObject.Instantiate(_agent.gameObject, 
                new Vector3(randomX, 
                Terrain.activeTerrain.SampleHeight(new Vector3(randomX, 1f, randomY)),
                randomY), Quaternion.identity);
            go.name = go.name.Split("(")[0];
            oldLocalScale = go.transform.localScale;
            go.transform.localScale = new Vector3(0, 0, 0);
            _child = go;
        } else if(_child != null) {
            Agent child;
            if((_child.name.Equals("Rabbit") && new System.Random().NextDouble() > 0.25)
                || (!_child.name.Equals("Rabbit") && new System.Random().NextDouble() > 0.4)) {
                child = _child.GetComponent<Agent>();
                _child.transform.localScale = oldLocalScale;
                child.initialisation();
                string name = child.Attributes["SpeciesName"].Split('(')[0];
                child.Attributes["EnergyNeeds"] = _agent.Attributes["SpeciesName"].Equals("Grass") ? "1.0" : "0.0";
                Debug.Log("Successfully breeding!");
                GameObject.Find("Player").GetComponent<DataUpdater>().AddNewAgent(child);
            } else {
                GameObject.Destroy(_child);
                Debug.Log("Failed breeding!");
            }
            
            _agent.Attributes["EnergyNeeds"] = _agent.Attributes["SpeciesName"].Equals("Grass") ? "1.0" : _agent.Attributes["EnergyNeeds"];
            _agent.Attributes["Stamina"] = _agent.Attributes["SpeciesName"].Equals("Grass") ? "1.0" : "0.38";
            _child = null;
        }
    }
}