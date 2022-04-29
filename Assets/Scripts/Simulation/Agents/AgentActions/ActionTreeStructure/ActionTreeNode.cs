using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeEditor {
    /// <summary>
    /// Cette classe représente une structure de données
    /// en arbre. Nous l'utilisons pour représenter toutes
    /// les actions possibles qu'un agent pourra réaliser.
    /// 
    /// Fait par EL MONTASER Osmane le 08/04/2022.
    /// </summary>
    public class ActionTreeNode<T> {
        public T Action { get { return _action; } set { _action = value; } }
        public List<ActionTreeNode<T>> Children { get { return _children; } set { _children = value; } }

        public ActionTreeNode<T> Parent;

        public string ParentTransition;

        public List<string> TransitionsCond { get { return _transitionsCond; } set { _transitionsCond = value; } }

        /// <summary>
        /// L'id du node crée, ce qui permet de le différencier
        /// avec d'autre noeuds.
        /// </summary>
        public int Id;

        /// <summary>
        /// Permet de donner un Id unique pour chaque noeud
        /// créé.
        /// </summary>
        public static int IdCounter = 0;

        /// <summary>
        /// La liste des actions filles du noeud courant.
        /// </summary>
        private List<ActionTreeNode<T>> _children;
        
        /// <summary>
        /// La liste de chaque ensembles de conditions menant 
        /// au nouvel état de l'agent. Chaque ensemble contient
        /// un string contenant plusieurs conditions séparées par
        /// un retour chariot.
        /// </summary>
        private List<string> _transitionsCond;

        /// <summary>
        /// L'action courante présente dans le noeud.
        /// </summary>
        private T? _action;

        /// <summary>
        /// Permet d'initialiser l'arbre des actions
        /// possibles d'un agent.
        /// 
        /// Fait par EL MONTASER Osmane le 08/04/2022.
        /// </summary>
        /// <param name="action"></param>
        public ActionTreeNode(T action) {
            _action = action;
            _children = new();
            _transitionsCond = new();
            Id = IdCounter++;
        }

        public ActionTreeNode() { 
            _children = new();
            _transitionsCond = new();
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
            if (_children.Contains(childAction))
                throw new ArgumentException("Node already contained in ActionTreeNode!");
            else {
                childAction.Parent = this;
                _children.Add(childAction);
            }
        }
    }
}
