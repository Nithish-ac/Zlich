using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        //classes
        public static GameManager Instance;
        private UImanager _uiManager;
        [SerializeField]
        internal Dice[] _dice;
        [SerializeField]
        private List<DiceSprite> _diceSprites;
        private List<(GameObject, int)> _disabledDice;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        private void Start()
        {
            _uiManager = UImanager.Instance;
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
            obj.SetActive(false);
            Sprite dieImage = null;

            foreach (DiceSprite diceSprite in _diceSprites)
            {
                if (diceSprite.intValue == number)
                {
                    dieImage = diceSprite.spriteValue;
                    break;
                }
            }
            int index = _uiManager._selectedDieSprites.FindIndex(image => image.sprite == null);

            if (index != -1)
            {
                _uiManager._selectedDieSprites[index].sprite = dieImage;
                _disabledDice.Add((obj, number));
            }
        }
        public void ClickOnDiceSprite(int dieNumber)
        {
            var disabledDie = _disabledDice.Find(die => die.Item2 == dieNumber);

            if (disabledDie.Item1 != null)
            {
                disabledDie.Item1.SetActive(true);

                _disabledDice.Remove(disabledDie);
            }
        }
    }
}

