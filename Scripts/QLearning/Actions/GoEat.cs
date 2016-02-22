using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.GameManager;
using UnityEngine;

namespace Assets.Scripts.QLearning.Actions
{
    public class GoEat : Action
    {
        private int index = 5;

        private MidPoint _characterPosition;
        public GoEat(MidPoint characterPosition)
        {
            this._characterPosition = characterPosition;
        }

        public Pair execute(State state)
        {

            //Debug.Log("EAT");

            //MUDAR HP, ENERGY
            //ja sabemos q podemos fazer
            int key = LearningCharacter.toKeyState(
                LearningCharacter.max_hp, 
                state.getEnergy()-1, 
                state.getCharacterPosition().z_index,
                state.getCharacterPosition().x_index);

            if (!LearningCharacter.all_states.ContainsKey(key))
                Debug.Log("error key not found: hp: " + (LearningCharacter.max_hp ) + "\n" +
                          "mp: " +  (state.getEnergy()-1) + "\n" +
                          "z: " + state.getCharacterPosition().z_index + "\n" +
                          "x: " + state.getCharacterPosition().x_index);

            //new state
            State new_state = LearningCharacter.all_states[key];

            Pair pair = new Pair();
            pair.setNewState(new_state);
            pair.setReward(calculateReward(state));

            return pair;
        }

        public bool canExecute(State state)
        {
            Point new_point = new Point()
            {
                x_coord = this._characterPosition.x_position,
                z_coord = this._characterPosition.z_position
            };

            if (LearningCharacter.getObstacleType(new_point) == 3 && state.getEnergy() > 0)
            {
                return true;
            }

            return false;
        }

        private float calculateReward(State state)
        {
            return 1;
        }

        public int getIndex()
        {
            return this.index;
        }

        public string ToString()
        {
            return "GO EAT";
        }

    }
}
