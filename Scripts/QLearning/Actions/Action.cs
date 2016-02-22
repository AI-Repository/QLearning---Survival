
using Assets.Scripts.GameManager;
using Assets.Scripts.QLearning;

namespace Assets.Scripts
{
 
        public interface Action
        {
            Pair execute(State state);
            int getIndex();
            bool canExecute(State state);
            string ToString();
        }
    
}
