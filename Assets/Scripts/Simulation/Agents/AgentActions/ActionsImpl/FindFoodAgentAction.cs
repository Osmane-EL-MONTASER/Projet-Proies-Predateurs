using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
public class FindFoodAgentAction : AgentAction {

    /// <summary>
    /// Permet d'initialiser l'attribut _agent.
    /// </summary>
    /// <param name="agent">
    /// L'agent sur lequel l'action est réalisée.
    /// </param>
    public FindFoodAgentAction(Agent agent) : base(agent) { }

    /// <summary>
    /// Dans cet fonction, il est fait en sorte que 
    /// l'agent cherche de la nourriture en fonction de
    /// son régime alimentaire.
    /// 
    /// Fait par EL MONTASER Osmane le 17/04/2022.
    /// </summary>
    public override void update() {
        _agent.Attributes["Stamina"] = (Convert.ToDouble(_agent.Attributes["Stamina"]) - (0.0001 * 4)).ToString();
        _agent.Attributes["EnergyNeeds"] = (Convert.ToDouble(_agent.Attributes["EnergyNeeds"]) + 0.0005).ToString();
        _agent.Attributes["WaterNeeds"] = (Convert.ToDouble(_agent.Attributes["WaterNeeds"]) + 0.0008).ToString();
        chercherAManger();

        if(_agent.AgentCible != null) {
            _agent.ForceChangeAction(_agent._currentAction.Children[0], "EnergyNeeds == -1\n<->\nEnergyNeeds == -1");
        }

        if(!_agent.Animation.GetBool("WalkTrigger"))
            _agent.affecterAnimations();
    }

    /// <summary>
    /// chercherAManger : L'agent cherche d'autres agents et chasse ceux qui sont des potentielles proies
    /// 
    /// Fait par Greg Demirdjian le 03/04/2022.
    /// </summary> 
    private void chercherAManger()
    {
        _agent.AnimauxEnVisuel.RemoveAll(n => n == null || n == null);
        if(_agent.AnimauxEnVisuel.Count == 0) // s'il n'y a pas d'animaux que l'agent voit
        {
            if((_agent.AgentMesh != null) && (_agent.AgentMesh.remainingDistance <= _agent.AgentMesh.stoppingDistance)) 
                _agent.AgentMesh.SetDestination(_agent.walker());// il se déplace 
            if (Convert.ToDouble(_agent.Attributes["EnergyNeeds"]) / Convert.ToDouble(_agent.Attributes["MaxEnergyNeeds"]) > 0.75)// s'il a très faim
                _agent.AgentMesh.speed = 0.75f * (float)Convert.ToDouble(_agent.Attributes["MaxSpeed"]); // il se déplace plus vite
        }
        else // si l'agent voit des animaux 
        {
            int rangDistMin = -1;
            for (int i = 0; i < _agent.AnimauxEnVisuel.Count; i++) // on cherche l'animal le plus proche parmi 
            {
                float distTemp = Vector3.Distance(_agent.transform.position, _agent.AnimauxEnVisuel[i].transform.position);
                if ((rangDistMin == -1) || (Vector3.Distance(_agent.transform.position, _agent.AnimauxEnVisuel[rangDistMin].transform.position) < distTemp))
                {
                    for (int j = 0; j < _agent.Preys.Count; j++)
                    {
                        Agent animalTemp = _agent.AnimauxEnVisuel[i].GetComponent<Agent>();
                        if (_agent.Preys[j] == animalTemp.Attributes["SpeciesName"]) // si l'ID de l'animal fait partie des ID des proies de l'agent.
                            rangDistMin = i; // on retient son rang dans la liste des animaux en visuel.
                    }
                }
            }
            if (rangDistMin != -1 && _agent.AnimauxEnVisuel[rangDistMin].GetComponent<Agent>() != null) // si un des animaux vus est une proie potentielle
            {
                _agent.AgentCible = _agent.AnimauxEnVisuel[rangDistMin];
            }
            else
            {
                if(_agent.AgentMesh.remainingDistance <= _agent.AgentMesh.stoppingDistance)
                {
                    _agent.AgentMesh.SetDestination(_agent.walker());
                }
            }

        }
    }
}