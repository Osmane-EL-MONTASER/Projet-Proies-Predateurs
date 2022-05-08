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
/// Fait par EL MONTASER Osmane le 10/04/2022. 
/// </summary>
public class SleepAgentAction : AgentAction {

    /// <summary>
    /// Permet d'initialiser l'attribut _agent.
    /// </summary>
    /// <param name="agent">
    /// L'agent sur lequel l'action est réalisée.
    /// </param>
    public SleepAgentAction(Agent agent) : base(agent) { }

    /// <summary>
    /// Dans cet fonction, TODO
    /// 
    /// Fait par EL MONTASER Osmane le 10/04/2022.
    /// </summary>
    public override void update() {
        //Debug.Log("Sleeping ... Stamina = " + _agent.Attributes["Stamina"]);

        if(_agent.AgentMesh.destination != _agent.transform.position)
            _agent.AgentMesh.SetDestination(_agent.transform.position);

        _agent.Attributes["Stamina"] = (Convert.ToDouble(_agent.Attributes["Stamina"]) + (Time.deltaTime * (ActionNames.TimeSpeed / ActionNames.DAY_DURATION) * ActionNames.STAMINA_FACTOR * 4)).ToString();
        _agent.Attributes["EnergyNeeds"] = (Convert.ToDouble(_agent.Attributes["EnergyNeeds"]) + (Time.deltaTime * (ActionNames.TimeSpeed / ActionNames.DAY_DURATION) * ActionNames.ENERGY_FACTOR)).ToString();
        _agent.Attributes["WaterNeeds"] = (Convert.ToDouble(_agent.Attributes["WaterNeeds"]) + (Time.deltaTime * (ActionNames.TimeSpeed / ActionNames.DAY_DURATION) * ActionNames.WATER_FACTOR)).ToString();
        
        if(!_agent.Animation.GetBool("IdleTrigger"))
            handleAnimation();
    }

    private void handleAnimation() {
        _agent.Animation.ResetTrigger("WalkTrigger");
        _agent.Animation.ResetTrigger("DeadTrigger");
        _agent.Animation.ResetTrigger("AttackTrigger");
        _agent.Animation.ResetTrigger("EatTrigger");
        _agent.Animation.SetTrigger("IdleTrigger");
    }
}