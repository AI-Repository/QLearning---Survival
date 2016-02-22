using Assets.Scripts.GameManager;
using System;
using UnityEngine;

namespace Assets.Scripts.QLearning.Actions
{
    class GoRest : Action
    {
        private int index = 6;

        private MidPoint _characterPosition;
        public GoRest(MidPoint characterPosition)
        {
            this._characterPosition = characterPosition;
        }

        public Pair execute(State state)
        {

            //Debug.Log("REST");
    

            //MUDAR HP, ENERGY
            //ja sabemos q podemos fazer
            int key = LearningCharacter.toKeyState(
                 Math.Max(state.getHp() - 1, 0),
                Math.Min(LearningCharacter.max_energy, state.getEnergy()+2),
                state.getCharacterPosition().z_index,
                state.getCharacterPosition().x_index);

            if (!LearningCharacter.all_states.ContainsKey(key))
                Debug.Log("error key not found: hp: " + Math.Max(state.getHp() - 1, 0) + "\n" +
                          "mp: " + (Math.Min(LearningCharacter.max_energy, state.getEnergy() + 2)) + "\n" +
                          "z: " + (state.getCharacterPosition().z_index) + "\n" +
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
            return true;
        }

        public int getIndex()
        {
            return this.index;
        }

        private float calculateReward(State state)
        {
            if (state.getHp() == 0)
                return -10;

            return 0;
        }

        public string ToString()
        {
            return "GO REST";
        }
    }
}
