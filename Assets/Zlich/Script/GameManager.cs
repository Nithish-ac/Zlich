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
                if(!dice._isSelected)
                    dice.RotateDice();
            }
        }
        #region ClickOnDie
        public void ClickOnDie(GameObject die)
        {
            if (!_previousPositions.ContainsKey(die))
            {
                _previousPositions[die] = die.transform.position;
                MoveDieToPlaceholder(die);
                die.GetComponent<Dice>()._isSelected = true;
            }
            else
            {
                MoveDieToPreviousPosition(die);
                _previousPositions.Remove(die);
                die.GetComponent<Dice>()._isSelected = false;
            }
            CalculateScore();
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
        #endregion
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
            _uiManager.UpdateScore(totalScore);
        }
        private int GetDiceFaceValue(GameObject die)
        {
            return die.GetComponent<Dice>()._diceFaceNow;
        }
        private int CalculateIndividualScore(int faceValue)
        {
            switch (faceValue)
            {
                case 1:
                    return 100; // 1 = 100 points
                case 5:
                    return 50;  // 5 = 50 points
                default:
                    // Check for 3 of a kind
                    if (CheckForNOfAKind(faceValue, 3))
                    {
                        return faceValue * 100; // e.g., three 4s equal 400 points
                    }
                    // Check for 3 ones
                    else if (faceValue == 1 && CheckForNOfAKind(faceValue, 3))
                    {
                        return 1000; // 3 ones = 1,000 points
                    }
                    // Check for 4 ones
                    else if (faceValue == 1 && CheckForNOfAKind(faceValue, 4))
                    {
                        return 2000; // 4 ones = 2,000 points
                    }
                    // Check for 3 twos, 3 threes, 3 fours, 3 fives, 3 sixes
                    else if (CheckForNOfAKind(faceValue, 3))
                    {
                        return faceValue * 100; // e.g., three 2s equal 200 points
                    }
                    // Check for a straight (five dice in consecutive number order)
                    else if (CheckForStraight())
                    {
                        return 1750; // 1,750 points for a straight
                    }
                    // Check for 3 pairs
                    else if (CheckForThreePairs())
                    {
                        return 1000; // 3 pairs = 1,000 points
                    }
                    else
                    {
                        return 0; // No points for other cases
                    }
            }
        }
        private bool CheckForNOfAKind(int faceValue, int n)
        {
            int count = 0;

            foreach (GameObject placeholder in _placeHolder)
            {
                if (placeholder.transform.childCount > 0)
                {
                    GameObject die = placeholder.transform.GetChild(0).gameObject;
                    int dieFaceValue = GetDiceFaceValue(die);
                    if (dieFaceValue == faceValue)
                    {
                        count++;
                    }
                }
            }

            return count >= n;
        }

        private bool CheckForStraight()
        {
            List<int> uniqueFaceValues = new List<int>();

            foreach (GameObject placeholder in _placeHolder)
            {
                if (placeholder.transform.childCount > 0)
                {
                    GameObject die = placeholder.transform.GetChild(0).gameObject;
                    int dieFaceValue = GetDiceFaceValue(die);

                    if (!uniqueFaceValues.Contains(dieFaceValue))
                    {
                        uniqueFaceValues.Add(dieFaceValue);
                    }
                }
            }

            // Check if all numbers from 1 to 6 are present
            for (int i = 1; i <= 6; i++)
            {
                if (!uniqueFaceValues.Contains(i))
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckForThreePairs()
        {
            int pairCount = 0;
            List<int> uniqueFaceValues = new List<int>();

            foreach (GameObject placeholder in _placeHolder)
            {
                if (placeholder.transform.childCount > 0)
                {
                    GameObject die = placeholder.transform.GetChild(0).gameObject;
                    int dieFaceValue = GetDiceFaceValue(die);

                    if (!uniqueFaceValues.Contains(dieFaceValue))
                    {
                        uniqueFaceValues.Add(dieFaceValue);
                    }
                    else
                    {
                        pairCount++;
                        uniqueFaceValues.Remove(dieFaceValue);
                    }
                }
            }

            return pairCount == 3 && uniqueFaceValues.Count == 0;
        }
        private void ShowZilchPopup()
        {
            // Implement logic to show a popup indicating Zilch
            Debug.Log("Zilch!");
        }
    }
}

