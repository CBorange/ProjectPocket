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
        private Dictionary<string, int> integerParameters;

        public void AddBool(string parameterName, bool stateForTransit)
        {
            booleanParameters.Add(parameterName, stateForTransit);
        }
        public void AddInteger(string parameterName, int stateForTransit)
        {
            integerParameters.Add(parameterName, stateForTransit);
        }
    }
}
