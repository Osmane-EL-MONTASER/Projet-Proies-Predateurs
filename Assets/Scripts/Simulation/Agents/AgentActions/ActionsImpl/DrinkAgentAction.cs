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
public class DrinkAgentAction : AgentAction {

    private GameObject _child;

    /// <summary>
    /// Permet d'initialiser l'attribut _agent.
    /// </summary>
    /// <param name="agent">
    /// L'agent sur lequel l'action est réalisée.
    /// </param>
    public DrinkAgentAction(Agent agent) : base(agent) { }

    /// <summary>
    /// Dans cet fonction, il est fait en sorte que 
    /// l'agent cherche de la nourriture en fonction de
    /// son régime alimentaire.
    /// 
    /// Fait par EL MONTASER Osmane le 17/04/2022.
    /// </summary>
    public override void update() {
        _agent.Attributes["Stamina"] = (Convert.ToDouble(_agent.Attributes["Stamina"]) - (Time.deltaTime * (ActionNames.TimeSpeed / ActionNames.DAY_DURATION) * ActionNames.STAMINA_FACTOR)).ToString();
        _agent.Attributes["EnergyNeeds"] = (Convert.ToDouble(_agent.Attributes["EnergyNeeds"]) + (Time.deltaTime * (ActionNames.TimeSpeed / ActionNames.DAY_DURATION) * ActionNames.ENERGY_FACTOR)).ToString();
        _agent.Attributes["WaterNeeds"] = (Convert.ToDouble(_agent.Attributes["WaterNeeds"]) + (Time.deltaTime * (ActionNames.TimeSpeed / ActionNames.DAY_DURATION) * ActionNames.WATER_FACTOR)).ToString();
        drink();
        //throw new NotImplementedException();
    }

    /// <summary>
    /// Permet aux agents de se reproduire.
    /// Les agents autotrophes se reproduisent tout seuls,
    /// tandis que les autres agents ont besoin d'un partenaire.
    ///
    /// Fait par EL MONTASER Osmane le 28/04/2022.
    /// </summary> 
    private void drink() {
        GameObject eauP = null; //Variable permettant de représenter le point d'eau le plus proche.
        double distance; //variable permettant de stocker la distance entre l'agent et un point d'eau.
        double distanceMin = System.Double.PositiveInfinity; ; //variable permettant de stocker la plus petite distance entre l'agent et le point d'eau le plus proche.
        GameObject[] eaux = GameObject.FindGameObjectsWithTag("pointEau"); // On stocke tous les points d'eau du terrain dans un tableau.
        //On recherche le point d'eau le plus proche.
        for (int i = 0 ; i < eaux.Length; i++) {
            distance = Vector3.Distance(_agent.AgentMesh.transform.position, eaux[i].transform.position);
            if (distance < distanceMin) 
            {
                eauP = eaux[i];
                distanceMin = distance;
            }
             
        }

        if (eauP == null)
            _agent.walker();

        //f (_agent.AgentMesh.destination == _agent.transform.position)
        //{
            RaycastHit hit;

            Vector3 direc = eauP.transform.position - _agent.transform.position;
            direc.x = - direc.x;
            direc.z = - direc.z;

            if (Physics.Raycast(new Vector3(eauP.transform.position.x, eauP.transform.position.y +1.0f, eauP.transform.position.z), direc, out hit, Mathf.Infinity)) 
            {
                UnityEngine.AI.NavMeshHit hitNM;
                if (UnityEngine.AI.NavMesh.SamplePosition(hit.point, out hitNM, 100.0f, 1)) {
                    _agent.AgentMesh.SetDestination(hitNM.position);
                }
            }
        //}
        if ((eauP != null) && (Vector3.Distance(_agent.AgentMesh.destination, _agent.transform.position)<=5.0f)) {
            //Si l'agent est assez proche du point d'eau...

            _agent.AgentMesh.isStopped = true; //Il s'arrête

            _agent.Attributes["WaterNeeds"] = "0";

            _agent.Attributes["IsThirsty"] = "false";

            _agent.AgentMesh.isStopped = false;
        }
    }
}