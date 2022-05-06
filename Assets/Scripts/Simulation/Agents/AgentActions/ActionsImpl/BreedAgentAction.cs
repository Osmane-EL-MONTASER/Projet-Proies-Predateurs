using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Linq;

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

    private GameObject _mate;

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
            GameObject go = GameObject.Instantiate(GameObject.Find("AgentManager").GetComponent<AgentManager>().Grass, 
                new Vector3(randomX, 
                Terrain.activeTerrain.SampleHeight(new Vector3(randomX, 1f, randomY)),
                randomY), Quaternion.identity);
            go.name = "Grass";
            oldLocalScale = go.transform.localScale;
            go.transform.localScale = new Vector3(0, 0, 0);
            _child = go;
        } else if(!_agent.Attributes["SpeciesName"].Equals("Grass")) {
            _mate = _agent.GetMate();
            if(_mate == null)
                findMate(_agent);
            else if(_mate != null) {
                _agent.AgentMesh.SetDestination(_mate.GetComponent<Agent>().transform.position);
                if(!(_agent.AgentMesh.remainingDistance <= _agent.AgentMesh.stoppingDistance)) {
                    Debug.Log("Now breeding...");
                    
                    if(_agent.Attributes["Gender"].Equals("2"))
                        _agent.Attributes["IsPregnant"] = "true";

                    _agent.Attributes["EnergyNeeds"] = _agent.Attributes["SpeciesName"].Equals("Grass") ? "1.0" : _agent.Attributes["EnergyNeeds"];
                    _agent.Attributes["Stamina"] = _agent.Attributes["SpeciesName"].Equals("Grass") ? "1.0" : "0.38";
                    _mate = null;
                }
            }
        } 
        if(_child != null) {
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

    private void findMate(Agent agent) {
        if(!_agent.Attributes["SpeciesName"].Equals("Grass") 
            && (_agent.AgentMesh != null) 
            && (_agent.AgentMesh.remainingDistance <= _agent.AgentMesh.stoppingDistance)) {
            _agent.AgentMesh.SetDestination(_agent.walker());
        }
        IEnumerable<GameObject> possibleMates = from candidate in _agent.animauxDansFov()
                            where candidate.GetComponent<Agent>().Attributes["SpeciesName"].Equals(_agent.Attributes["SpeciesName"]) && candidate.GetComponent<Agent>().Attributes["Gender"].Equals(_agent.Attributes["Gender"])
                            select candidate;
    
        if(possibleMates.Count() != 0) {
            Debug.Log("Found a " + _agent.Attributes["SpeciesName"] + " to mate with!");
            possibleMates.First().GetComponent<Agent>().SetMate(_agent.gameObject);
            _agent.SetMate(possibleMates.First());
        }
    }

    /// <summary>
    /// Savoir si l'agent en question est en capacité
    /// de se reproduire. Cela afin d'éviter que des
    /// animaux enceintes ne se reproduisent ainsi
    /// que les animaux n'ayant pas la majorité
    /// sexuelle.
    /// La monogamie est activée par défaut.
    /// 
    /// Fait par EL MONTASER Osmane le 06/05/2022.
    /// </summary>
    /// <param name="agent">
    /// 
    /// </param>
    /// <returns></returns>
    private bool isBreedingReady(Agent agent) {
        return false;
    }
}