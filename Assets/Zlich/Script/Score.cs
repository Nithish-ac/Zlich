using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score Instance;
    [SerializeField]
    internal Dice[] _dice;
    [SerializeField]
    internal TMPro.TMP_Text[] _score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void UpdateScoreText(string diceTag)
    {
        for (int i = 0; i < _dice.Length; i++)
        {
            if (_dice[i].gameObject.CompareTag(diceTag))
            {
                int diceValue = _dice[i].GetDiceFaceScore(diceTag);
                _score[i].text = diceTag+" = "+ diceValue.ToString();
                break;
            }
        }
    }
}
