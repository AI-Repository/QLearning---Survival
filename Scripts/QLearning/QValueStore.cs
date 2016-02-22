using System.Collections.Generic;
using System.IO;
using Assets.Scripts.GameManager;
using JsonFx.Json;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.QLearning
{
    class QValueStore
    {
        //1800 states
        //5 actions
        public float[,] Qmatrix;

        private int n_states;
        private int n_actions;

        private string path = @"q-matrix-files\q-learning.json";

        public QValueStore(int _n_states, int _n_actions)
        {

            n_states = _n_states;
            n_actions = _n_actions;

           Qmatrix = new float[n_states, n_actions];
            for (int i = 0; i < n_states; i++)
            {
                for (int j = 0; j < n_actions; j++)
                {
                    this.Qmatrix[i, j] = 0;
                    //Debug.Log(Qmatrix[i, j]);
                }
                
            }
        }

        public void printQValueStore()
        {
            for (int i = 0; i < n_states; i++)
            {
                for (int j = 0; j < n_actions; j++)
                {
                    Debug.Log(Qmatrix[i, j]);
                }

            }
        }

        public float getQValue(State state, Action action)
        {
            //ja sabemos estado
            return Qmatrix[state.getIndex(), action.getIndex()];
        }

        public Action getBestAction(State _state)
        {
            float bestQValue = float.MinValue;
            Action bestAction = null;

            //todas as açoes
            List<Action> actions = ReinforcementProblem.getAvaliableActions(_state);
            
            foreach (var action in actions)
            {
                int index_action = action.getIndex();
                int index_state = _state.getIndex();

                float Q = Qmatrix[index_state, index_action];
                if (bestQValue <= Q)
                {
                    if (bestQValue == Q)
                    {
                        Random rnd = new Random();
                        int random = rnd.Next(0, 2);


                        if (random == 0)
                        {
                            bestQValue = Q;
                            bestAction = action;
                        }
                    }
                    else
                    {
                        bestQValue = Q;
                        bestAction = action;
                    }
                  
                }
            }
            return bestAction;
        }

        public void storeQValue(State state, Action action, float value)
        {
           
           int index_action = action.getIndex();
           int index_state = state.getIndex();

           Qmatrix[index_state, index_action] = value;
        }

        private static T[,] Make2DArray<T>(T[] input, int height, int width)
        {
            T[,] output = new T[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    output[i, j] = input[i * width + j];
                }
            }
            return output;
        }

        public bool readMatrix()
        {
            if (File.Exists(path))
            {

                var streamReader1 = new StreamReader(path);
                string data2 = streamReader1.ReadToEnd();
                streamReader1.Close();

                float[] midpositions = JsonReader.Deserialize<float[]>(data2);

                Qmatrix = Make2DArray(midpositions, n_states, n_actions);

                return true;
            }
            else
            {
                return false;
            }
        }

        public void writeMatrix()
        {

            var q_learning_matrix = JsonWriter.Serialize(Qmatrix);

            var streamWriter = new StreamWriter(path);
            streamWriter.Write(q_learning_matrix);
            streamWriter.Close();

           

        }
    }
}
