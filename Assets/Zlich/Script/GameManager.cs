using System;
using System.Collections.Generic;
using System.Linq;
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
        private int _currentPlaceholderIndex = -1;
        internal int _currentPlayerIndex = 0;
        private int[] _scores;
        private int _totalScore;
        [SerializeField]
        private Button _rollBtn;
        [SerializeField]
        private Button _collectBtn;
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
            _scores = new int[2];
            _collectBtn.interactable = false;
        }
        private void Update()
        {
            if (_scores[_currentPlayerIndex] >= 300)
            {
                _collectBtn.interactable = true;
            }
            else
            {
                _collectBtn.interactable = true;
            }
        }
        public void RollDice()
        {
            _currentPlaceholderIndex++;
            foreach (Dice dice in _dice)
            {
                if(!dice._isSelected)
                    dice.RotateDice();
            }
            _rollBtn.interactable = false;
            Invoke(nameof(ShowZilchPopup), 3f);
        }
        public void CollectScore()
        {
            _totalScore += _scores[_currentPlayerIndex];
            _scores[_currentPlayerIndex] = 0;
            _uiManager.UpdateTotalScore(_totalScore);

            _uiManager.UpdateScore(0);
            ResetDice();
            _currentPlaceholderIndex = -1;
            SwitchToNextPlayer();
        }
        public void ResetDice()
        {
            foreach (Dice dice in _dice)
            {
                dice.ResetDice();
                dice._isSelected = false;
                dice._diceFaceNow = 0;
            }
            foreach (var key in _previousPositions.Keys)
            {
                MoveDieToPreviousPosition(key);
            }
            _previousPositions.Clear();
        }
        private void SwitchToNextPlayer()
        {
            _currentPlayerIndex = (_currentPlayerIndex + 1) % 2;

            _rollBtn.interactable = true;// Cycle between players
        }
        #region ClickOnDie
        public void ClickOnDie(GameObject die)
        {
            if (!_previousPositions.ContainsKey(die))
            {
                _previousPositions[die] = die.transform.position;
                MoveDieToPlaceholder(die);
                die.GetComponent<Dice>()._isSelected = true;
                CalculateScore(die,true);
            }
            else
            {
                MoveDieToPreviousPosition(die);
                _previousPositions.Remove(die);
                die.GetComponent<Dice>()._isSelected = false;
                CalculateScore(die, false);
            }
            
        }

        private void MoveDieToPlaceholder(GameObject die)
        {
            GameObject placeholder = _placeHolder[_currentPlaceholderIndex];
            Transform placeholderTransform = placeholder.transform;

            for (int i = 0; i < placeholderTransform.childCount; i++)
            {
                Transform child = placeholderTransform.GetChild(i);
                if (child.childCount == 0) // Check if the child has no children (i.e., empty)
                {
                    // Place the die as a child of the empty placeholder
                    die.transform.SetParent(child);
                    die.transform.localPosition = Vector3.zero;// Cycle through placeholders
                    return; // Exit the method once the die is placed
                }
            }
        }

        private void MoveDieToPreviousPosition(GameObject die)
        {
            die.transform.SetParent(null); // Unparent the die
            die.transform.position = _previousPositions[die]; // Move back to the previous position
        }
        #endregion
        #region Calculate Score
        private void CalculateScore(GameObject die,bool isAdded)
        {
            _rollBtn.interactable = true;
            int faceValue = GetDiceFaceValue(die);
            if(isAdded)
                _scores[_currentPlayerIndex] += CalculateIndividualScore(faceValue);
            else
                _scores[_currentPlayerIndex] -= CalculateIndividualScore(faceValue);
            _uiManager.UpdateScore(_scores[_currentPlayerIndex]);
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
                    if (CheckForNOfAKind(faceValue, 3))
                    {
                        return 1000;
                    }
                    if(CheckForNOfAKind(faceValue, 4))
                    {
                        return 2000;
                    }
                    return 100; // 1 = 100 points
                case 5:
                    if (CheckForNOfAKind(faceValue, 3))
                    {
                        return faceValue * 100;
                    }
                    return 50;  // 5 = 50 points
                default:
                    // Check for 3 of a kind
                    if (CheckForNOfAKind(faceValue, 3))
                    {
                        return faceValue * 100;
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
            Transform placeholder = _placeHolder[_currentPlaceholderIndex].transform;
            for (int i = 0; i < placeholder.childCount; i++)
            {
                Transform child = placeholder.GetChild(i);
                if (child.childCount > 0)
                { 
                    GameObject die = child.transform.GetChild(0).gameObject;
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

            Transform placeholder = _placeHolder[_currentPlaceholderIndex].transform;
            for (int i = 0; i < placeholder.childCount; i++)
            {
                Transform child = placeholder.GetChild(i);
                if (child.childCount > 0)
                {
                    GameObject die = child.transform.GetChild(0).gameObject;
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

            Transform placeholder = _placeHolder[_currentPlaceholderIndex].transform;
            for (int i = 0; i < placeholder.childCount; i++)
            {
                Transform child = placeholder.GetChild(i);
                if (child.childCount > 0)
                {
                    GameObject die = child.transform.GetChild(0).gameObject;
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
        #endregion
        private void ShowZilchPopup()
        {
            if (!CanPlaceDice())
            {
                Debug.Log("Zilch!");
                _uiManager._popUp.SetActive(true);
                _scores[_currentPlayerIndex] = 0;
                _uiManager.UpdateScore(0);
                ResetDice();
                _currentPlaceholderIndex = -1;
                SwitchToNextPlayer();

                Invoke("HideZilchPopup", 2f);
            }
        }
        private void HideZilchPopup()
        {
            _uiManager._popUp.SetActive(false);
        }
        public bool CanPlaceDice()
        {
            int[] faceCounts = new int[7];
            foreach (var die in _dice)
            {
                if(!die._isSelected)
                    faceCounts[die._diceFaceNow]++;
            }
            for (int i = 1; i < faceCounts.Length; i++)
            {
                Debug.Log("Face " + i + ": " + faceCounts[i]);
            }
            bool hasPossibility = true;

            if (faceCounts[1] == 0 && faceCounts[5] == 0)
            {
                hasPossibility = false;
            }

            for (int i = 1; i <= 6; i++)
            {
                if (faceCounts[i] >= 3)
                {
                    hasPossibility = true;
                    break;
                }
            }

            for (int i = 1; i <= 6; i++)
            {
                if (faceCounts[i] >= 3 && i != 1 && i != 5) 
                {
                    hasPossibility = true;
                    break;
                }
            }

            // Check for a straight
            if (faceCounts.Skip(1).Take(5).All(count => count >= 1))
            {
                hasPossibility = true;
            }

            // Check for three pairs
            if (faceCounts.Count(count => count >= 2) >= 3)
            {
                hasPossibility = true;
            }

            return hasPossibility;
        }
    }
}

