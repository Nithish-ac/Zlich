using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zilch
{
    public class UImanager : MonoBehaviour
    {
        public static UImanager Instance;
        [SerializeField]
        internal TMPro.TMP_Text _currentScore;
        [SerializeField]
        internal TMPro.TMP_Text[] _totalScore;
        private GameManager _gameManager;
        [SerializeField]
        internal GameObject _popUp;
        [SerializeField]
        internal GameObject _playersTurn;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        private void Start()
        {
            _gameManager = GameManager.Instance;
        }
        public void UpdateScore(int score)
        {
            _currentScore.text = "Current Score: "+ score.ToString();
        }
        public void UpdateTotalScore(int totalScore)
        {
            _totalScore[_gameManager._currentPlayerIndex].text = totalScore.ToString();
        }
    }
}
