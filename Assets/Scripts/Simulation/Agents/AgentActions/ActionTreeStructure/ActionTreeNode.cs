using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Cette classe représente une structure de données
/// en arbre. Nous l'utilisons pour représenter toutes
/// les actions possibles qu'un agent pourra réaliser.
/// 
/// Fait par EL MONTASER Osmane le 08/04/2022.
/// </summary>
public class ActionTreeNode<T> {
    /// <summary>
    /// La liste des actions filles du noeud courant.
    /// </summary>
    private List<ActionTreeNode<T>> _children;

    /// <summary>
    /// L'action courante présente dans le noeud.
    /// </summary>
    private T _action;

    /// <summary>
    /// Permet d'initialiser l'arbre des actions
    /// possibles d'un agent.
    /// 
    /// Fait par EL MONTASER Osmane le 08/04/2022.
    /// </summary>
    /// <param name="action"></param>
    public ActionTreeNode(T action) {
        _action = action;
    }

    /// <summary>
    /// Permet d'ajouter un noeud non existant dans la 
    /// liste des noeuds fils de l'arbre.
    /// 
    /// Fait par EL MONTASER Osmane le 08/04/2022.
    /// </summary>
    /// <param name="childAction">
    /// Le noeud fils à ajouter dans la liste.
    /// </param>
    public void AddChild(ActionTreeNode<T> childAction) {
        if(_children.Contains(childAction))
            throw new ArgumentException("Node already contained in ActionTreeNode!");
        else
            _children.Add(childAction);
    }
}