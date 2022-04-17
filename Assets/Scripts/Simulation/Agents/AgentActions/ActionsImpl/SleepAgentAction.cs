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
        Debug.Log("Sleeping...");
        //throw new NotImplementedException();
    }
}