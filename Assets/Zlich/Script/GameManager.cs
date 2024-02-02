using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zilch
{
    [System.Serializable]
    public class DiceSprite
    {
        public int intValue;
        public Sprite spriteValue;
    }
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        [SerializeField]
        internal Dice[] _dice;
        public DiceSprite[] intSpriteArray;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        public void RollDice()
        {
            foreach (Dice dice in _dice)
            {
                dice.RotateDice();
            }
        }
        public void ClickOnDie(GameObject obj, int number)
        {

        }
    }
}

