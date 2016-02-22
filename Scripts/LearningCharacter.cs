using System;
using System.Collections.Generic;
using Assets.Scripts.GameManager;
using Assets.Scripts.QLearning;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;


namespace Assets.Scripts
{
    public class LearningCharacter : MonoBehaviour
    {
        //where all states will be saved
        public static Dictionary<int, State> all_states;

        //state properties, used to generate each one
        public static int max_hp = 9; // [0-10]
        public static int max_energy = 5; // [0-5]
        private int n_positions = 10; // map 10x10

        // Q-Learning properties
        private int n_states;
        private int n_actions = 7;
        private ReinforcementProblem problem;
        public bool learning = true;
        private int iteractions = 25; // iteractions the character must survive
        private int tries = 100;
        
        // Variables realted to GUI
        private Text hp_text_object;
        private Text energy_text_object;
        private Text actions_text_object;


        // Varaibles to move character
        private Boolean is_moving = false;
        private int seconds_per_action = 5;
        private float inc; // increment's
        private Vector3 dir; // direction

        private int current_try = 0;
        private int current_iteraction = 0;

        private State current_state;
        private bool new_try = true;

        // all gameobjects that were obstacles (possible tags: Boar, Obstacle and Bed)
        public static List<GameObject> obstacles { get; set; }

        // this will keep all mid positions, including indexes and positions
        MidPoint[,] positions { get; set; }

        // Holds the store for Q-values, we use this to make decisions based on the learning
        private QValueStore store;
        
        private void Start()
        {
            // get all obstacles and save them into obstacles
            getBadPositions(); 

            // get all mid positions
            positions = new MidPoint[10, 10]; 
            calculate_mid_positions(positions);

            // save all mid positions to a file
            
            //var data_positions = JsonWriter.Serialize(positions);
            //var streamWriter = new StreamWriter(@"D:\midpositions.json");
            //streamWriter.Write(data_positions);
            //streamWriter.Close();

            // generate all possible states
            createStates();
            
            // initialize q-learning matrix
            store = new QValueStore(n_states, n_actions);

            // read q-learning matrix
            store.readMatrix();

            // initialize problem
            problem = new ReinforcementProblem();

            //store.printQValueStore();

            // one iteration test
            //QLearning(problem, 1, 1, 0, 0, 0);

             hp_text_object = GameObject.Find("HP").GetComponent<Text>();
             energy_text_object = GameObject.Find("Energy").GetComponent<Text>();
             actions_text_object = GameObject.Find("Actions").GetComponent<Text>();
            
        }

        public void createStates()
        {
            //all_states = new List<State>();
            all_states = new Dictionary<int, State>();
            State state;
            int index = 0;

            for (int i = 0; i <= max_hp; i++) // HP [0,1,2]
            {
                for (int j = 0; j <= max_energy; j++) // ENERGY [0,1,2]
                {
                    for (int k = 0; k < n_positions; k++) // POSITION X [10]
                    {
                        for (int l = 0; l < n_positions; l++) // POSITION Z [10]
                        {
                                state = new State();
                                state.setHp(i);
                                state.setEnergy(j);
                                state.setCharacterPosition(positions[k, l]);
                                state.setIndex(index);

                                //Debug.Log("State: " + state.getObjective() + " " + state.getCharacterPosition().z_index + " " + state.getCharacterPosition().x_index + " " + state.getEnergy() + " " + state.getHp());

                                int hash_index =  toKeyState(i, j, state.getCharacterPosition().z_index,
                                state.getCharacterPosition().x_index);

                                all_states.Add(hash_index, state);
                                index++;
                        }
                    }
                }
            }

           n_states = all_states.Count;
        }

        //converts a state to index number
        public static int toKeyState(int hp, int energy, int zz, int xx)
        {
            return  xx * 1 + zz*10 + energy*100 + hp*1000;
        }

        // save each obstacle and it's mid position inside list "obstacles"
        public void getBadPositions()
        {
            obstacles = new List<GameObject>();

            foreach (var obs in GameObject.FindGameObjectsWithTag("Obstacle"))
            {
                obstacles.Add(obs);
            }

            foreach (var boar in GameObject.FindGameObjectsWithTag("Boar"))
            {
                obstacles.Add(boar);
            }

            foreach (var bed in GameObject.FindGameObjectsWithTag("Bed"))
            {
                obstacles.Add(bed);
            }
        }

        // dado um ponto verifica se existe ou não na lista de points "bad positions"
        public static int getObstacleType(Point a)
        {
            int result = 0;

            foreach (GameObject t in obstacles)
            {
                if ((int)t.transform.position.x == a.x_coord &&
                    (int)t.transform.position.z == a.z_coord)
                {
                    if (t.CompareTag("Obstacle"))
                    {
                        result = 1;
                    }
                    else if (t.CompareTag("Bed"))
                    {
                        result = 2;
                    }
                    else if (t.CompareTag("Boar"))
                    {
                        result = 3;
                    }
                }
            }

            return result;
        }

