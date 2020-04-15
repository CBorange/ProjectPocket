using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AIMachineLibrary
{
    public class AIAction
    {
        public AIAction(IEnumerator act, string identify, bool isRepeat, AIMachine machine)
        {
            myAction = act;
            this.identify = identify;
            this.isRepeat = isRepeat;
            this.machine = machine;
            transitions = new Dictionary<string, ActionTransition>();
        }

        private AIMachine machine;
        private bool isRepeat;
        public bool IsRepeat
        {
            get { return isRepeat; }
        }
        private string identify;
        public string Identify
        {
            get { return identify; }
        }
        private IEnumerator myAction;
        public IEnumerator MyAction
        {
            get { return myAction; }
        }
        private Dictionary<string, ActionTransition> transitions;
        public Dictionary<string, ActionTransition> Transitions
        {
            get { return transitions; }
        }

        // Transition Method
        public void AddTransition(string key, ActionTransition transition)
        {
            transitions.Add(key, transition);
        }
        public bool ConnectBooleanOnTransition(string transitionName, string boolName, bool valueForTransit)
        {
            ActionTransition transition = null;
            if (transitions.TryGetValue(transitionName, out transition))
            {
                transition.AddBool(boolName, valueForTransit);
                return true;
            }
            else
                return false;
        }
        public bool ConnectIntergerOnTransition(string transitionName, string integerName, int valueForTransit)
        {
            ActionTransition transition = null;
            if (transitions.TryGetValue(transitionName, out transition))
            {
                transition.AddInteger(integerName, valueForTransit);
                return true;
            }
            else
                return false;
        }
        public bool ExecuteTransit()
        {
            ActionTransition noParameterTransition = null;
            foreach (var transitionKVP in transitions)
            {
                ActionTransition transition = transitionKVP.Value;
                int parameterCount = 0;
                parameterCount += transition.BooleanParameters.Count;
                parameterCount += transition.IntegerParameters.Count;

                foreach (var booleanKVP in machine.BooleanParameters)
                {
                    if (transition.EqualsTransitTrigger_Boolean(booleanKVP.Key, booleanKVP.Value))
                    {
                        transition.ExecuteTransit();
                        return true;
                    }
                }
                foreach (var integerKVP in machine.IntegerParameters)
                {
                    if (transition.EqualsTransitTrigger_Integer(integerKVP.Key, integerKVP.Value))
                    {
                        transition.ExecuteTransit();
                        return true;
                    }
                }
                if (parameterCount == 0)
                    noParameterTransition = transition;
            }
            if (noParameterTransition != null)
            {
                noParameterTransition.ExecuteTransit();
                return true;
            }
            return false;
        }

        // Action Method
        public void ExecuteAction()
        {
            machine.SetCurrentAction(this);
            machine.StartCoroutine(myAction);
        }
        public void ResetAction(IEnumerator resetedAction)
        {
            machine.StopCoroutine(myAction);
            myAction = resetedAction;
        }
    }
}
