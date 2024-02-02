using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    Rigidbody body;
    [SerializeField]
    private float _maxRandomForceValue, _startRollingForce;
    private float _forceX, _forceY, _forceZ;
    internal int _diceFaceNow;
    private bool _isRolling;
    private void Awake()
    {
        Initialize();
    }
    private void Initialize()
    {
        body = GetComponent<Rigidbody>();
        body.isKinematic = true;
        transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
    }
    private void Update()
    {
        if (body != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RotateDice();
            }
            if (_isRolling && body.IsSleeping() == true)
            {
                _isRolling = false;
                CheckRoll();
            }
        }
    }
    public int GetDiceFaceScore(string tag)
    {
        if(gameObject.tag == tag && !_isRolling)
        {
            return _diceFaceNow;
        }
        return 0;
    }
    private void RotateDice()
    {
        _isRolling = true;
        body.isKinematic = false;

        _forceX = Random.Range(0f, _maxRandomForceValue);
        _forceY = Random.Range(0f, _maxRandomForceValue);
        _forceZ = Random.Range(0f, _maxRandomForceValue);

        body.AddForce(Vector3.back * _startRollingForce);
        body.AddTorque(new Vector3(_forceX, _forceY, _forceZ));
    }
    public void CheckRoll()
    {
        float yDot, xDot, zDot;
        int rollvalue = -1;
        yDot = Mathf.Round(Vector3.Dot(transform.up.normalized, Vector3.up));
        zDot = Mathf.Round(Vector3.Dot(transform.forward.normalized, Vector3.up));
        xDot = Mathf.Round(Vector3.Dot(transform.right.normalized, Vector3.up));

        switch (yDot)
        {
            case 1:
                rollvalue = 2;
                break;
            case -1:
                rollvalue = 5;
                break;
        }
        switch (xDot)
        {
            case 1:
                rollvalue = 4;
                break;
            case -1:
                rollvalue = 3;
                break;
        }
        switch (zDot)
        {
            case 1:
                rollvalue = 1;
                break;
            case -1:
                rollvalue = 6;
                break;
        }
        _diceFaceNow = rollvalue;
        Score.Instance.UpdateScoreText(gameObject.tag);
        Debug.Log("Score Updated = " + gameObject.tag + " = "+_diceFaceNow);
    }
}