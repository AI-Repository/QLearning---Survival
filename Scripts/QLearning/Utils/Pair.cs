using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.GameManager;

namespace Assets.Scripts.QLearning
{
    public class Pair
    {
        public float reward { get; set; }

        public State new_state { get; set; }

        public Pair() {}

        //SET
        public void setReward(float _reward)
        {
            this.reward = _reward;
        }

        public void setNewState(State _state)
        {
            this.new_state = _state;
        }

        //GET
        public float getReward()
        {
            return this.reward;
        }

        public State getNewState()
        {
            return this.new_state;
        }
    }
}
