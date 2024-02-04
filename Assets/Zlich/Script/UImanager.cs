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
        internal TMPro.TMP_Text[] _score;
        [SerializeField]
        internal TMPro.TMP_Text PlayerScore;
        private GameManager _gameManager;
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
        //public void UpdateScoreText(string diceTag)
        //{
        //    for (int i = 0; i < _gameManager._dice.Length; i++)
        //    {
        //        if (_gameManager._dice[i].gameObject.CompareTag(diceTag))
        //        {
        //            int diceValue = _gameManager._dice[i].GetDiceFaceScore(diceTag);
        //            _score[i].text = diceTag + " = " + diceValue.ToString();
        //            break;
        //        }
        //    }
        //}
        public void UpdateScore(int score)
        {
            PlayerScore.text = "Player Score :"+ score.ToString();
        }
    }
}
