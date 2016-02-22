
using UnityEngine;

namespace Assets.Scripts.QLearning
{
    public class MidPoint
    {
        public int x_index { get; set; }
        public int z_index { get; set; }
        public int x_position { get; set; }
        public int z_position { get; set; }
        public int type { get; set; } // 0 none, 1 obstacle, 2 resource food, 3 resource bed

        public Vector3 position_vector()
        {
            return new Vector3(x_position, 0.5f, z_position);
        }
    
    }
}
