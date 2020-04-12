using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIMachineLibrary
{
    public class AIMachine : MonoBehaviour
    {
        // Parameters For Trigger
        private Dictionary<string, bool> booleanParameters;
        private Dictionary<string, int> integerParameters;

        // Variable For Action
        private Dictionary<string, AIAction> aiActions;
        private AIAction currentAction;

        // Variable For Controll AIMachine
        private bool ai_InOperation;

        public void Initialize()
        {
            booleanParameters = new Dictionary<string, bool>();
            integerParameters = new Dictionary<string, int>();
            aiActions = new Dictionary<string, AIAction>();
            ai_InOperation = false;
        }
        public void StartAI()
        {
            ai_InOperation = true;
            currentAction.Execute(this);
        }

        // Parameter Method
        public void AddBool(string name)
        {
            booleanParameters.Add(name, false);
        }
        public void AddInteger(string name)
        {
            integerParameters.Add(name, 0);
        }
        public void SetBool(string name, bool value)
        {

        }

        // AIAction Method
        public void AddAction(string name, AIAction action)
        {
            aiActions.Add(name, action);
        }
        public void SetEntryAction(string actionName)
        {
            if (ai_InOperation)
            {
                Debug.Log("AI가 동작중에는 EntryAction을 변경할 수 없음");
                return;
            }
            AIAction foundAction = null;
            if (!aiActions.TryGetValue(actionName, out foundAction))
            {
                PrintDebug_InvaildAction(actionName);
                return;
            }
            currentAction = foundAction;
        }
        public void AddTransition(string startActionName, string transtionKey, string targetActionName)
        {
            AIAction foundStartAction = null;
            AIAction foundTargetAction = null;
            if (!aiActions.TryGetValue(startActionName, out foundStartAction))
            {
                PrintDebug_InvaildAction(startActionName);
                return;
            }
            if (!aiActions.TryGetValue(targetActionName, out foundTargetAction))
            {
                PrintDebug_InvaildAction(targetActionName);
                return;
            }
            foundStartAction.AddTransition(transtionKey, new ActionTransition(foundTargetAction));
        }
        public void ConnectBoolOnTransition(string actionName, string parameterName, bool state, string transitionKey)
        {
            AIAction foundAction = null;
            ActionTransition transition = null;
            bool foundParameter = false;
            if (!aiActions.TryGetValue(actionName, out foundAction))
            {
                PrintDebug_InvaildAction(actionName);
                return;
            }
            if (!booleanParameters.TryGetValue(parameterName, out foundParameter))
            {
                PrintDebug_InvaildParameter(parameterName);
                return;
            }
            if (!foundAction.Transitions.TryGetValue(transitionKey, out transition))
            {
                PrintDebug_InvaildTransition(actionName, transitionKey);
                return;
            }
            transition.AddBool(parameterName, state);
        }
        public void ConnectIntegerOnTranstion(string actionName, string parameterName, int state, string transitionKey)
        {
            AIAction foundAction = null;
            ActionTransition transition = null;
            bool foundParameter = false;
            if (!aiActions.TryGetValue(actionName, out foundAction))
            {
                PrintDebug_InvaildAction(actionName);
                return;
            }
            if (!booleanParameters.TryGetValue(parameterName, out foundParameter))
            {
                PrintDebug_InvaildParameter(parameterName);
                return;
            }
            if (!foundAction.Transitions.TryGetValue(transitionKey, out transition))
            {
                PrintDebug_InvaildTransition(actionName, transitionKey);
                return;
            }
            transition.AddInteger(parameterName, state);
        }

        // AIAction Util Method
        private void PrintDebug_InvaildAction(string actionName)
        {
            Debug.Log($"AIMachine : {actionName} Key에 해당하는 액션이 존재하지 않음");
        }
        private void PrintDebug_InvaildParameter(string parameterName)
        {
            Debug.Log($"Parameter : {parameterName} Key에 해당하는 Parameter가 존재하지 않음");
        }
        private void PrintDebug_InvaildTransition(string actionName, string transitionName)
        {
            Debug.Log($"Transition : {actionName} 액션에  {transitionName} Key에 해당하는 Transition이 존재하지 않음");
        }
    }
}
