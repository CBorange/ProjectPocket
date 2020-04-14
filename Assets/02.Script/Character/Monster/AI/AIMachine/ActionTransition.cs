using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIMachineLibrary
{
    public class ActionTransition
    {
        public ActionTransition(AIAction target)
        {
            targetAction = target;
            booleanParameters = new Dictionary<string, bool>();
            integerParameters = new Dictionary<string, int>();
        }
        
        private AIAction targetAction;
        public AIAction TargetAction
        {
            get { return targetAction; }
        }

        private Dictionary<string, bool> booleanParameters;
        public Dictionary<string, bool> BooleanParameters
        {
            get { return booleanParameters; }
        }
        private Dictionary<string, int> integerParameters;
        public Dictionary<string, int> IntegerParameters
        {
            get { return integerParameters; }
        }

        public void AddBool(string parameterName, bool stateForTransit)
        {
            booleanParameters.Add(parameterName, stateForTransit);
        }
        public void AddInteger(string parameterName, int stateForTransit)
        {
            integerParameters.Add(parameterName, stateForTransit);
        }

        public bool EqualsTransitTrigger_Boolean(string parameterName, bool value)
        {
            bool foundValue;
            if (booleanParameters.TryGetValue(parameterName, out foundValue))
            {
                if (foundValue == value)
                    return true;
            }
            return false;
        }
        public bool EqualsTransitTrigger_Integer(string parameterName, int value)
        {
            int foundValue;
            if (integerParameters.TryGetValue(parameterName, out foundValue))
            {
                if (foundValue == value)
                    return true;
            }
            return false;
        }
        public void ExecuteTransit()
        {
            targetAction.ExecuteAction();
        }
    }
}
