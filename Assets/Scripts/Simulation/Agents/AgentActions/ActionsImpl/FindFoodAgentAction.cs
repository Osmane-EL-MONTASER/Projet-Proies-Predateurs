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
        Debug.Log("Finding Food...");

    }

    /// <summary>
    /// chercherAManger : L'agent cherche d'autres agents et chasse ceux qui sont des potentielles proies
    /// 
    /// Fait par Greg Demirdjian le 03/04/2022.
    /// </summary> 
    private void chercherAManger()
    {
        
        if (AnimauxEnVisuel.Count == 0) // s'il n'y a pas d'animaux que l'agent voit
        {
            if((AgentMesh != null) && (AgentMesh.remainingDistance <= AgentMesh.stoppingDistance)) 
                AgentMesh.SetDestination(walker());// il se déplace 
            if (Convert.ToDouble(Attributes["EnergyNeeds"]) / Convert.ToDouble(Attributes["MaxEnergyNeeds"]) > 0.75)// s'il a très faim
                AgentMesh.speed = 0.75f * (float)Convert.ToDouble(Attributes["MaxSpeed"]); // il se déplace plus vite
        }
        else // si l'agent voit des animaux 
        {
            int rangDistMin = -1;
            for (int i = 0; i < AnimauxEnVisuel.Count; i++) // on cherche l'animal le plus proche parmi 
            {
                float distTemp = Vector3.Distance(transform.position, AnimauxEnVisuel[i].transform.position);
                if ((rangDistMin == -1) || (Vector3.Distance(transform.position, AnimauxEnVisuel[rangDistMin].transform.position) < distTemp))
                {
                    for (int j = 0; j < preys.Count; j++)
                    {
                        Agent animalTemp = AnimauxEnVisuel[i].GetComponent<Agent>();
                        if (preys[j] == animalTemp.Attributes["SpeciesName"]) // si l'ID de l'animal fait partie des ID des proies de l'agent.
                            rangDistMin = i; // on retient son rang dans la liste des animaux en visuel.
                    }
                }
            }
            if (rangDistMin != -1) // si un des animaux vus est une proie potentielle
            {
                AgentCible = AnimauxEnVisuel[rangDistMin];
            }
            else
            {
                AgentMesh.SetDestination(walker());
            }

        }
    }
}