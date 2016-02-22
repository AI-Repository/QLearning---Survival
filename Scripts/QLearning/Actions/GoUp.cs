using Assets.Scripts.GameManager;
using UnityEngine;

namespace Assets.Scripts.QLearning.Actions
{
    class GoUp : Action
    {
        private int index = 0;

        public MidPoint _characterPosition;

        public GoUp(MidPoint characterPosition)
        {
            this._characterPosition = characterPosition;
        }

        public Pair execute(State state)
        {

            //Debug.Log("UP");


            //ja sabemos q podemos fazer
            int key = LearningCharacter.toKeyState(
                state.getHp() - 1, 
                state.getEnergy() - 1, 
                state.getCharacterPosition().z_index-1,
                state.getCharacterPosition().x_index);

            if (!LearningCharacter.all_states.ContainsKey(key))
                Debug.Log("error key not found: hp: " + (state.getHp() - 1) + "\n" +
                          "mp: " + (state.getEnergy() - 1) + "\n" +
                          "z: " + (state.getCharacterPosition().z_index - 1) + "\n" +
                          "x: " + (state.getCharacterPosition().x_index));

            //new state
            State new_state = LearningCharacter.all_states[key];

            Pair pair = new Pair();
            pair.setNewState(new_state);
            pair.setReward(calculateReward(state));

            return pair;
        }

        public bool canExecute(State state)
        {
            if (this._characterPosition.z_index != 0 && state.getEnergy() > 0 && state.getHp() > 0)
            {
                Point new_point = new Point()
                {
                    x_coord = this._characterPosition.x_position,
                    z_coord = this._characterPosition.z_position + 10
                };

                if (LearningCharacter.getObstacleType(new_point) != 1)
                {
                    return true;
                }

            }

            return false;
        }

        public int getIndex()
        {
            return this.index;
        }

        private float calculateReward(State state)
        {
            return 0;
        }

        public string ToString()
        {
            return "GO UP";
        }
    }
}
