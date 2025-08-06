using System;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class PlateBehavior : MonoBehaviour
{

    public bool isBumbuIncluded = false;
    public bool isMeatIncluded = false;

    public ServingHelper servingHelper;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    private bool isDone = false;
    void Update()
    {
        if (servingHelper.IsAllIncluded() && !isDone)
        {
            servingHelper.ShowInformation();
            isDone = true;
        }

    }
}
