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
        Debug.Log("Breeding... " + _agent.Attributes["EnergyNeeds"]);
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
            _child = go;
        } else if(_child != null) {
            Agent child = _child.GetComponent<Agent>();
            string name = child.Attributes["SpeciesName"].Split('(')[0];
            
            new Thread(() => {
                child.Db.AddAgent(child.Attributes["Id"], name, .0f, -1.0f, 0, child.Db.SelectSpeciesId(name), Convert.ToInt32(child.Attributes["Gender"]));
            }).Start();
            
            child.Attributes["EnergyNeeds"] = "1.0";
            _agent.Attributes["EnergyNeeds"] = "1.0";
            _child = null;
        }
    }
}