using System.Collections.Generic;
using UnityEngine;

namespace Pools
{
    public class TurtlePool : ObjectPool
    {
        public static TurtlePool instance;

        // Start is called before the first frame update
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        private void Start()
        {
            AddObjects(amountToPool);
        }
    }
}