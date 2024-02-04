using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zilch
{
    public class GameManager : MonoBehaviour
    {
        //classes
        public static GameManager Instance;
        private UImanager _uiManager;
        [SerializeField]
        internal Dice[] _dice;
        [SerializeField]
        internal List<GameObject> _placeHolder;
        private Dictionary<GameObject, Vector3> _previousPositions = new();
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
        public void ClickOnDie(GameObject die)
        {
            if (!_previousPositions.ContainsKey(die))
            {
                // If die is not in the dictionary, store its previous position
                _previousPositions[die] = die.transform.position;
                MoveDieToPlaceholder(die);
            }
            else
            {
                // If die is in the dictionary, move it back to the previous position
                MoveDieToPreviousPosition(die);
                _previousPositions.Remove(die); // Remove from dictionary after moving back
            }
        }

        private bool IsPlaceholderOccupied(GameObject placeholder)
        {
            return placeholder.transform.childCount > 0;
        }

        private void MoveDieToPlaceholder(GameObject die)
        {
            foreach (GameObject placeholder in _placeHolder)
            {
                if (!IsPlaceholderOccupied(placeholder))
                {
                    die.transform.SetParent(placeholder.transform);
                    die.transform.localPosition = Vector3.zero;
                    break;
                }
            }
        }

        private void MoveDieToPreviousPosition(GameObject die)
        {
            die.transform.SetParent(null); // Unparent the die
            die.transform.position = _previousPositions[die]; // Move back to the previous position
        }
        private void CalculateScore()
        {
            int totalScore = 0;

            foreach (GameObject placeholder in _placeHolder)
            {
                if (placeholder.transform.childCount > 0)
                {
                    GameObject die = placeholder.transform.GetChild(0).gameObject;
                    int faceValue = GetDiceFaceValue(die);
                    totalScore += CalculateIndividualScore(faceValue);
                }
            }
        }
        private int GetDiceFaceValue(GameObject die)
        {
            return die.GetComponent<Dice>()._diceFaceNow;
        }
    }
}

