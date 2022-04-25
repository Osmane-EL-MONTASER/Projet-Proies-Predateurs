using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// Classe permettant d'initialiser un dictionnaire de
/// données contenant tous les attributs nécessaire à
/// un Agent.
///
/// Fait par EL MONTASER Osmane le 15/04/2022.
/// </summary> 
public static class AgentAttributes {
    /// <summary>
    /// Le dictionnaire généré par la classe.
    /// </summary>
    private static Dictionary<string, string> _attributes;
    
    public static Dictionary<string, string> GetAttributesDict() {
        _attributes = new();
        
        _attributes.Add("Range", "0");
        _attributes.Add("CarcassEnergyContribution", "0");
        _attributes.Add("WaterNeeds", "0");
        _attributes.Add("MaxWaterNeeds", "0");
        _attributes.Add("IsThirsty", "false");
        _attributes.Add("EnergyNeeds", "0");
        _attributes.Add("MaxEnergyNeeds", "0");
        _attributes.Add("IsHungry", "true");
        _attributes.Add("X", "0");
        _attributes.Add("Y", "0");
        _attributes.Add("Z", "0");
        _attributes.Add("Speed", "0");
        _attributes.Add("MaxSpeed", "0");
        _attributes.Add("Gender", "0");
        _attributes.Add("IsPregnant", "false");
        _attributes.Add("GestationPeriod", "0");
        _attributes.Add("Age", "0");
        _attributes.Add("MaturityAge", "0");
        _attributes.Add("MaxAge", "0");
        _attributes.Add("IsAdult", "false");
        _attributes.Add("DigestionTime", "0");
        _attributes.Add("RemainingDigestionTime", "0");
        _attributes.Add("PreyConsumptionTime", "0");
        _attributes.Add("Health", "0");
        _attributes.Add("MaxHealth", "0");
        _attributes.Add("IsAlive", "true");
        _attributes.Add("Stamina", "0");
        _attributes.Add("MaxStamina", "0");
        _attributes.Add("DeathCause", "");
        _attributes.Add("SpeciesName", "");
        _attributes.Add("Id", "");
        _attributes.Add("Ad", "0");

        return _attributes;
    }
}
