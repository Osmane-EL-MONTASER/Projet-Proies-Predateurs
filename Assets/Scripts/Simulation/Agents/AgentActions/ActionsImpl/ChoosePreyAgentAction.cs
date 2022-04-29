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
public class ChoosePreyAgentAction : AgentAction {

    /// <summary>
    /// Permet d'initialiser l'attribut _agent.
    /// </summary>
    /// <param name="agent">
    /// L'agent sur lequel l'action est réalisée.
    /// </param>
    public ChoosePreyAgentAction(Agent agent) : base(agent) { }

    /// <summary>
    /// Dans cet fonction, il est fait en sorte que 
    /// l'agent cherche de la nourriture en fonction de
    /// son régime alimentaire.
    /// 
    /// Fait par EL MONTASER Osmane le 17/04/2022.
    /// </summary>
    public override void update() {
        //Debug.Log("Chasing prey...  EnergyNeeds = " + _agent.Attributes["EnergyNeeds"]);
        _agent.Attributes["Stamina"] = (Convert.ToDouble(_agent.Attributes["Stamina"]) + ActionNames.STAMINA_FACTOR).ToString();
        _agent.Attributes["EnergyNeeds"] = (Convert.ToDouble(_agent.Attributes["EnergyNeeds"]) + ActionNames.ENERGY_FACTOR).ToString();
        _agent.Attributes["WaterNeeds"] = (Convert.ToDouble(_agent.Attributes["WaterNeeds"]) + ActionNames.WATER_FACTOR).ToString();
        
        if(Convert.ToDouble(_agent.Attributes["Stamina"]) < 0.25) {
            _agent.ForceChangeAction(_agent._actionTree, "<->\nStamina >= 1");
            return;
        }

        if(_agent.AgentCible == null 
            || _agent.AgentCible.GetComponent<Agent>() == null) {
            _agent.ForceChangeAction(_agent._actionTree, _agent._actionTree.ParentTransition);
            return;
        }

        chasser();

        //throw new NotImplementedException();
    }

    /// <summary>
    /// chasser : l'agent chasse un autre agent
    ///
    /// Fait par Greg Demirdjian le 03/04/2022.
    /// </summary> 
    private void chasser() {
        Agent animalTemp = _agent.AgentCible.GetComponent<Agent>();

        float dist = Vector3.Distance(_agent.transform.position, _agent.AgentCible.transform.position);

        if (dist <= 2.0f)
        {
            _agent.AgentMesh.isStopped = true;

            if (bool.Parse(animalTemp.Attributes["IsAlive"])) // si la cible est en vie
            {
                animalTemp.Attributes["Health"] = (Convert.ToDouble(animalTemp.Attributes["Health"]) - Convert.ToDouble(_agent.Attributes["Ad"])).ToString(); //l'agent attaque la cible
                // rajotuer les anim si dispo
            }
            else if (Convert.ToDouble(animalTemp.Attributes["CarcassEnergyContribution"]) >= 10.0)
            {
                animalTemp.Attributes["CarcassEnergyContribution"] = (Convert.ToDouble(animalTemp.Attributes["CarcassEnergyContribution"]) - 0.5).ToString();
                _agent.Attributes["EnergyNeeds"] = (Convert.ToDouble(_agent.Attributes["EnergyNeeds"]) - 0.5).ToString();
                if (Convert.ToDouble(_agent.Attributes["EnergyNeeds"]) < 0.0)
                    _agent.Attributes["EnergyNeeds"] = (0.0).ToString();
            }
        }
        else
        {
            _agent.AgentMesh.SetDestination(_agent.AgentCible.transform.position);
            _agent.AgentMesh.isStopped = false;
        }

        if ((Convert.ToDouble(animalTemp.Attributes["CarcassEnergyContribution"]) < 10.0) || (Convert.ToDouble(_agent.Attributes["EnergyNeeds"])<=0.0))
        {
            _agent.AgentMesh.isStopped = false;
            _agent.AnimauxEnVisuel.Remove(_agent.AgentCible);
            _agent.AgentCible = null;
            if ((Convert.ToDouble(_agent.Attributes["EnergyNeeds"]) / Convert.ToDouble(_agent.Attributes["MaxEnergyNeeds"]) <= 0.25)&&(_agent.Attributes["IsHungry"]=="true"))
            {
                _agent.Attributes["IsHungry"]="false";
            }           
        }
        else
        {
            // ajouter une condition pour les agents ayant le trait : chasse en meute
            for (int i = 0 ; i < _agent.AnimauxEnVisuel.Count ; i++)
            {
                if(_agent.AnimauxEnVisuel[i] != null) {
                    Agent animalTemp2 = _agent.AnimauxEnVisuel[i].GetComponent<Agent>();
                    if (( _agent.Attributes["SpeciesName"] == animalTemp2.Attributes["SpeciesName"] ) && ( animalTemp2.AgentCible == null ))
                        animalTemp2.AgentCible = _agent.AgentCible;
                }
            }
        }

    }
}