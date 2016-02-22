
using Assets.Scripts.GameManager;
using UnityEngine;

namespace Assets.Scripts.QLearning.Actions
{
    public class GoSleep: Action
    {
        private int index = 4;

        private MidPoint _characterPosition;

        public GoSleep(MidPoint characterPosition)
        {
            this._characterPosition = characterPosition;
        }

        public Pair execute(State state)
        {

            //Debug.Log("SLEEP");


            //MUDAR HP, ENERGY
            //ja sabemos q podemos fazer
            int key = LearningCharacter.toKeyState(
                state.getHp() - 1, 
                LearningCharacter.max_energy, 
                state.getCharacterPosition().z_index,
                state.getCharacterPosition().x_index);

            if (!LearningCharacter.all_states.ContainsKey(key))
                Debug.Log("error key not found: hp: " + (state.getHp() - 1) + "\n" +
                          "mp: " + (LearningCharacter.max_energy) + "\n" +
                          "z: " + (state.getCharacterPosition().z_index) + "\n" +
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
            if (state.getHp() > 0)
            {
                Point new_point = new Point()
                {
                    x_coord = this._characterPosition.x_position,
                    z_coord = this._characterPosition.z_position
                };

                if (LearningCharacter.getObstacleType(new_point) == 2)
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
            return "GO SLEEP";
        }
    }
}
