using System.Collections.Generic;
using UnityEngine;

namespace Pools
{
    public class DragonPool : ObjectPool
    {
        public static DragonPool instance;

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