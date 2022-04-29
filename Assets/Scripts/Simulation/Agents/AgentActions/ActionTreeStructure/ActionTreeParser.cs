using System;
using TreeEditor;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// Cette classe contient les fonctions requises
/// afin de convertir les ActionTreeNode<string>
/// en ActionTreeNode<AgentAction>.
/// Elle possède aussi des fonctions afin de convertir
/// les listes de conditions(List<string>) en un booléen
/// true ou false.
/// 
/// Fait par EL MONTASER Osmane le 16/04/2022.
/// </summary>
public static class ActionTreeParser {
    /// <summary>
    /// Fonction qui permet de convertir l'abre de décisions
    /// de chaînes de caractères fait sous le logiciel 
    /// d'édition d'arbres en arbre de décisions avec des
    /// références à un vrai objet AgentAction.
    /// 
    /// Fait par EL MONTASER Osmane le 17/04/2022.
    /// </summary>
    /// <param name="strActionTree">
    /// L'arbre de décisions à convertir.
    /// </param>
    /// <returns>
    /// L'arbre de décisions converti et utilisable par le
    /// laboratoire et les agents.
    /// </returns>
    public static ActionTreeNode<AgentAction> ParseStringActionTree(ActionTreeNode<string> strActionTree, Agent agent) {
        ActionTreeNode<AgentAction> actionTree = new(stringActionToAgentAction(strActionTree.Action, agent));
        DFS(actionTree, strActionTree, agent);

        return actionTree;
    }

    /// <summary>
    /// Fonction qui permet de convertir le texte présent dans chaque
    /// condition de transition en un booléen, vrai ou faux.
    /// 
    /// Fait par EL MONTASER Osmane le 17/04/2022.
    /// </summary>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static bool CondTextToBool(string condition, Agent agent, bool reversed = false) {
        if(condition == "")
            return false;

        string[] delimiters = new [] { "<=", ">=", "<" , ">", "==", "!=" };
        bool result = true;
        string condition1, condition2, conditionTemp;
        string[] conditionSplit = condition.Split("<->");
        
        condition1 = conditionSplit[0];
        condition2 = conditionSplit[1];
        
        if(reversed)
            conditionTemp = conditionSplit[1];
        else
            conditionTemp = conditionSplit[0];

        using(StringReader reader = new StringReader(conditionTemp)) {
            string line;
            //Pour chaque condition séparée par une ligne.
            while((line = reader.ReadLine()) != null) {
                line = line.Replace(" ", "");
                string[] splited = line.Split(delimiters, StringSplitOptions.None);
                
                if(line.Contains("<=")) 
                    result = result && Convert.ToDouble(agent.Attributes[splited[0]]) <= Convert.ToDouble(splited[1]);
                else if(line.Contains(">="))
                    result = result && Convert.ToDouble(agent.Attributes[splited[0]]) >= Convert.ToDouble(splited[1]);
                else if(line.Contains(">"))
                    result = result && Convert.ToDouble(agent.Attributes[splited[0]]) > Convert.ToDouble(splited[1]);
                else if(line.Contains("<"))
                    result = result && Convert.ToDouble(agent.Attributes[splited[0]]) < Convert.ToDouble(splited[1]);
                else if(line.Contains("=="))
                    result = result && Convert.ToDouble(agent.Attributes[splited[0]]) == Convert.ToDouble(splited[1]);
                else if(line.Contains("!="))
                    result = result && Convert.ToDouble(agent.Attributes[splited[0]]) != Convert.ToDouble(splited[1]);
            }
        }
        
        return result;
    }

    /// <summary>
    /// Reads an object instance from an XML file.
    /// <para>Object type must have a parameterless constructor.</para>
    /// 
    /// Fait par Manuel Faux, 
    /// https://stackoverflow.com/questions/6115721/how-to-save-restore-serializable-object-to-from-file
    /// </summary>
    /// <typeparam name="T">The type of object to read from the file.</typeparam>
    /// <param name="filePath">The file path to read the object instance from.</param>
    /// <returns>Returns a new instance of the object read from the XML file.</returns>
    public static T ReadFromXmlFile<T>(string filePath) where T : new() {
        TextReader reader = null;
        try {
            var serializer = new XmlSerializer(typeof(T));
            reader = new StreamReader(filePath);
            return (T)serializer.Deserialize(reader);
        } finally {
            if (reader != null)
                reader.Close();
        }
    }

    /// <summary>
    /// Fonction qui permet de construire l'arbre à partir d'un
    /// arbre de décisions de string avec un DFS.
    /// 
    /// Fait par EL MONTASER Osmane le 17/04/2022.
    /// </summary>
    /// <param name="tree"></param>
    /// <param name="strActionTree"></param>
    /// <param name="agent"></param>
    private static void DFS(ActionTreeNode<AgentAction> tree, ActionTreeNode<string> strActionTree,
                                                     Agent agent) {
        foreach(ActionTreeNode<string> node in strActionTree.Children) {
            if(strActionTree.TransitionsCond.Count != 0)
                tree.TransitionsCond.Add(strActionTree.TransitionsCond[tree.Children.Count]);

            ActionTreeNode<AgentAction> childAction = new ActionTreeNode<AgentAction>(stringActionToAgentAction(node.Action, agent));
            tree.AddChild(childAction);
            DFS(tree.Children[tree.Children.Count - 1], node, agent);
        }
    }

    /// <summary>
    /// Permet de convertir les actions en string en vrai
    /// instance AgentAction utilisable par les agents.
    /// 
    /// Fait par EL MONTASER Osmane le 17/04/2022.
    /// </summary>
    /// <param name="action"></param>
    /// <param name="agent"></param>
    /// <returns></returns>
    private static AgentAction stringActionToAgentAction(string action, Agent agent) {
        switch(action) {
            case ActionNames.IDLE_ACTION:
                return new IdleAgentAction(agent);
                break;
            case ActionNames.FIND_FOOD_ACTION:
                return new FindFoodAgentAction(agent);
                break;
            case ActionNames.SLEEP_ACTION:
                return new SleepAgentAction(agent);
                break;
            case ActionNames.CHOOSE_PREY_ACTION:
                return new ChoosePreyAgentAction(agent);
                break;
            case ActionNames.EAT_ACTION:
                return new EatAgentAction(agent);
                break;
            case ActionNames.BREED_ACTION:
                return new BreedAgentAction(agent);
                break;
            case ActionNames.DRINK_ACTION:
                return new DrinkAgentAction(agent);
                break;
            default:
                return new IdleAgentAction(agent);
        }
    }
}