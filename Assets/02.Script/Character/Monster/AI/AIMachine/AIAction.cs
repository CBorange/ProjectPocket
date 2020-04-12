using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AIMachineLibrary
{
    public class AIAction
    {
        public AIAction(IEnumerator act, bool repeat, string identify)
        {
            myAction = act;
            isRepeat = repeat;
            this.identify = identify;
            transitions = new Dictionary<string, ActionTransition>();
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
        private bool isRepeat;
        public bool IsRepeat
        {
            get { return isRepeat; }
        }

        private Dictionary<string, ActionTransition> transitions;
        public Dictionary<string, ActionTransition> Transitions
        {
            get { return transitions; }
        }

        public void AddTransition(string key, ActionTransition transition)
        {
            transitions.Add(key, transition);
        }
        public void Execute(AIMachine machine)
        {
            machine.StartCoroutine(myAction);
        }
    }
}