        // calculate all mid positions, using the structure of the class MidPoint
        public MidPoint[,] calculate_mid_positions(MidPoint[,] pos)
        {
            int zStart = 50; // z-axis beginning
            int xStart = -50; // x-axis beginning

            // world representation of 10 x 10 areas 
            for (int z = 0; z < 10; z++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Point checking_mid_point = new Point { x_coord = xStart + 5, z_coord = zStart - 5 };

                    MidPoint new_mid_point = new MidPoint
                    {
                        x_index = x,
                        z_index = z,
                        x_position = (xStart + 5),
                        z_position = (zStart - 5),
                        type = getObstacleType(checking_mid_point)
                    };

                    pos[x, z] = new_mid_point;

                    xStart = xStart + 10;
                }

                xStart = -50;
                zStart = zStart - 10;
            }

            return pos;
        }

        // Updates the store by investigating the problem
        // nu [0..99]
        public void QLearning(ReinforcementProblem problem, float alpha, float gamma, float rho, float nu)
        {
           
            // Pick a new state every once in a while
            Random rnd = new Random();

            float random = (float) rnd.NextDouble();

            if (random < nu)
            {
                current_state = problem.getRandomState();
            }

            List<Action> avaliableActions = ReinforcementProblem.getAvaliableActions(current_state);

            int numberofactions = rnd.Next(0, avaliableActions.Count);

            Action bestAction;

            random = (float)rnd.NextDouble();

            if (random < rho)
            {
                bestAction = avaliableActions[numberofactions];
            }
            else
            {
                bestAction = store.getBestAction(current_state);
            }

            Pair par = problem.takeAction(current_state, bestAction);

            // Get the current q from the store
            float Q = store.getQValue(current_state, bestAction);

            // Get the q of the best action from the new state
            float maxQ = store.getQValue(par.new_state, store.getBestAction(par.new_state));

            // Perform the q learning
            Q = (1 - alpha) * Q + alpha * (par.reward + gamma * maxQ);

            // Store the new Q-value
            store.storeQValue(current_state, bestAction, Q);

            // And update the state
            current_state = par.new_state;

        }

        private void Learn()
        {
            if (current_try < tries)
            {
                if (new_try)
                {
                    current_try++;
                    current_state = problem.getRandomState();
                    new_try = false;
                    //Debug.Log("Try number " + current_try);
                }
                if (current_iteraction < iteractions)
                {
                    //                 alpha,gamma, rho,  nu
                    QLearning(problem, 0.7f, 0.75f, 0.2f, 0.1f);
                    current_iteraction++;
                    // if is dead restarts
                    if (current_state.getHp() == 0)
                    {
                        Debug.Log("restarts.... (dies)");
                        current_iteraction = 0;
                        new_try = true;
                    }
                }
                else
                {
                    Debug.Log("Success!!!!");
                    current_iteraction = 0;
                    new_try = true;
                }
            }
            else
            {
                current_try = 0;

                //Debug.Log("End and save");
                //save = true;
                store.writeMatrix();
                Debug.Log("Saved");
            }
        }

        private void Play()
        {
            if (current_iteraction < iteractions)
            {
                if (current_iteraction == 0)
                {
                    current_state = problem.getRandomState();

                    int key = LearningCharacter.toKeyState(
                         max_hp,
                         max_energy,
                         current_state.getCharacterPosition().z_index,
                         current_state.getCharacterPosition().x_index);

                    current_state = all_states[key];

                    this.gameObject.transform.position = current_state.getCharacterPosition().position_vector();

                    hp_text_object.text = "" + max_hp;
                    energy_text_object.text = "" + max_energy;

                }

                if (!is_moving)
                {

                    Action bestAction = store.getBestAction(current_state);

                    Pair new_move = bestAction.execute(current_state);

                    float distance = Vector3.Distance(current_state.getCharacterPosition().position_vector(), new_move.new_state.getCharacterPosition().position_vector());

                    inc = distance / seconds_per_action;

                    dir = (new_move.new_state.getCharacterPosition().position_vector() -
                                        current_state.getCharacterPosition().position_vector());

                    current_state = new_move.new_state;

                    is_moving = true;

                    current_iteraction++;

                    hp_text_object.text = "" + current_state.getHp();
                    energy_text_object.text = "" + current_state.getEnergy();

                    actions_text_object.text += bestAction.ToString() + ", ";

                    Debug.Log("AÇÃO --- " + bestAction.ToString());

                }
                else
                {
                    andar(dir / inc);

                }

                if (current_state.getHp() == 0)
                {
                    Debug.Log("MORREU HP = 0");
                    current_iteraction = iteractions;
                }
            }

          
          

          

            

        }

        private void andar(Vector3 dir)
        {
            if (Vector3.Distance(current_state.getCharacterPosition().position_vector(), this.gameObject.transform.position) < 0.5f)
            {
                this.gameObject.transform.position = current_state.getCharacterPosition().position_vector();
                is_moving = false;

            }else{

                this.gameObject.transform.position += (dir * Time.deltaTime); 
            }
        }

        // Update is called once per frame
        private void Update()
        {

            if (learning)
            {
                Learn();
            }
            else
            {
                Play();
            }
        }
    }
}