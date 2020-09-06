using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{
    [SerializeField] ShipScript ship;
    [SerializeField] Vector3 offset;

    private void Start()
    {
        StartCoroutine("ChangePosition");
    }
    IEnumerator ChangePosition()
    {
        while (1 == 1)
        {
            transform.position = ship.transform.position + offset;
            yield return new WaitForEndOfFrame();
        }
    }
}
