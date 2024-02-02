using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FaceDetector : MonoBehaviour
{
    [SerializeField]
    internal Dice[] dice;

    private void OnTriggerStay(Collider other)
    {
        //if (dice != null)
        //{
        //    bool allDiceZeroVelocity = dice.All(singleDice => singleDice.GetComponent<Rigidbody>().IsSleeping() == true);
        //    if (allDiceZeroVelocity)
        //    {
        //        Debug.Log("Dice is stoped");    
        //        switch (other.gameObject.tag)
        //        {
        //            case "D1":
        //                dice[0]._diceFaceNow = int.Parse(other.name);
        //                break;
        //            case "D2":
        //                dice[1]._diceFaceNow = int.Parse(other.name);
        //                break;
        //            case "D3":
        //                dice[2]._diceFaceNow = int.Parse(other.name);
        //                break;
        //            case "D4":
        //                dice[3]._diceFaceNow = int.Parse(other.name);
        //                break;
        //            case "D5":
        //                dice[4]._diceFaceNow = int.Parse(other.name);
        //                break;
        //            case "D6":
        //                dice[5]._diceFaceNow = int.Parse(other.name);
        //                break;
        //                // Add additional cases for other dice faces if needed
        //        }
        //    }
        //}
    }
}
