using Assets.Scripts.QLearning;

namespace Assets.Scripts.GameManager
{
    public class State
    {
        private int hp { get; set; } // [0,1,2]
        private int energy { get; set; } // [0,1,2]

        private MidPoint characterPosition { get; set; } // [10x10], Objeto da classe MidPoint

        private int index { get; set; } //index [0, num_state]

        public State() { }

        /*
        SETS
        */
        public void setHp(int _hp)
        {
            this.hp = _hp;
        }

        public void setEnergy(int _energy)
        {
            this.energy = _energy;
        }

        public void setCharacterPosition(MidPoint _characterPosition)
        {
            this.characterPosition = _characterPosition;
        }


        public void setIndex(int _index)
        {
            this.index = _index;
        }

        /*
        GETS
        */
        public int getHp()
        {
            return this.hp;
        }

        public int getEnergy()
        {
            return this.energy;
        }

        public MidPoint getCharacterPosition()
        {
            return this.characterPosition;
        }


        public int getIndex()
        {
            return this.index;
        }
    }
}
