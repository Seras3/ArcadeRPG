using System;
using System.Linq;
using UnityEngine;


namespace Stats
{
    public class PlayerStats : CharacterStats
    {
        public Weapon weapon = new Weapon("StormBreaker", 25);

        

        //private List<Weapon> ownedWeapons = new List<Weapon>();

        public override void Die()
        {
            // freeze all characters
            EnemyController.FreezeAll();
            GetComponent<HumanoidMovementPlayer>().enabled = false;
            GetComponent<PlayerShooting>().enabled = false;
            // fade-in 'you died' canvas
            var gameOverCanvas = Resources.FindObjectsOfTypeAll<Canvas>().First(obj => obj.gameObject.transform.name == "GameOverCanvas").gameObject;
            gameOverCanvas.SetActive(true);
            gameOverCanvas.GetComponent<CanvasFader>().Fade();

            Debug.Log(transform.name + " died.");
        }
    }
}