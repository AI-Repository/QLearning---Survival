using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Assets.Scripts.GameManager;
using Assets.Scripts.QLearning.Actions;
using Random = System.Random;

using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.QLearning
{
    public class ReinforcementProblem 
    {
        static List<Action> availableActions;

        public ReinforcementProblem()
        {
        }

        // Choose a random starting state for the problem
        public State getRandomState()
        {
            Random rnd = new Random();

            bool is_obstable = true;

            State result = new State();

            while (is_obstable)
            {
                int i = rnd.Next(0, LearningCharacter.all_states.Count);

                


                //result = LearningCharacter.all_states[i];
                int key = LearningCharacter.all_states.Keys.ElementAt(i);

                result = LearningCharacter.all_states[key];

                is_obstable = (result.getCharacterPosition().type == 1);
            }

            return result;
        }

        // Gets the available actions for the given state
        public static List<Action> getAvaliableActions(State state)
        {
            List<Action> result = new List<Action>();
            allActions(state);

            foreach (var action in availableActions)
            {
                if (action.canExecute(state))
                {
                    result.Add(action);
                }
            }

            return result;
        }

        // a pair consisting of the reward and the new state.
        public Pair takeAction(State state, Action action)
        {
            return action.execute(state);
        }

        /* ALL ACTION */
        private static void allActions(State state)
        {
            List<Action> result = new List<Action>();
            result.Add(new GoUp(state.getCharacterPosition()));
            result.Add(new GoLeft(state.getCharacterPosition()));
            result.Add(new GoDown(state.getCharacterPosition()));
            result.Add(new GoRight(state.getCharacterPosition()));

            result.Add(new GoSleep(state.getCharacterPosition()));
            result.Add(new GoEat(state.getCharacterPosition()));
            result.Add(new GoRest(state.getCharacterPosition()));

            availableActions = result;
        }
    }
}
