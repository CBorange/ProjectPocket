using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AIMachineLibrary
{
    public class AIMachine : MonoBehaviour
    {
        // Parameters For Trigger
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
            currentAction.ExecuteAction();
        }

        // Parameter Method
        public void AddBool(string name, bool initValue)
        {
            booleanParameters.Add(name, initValue);
        }
        public void AddInteger(string name, int initValue)
        {
            integerParameters.Add(name, initValue);
        }
        public void SetBool(string parameterName, bool newValue)
        {
            bool foundParameter = false;
            if (booleanParameters.TryGetValue(parameterName, out foundParameter))
            {
                booleanParameters[parameterName] = newValue;
                currentAction.ExecuteTransit();
            }
            else
                PrintDebug_InvaildParameter(parameterName);
        }
        public void SetInteger(string parameterName, int newValue)
        {
            int foundParameter = 0;
            if (integerParameters.TryGetValue(parameterName, out foundParameter))
            {
                integerParameters[parameterName] = newValue;
                currentAction.ExecuteTransit();
            }
            else
                PrintDebug_InvaildParameter(parameterName);
        }

        // AIAction Method
        public void AddAction(string actionName, IEnumerator action, bool isRepeat)
        {
            aiActions.Add(actionName, new AIAction(action, actionName, isRepeat, this));
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
        public void ConnectBoolOnTransition(string actionName, string parameterName, bool valueForTransit, string transitionName)
        {
            AIAction foundAction = null;
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
            if (!foundAction.ConnectBooleanOnTransition(transitionName, parameterName, valueForTransit))
                PrintDebug_InvaildTransition(actionName, transitionName);
        }
        public void ConnectIntegerOnTranstion(string actionName, string parameterName, int valueForTransit, string transitionName)
        {
            AIAction foundAction = null;
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
            if (!foundAction.ConnectIntergerOnTransition(transitionName, parameterName, valueForTransit))
                PrintDebug_InvaildTransition(actionName, transitionName);
        }

        // Callback
        public void EndAction(IEnumerator resetedRoutine)
        {
            currentAction.ResetAction(resetedRoutine);
            if (!currentAction.ExecuteTransit())
            {
                if (currentAction.IsRepeat)
                    currentAction.ExecuteAction();
            }
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
