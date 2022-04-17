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
public class IdleAgentAction : AgentAction {

    /// <summary>
    /// Permet d'initialiser l'attribut _agent.
    /// </summary>
    /// <param name="agent">
    /// L'agent sur lequel l'action est réalisée.
    /// </param>
    public IdleAgentAction(Agent agent) : base(agent) { }

    /// <summary>
    /// Dans cet fonction, il est vérifié si l'agent
    /// a bien changé d'animation, qu'il n'a plus de
    /// destination.
    /// 
    /// Fait par EL MONTASER Osmane le 10/04/2022.
    /// </summary>
    public override void update() {
        Debug.Log("Idling...");
        //throw new NotImplementedException();
    }
}