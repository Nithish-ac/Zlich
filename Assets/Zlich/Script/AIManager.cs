using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zilch
{
    public class AIManager : MonoBehaviour
    {
        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = GameManager.Instance;
        }
        public void MakeMove()
        {
            // Implement AI logic to make a move here
            // You can define different strategies or rules for the AI to follow
            
            // For example, you can randomly select dice to roll or collect
            
            // For demonstration purposes, let's randomly select a die to roll
            if (_gameManager.CanPlaceDice())
            {
                // Select a random die index from the available dice
                int randomDieIndex = Random.Range(0, _gameManager._dice.Length);
                
                // Call the ClickOnDie method in the GameManager to simulate clicking on the selected die
                _gameManager.ClickOnDie(_gameManager._dice[randomDieIndex].gameObject);
            }
            else
            {
                // If no moves are possible, end the turn
                EndTurn();
            }
        }

        private void EndTurn()
        {
            // End the turn here (if needed)
            // This method can be extended to perform additional actions at the end of the AI's turn
        }
    }
}
