using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Classe qui réprésente une action possible 
/// pour l'agent.
/// Toutes les actions devront implémenter cette
/// classe.
/// 
/// Fait par EL MONTASER Osmane le 09/04/2022.
/// </summary>
public abstract class AgentAction {
    /// <summary>
    /// L'agent sur lequel l'action est réalisée.
    /// </summary>
    protected Agent _agent;

    /// <summary>
    /// Permet d'initialiser l'attribut _agent.
    /// </summary>
    /// <param name="agent">
    /// L'agent sur lequel l'action est réalisée.
    /// </param>
    public AgentAction(Agent agent) {
        _agent = agent;
    }

    /// <summary>
    /// Fonction à implémenter dans chaque classe
    /// de type ActionAgent. C'est la fonction qui
    /// est appelée afin d'effectuer l'action 
    /// voulue.
    /// 
    /// Fait par EL MONTASER Osmane le 09/04/2022.
    /// </summary>
    public abstract void update();
}