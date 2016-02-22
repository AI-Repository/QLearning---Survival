using Assets.Scripts.GameManager;
using UnityEngine;

namespace Assets.Scripts.QLearning.Actions
{
    class GoLeft : Action
    {
        private int index = 1;

        private MidPoint _characterPosition;
        public GoLeft(MidPoint characterPosition)
        {
            this._characterPosition = characterPosition;
        }

        public Pair execute(State state)
        {
            //Debug.Log("LEFT");


            //ja sabemos q podemos fazer
            int key = LearningCharacter.toKeyState(
                state.getHp() - 1, 
                state.getEnergy() - 1, 
                state.getCharacterPosition().z_index,
                state.getCharacterPosition().x_index-1);

            if (!LearningCharacter.all_states.ContainsKey(key))
                Debug.Log("error key not found: hp: " + (state.getHp() - 1) + "\n" +
                          "mp: " + (state.getEnergy() - 1) + "\n" +
                          "z: " + (state.getCharacterPosition().z_index) + "\n" +
                          "x: " + (state.getCharacterPosition().x_index-1));

            //new state
            State new_state = LearningCharacter.all_states[key];

            Pair pair = new Pair();
            pair.setNewState(new_state);
            pair.setReward(calculateReward((state)));

            return pair;
        }

        public bool canExecute(State state)
        {
            if (this._characterPosition.x_index != 0 && state.getEnergy() > 0 && state.getHp() > 0)
            {
                Point new_point = new Point()
                {
                    x_coord = this._characterPosition.x_position - 10,
                    z_coord = this._characterPosition.z_position
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
            return "GO LEFT";
        }
    }
}
