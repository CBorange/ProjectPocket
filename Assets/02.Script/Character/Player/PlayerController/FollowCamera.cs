using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SphericalCoordinate
{
    public float radius, azimuth, elevation;

    private float minAzimuth = 0f;
    private float maxAzimuth = 360f;

    private float minElevation = 0f;
    private float maxElevation = 90f;

    public float Radius 
    {
        get { return radius; }
        set { radius = value; }
    }
    public float Azimuth
    {
        get { return azimuth; }
        set
        {
            azimuth = Mathf.Repeat(value, maxAzimuth - minAzimuth);
        }
    }
    public float Elevation
    {
        get { return elevation; }
        set
        {
            elevation = Mathf.Clamp(value, minElevation, maxElevation);
        }
    }

    public SphericalCoordinate(Vector3 initializePos) 
    {
        minAzimuth = Mathf.Deg2Rad * minAzimuth;
        maxAzimuth = Mathf.Deg2Rad * maxAzimuth;

        minElevation = Mathf.Deg2Rad * minElevation;
        maxElevation = Mathf.Deg2Rad * maxElevation;

        Radius = 4f;
        Azimuth = Mathf.Atan2(initializePos.z, initializePos.x);
        Elevation = Mathf.Asin(initializePos.y / Radius);
    }

    public Vector3 GetCartesianCoord()
    {
        float t = Radius * Mathf.Cos(Elevation);
        return new Vector3(t * Mathf.Cos(Azimuth), Radius * Mathf.Sin(Elevation), t * Mathf.Sin(Azimuth));
    }
    public void Rotate(float newAzimuth, float newElevation)
    {
        Azimuth += newAzimuth;
        Elevation += newElevation;
    }
}
public class FollowCamera : MonoBehaviour
{
    private readonly float horRotateSpped = 4f;
    private readonly float verRotateSpped = 3f;
    public Transform targetToFollow;
    public SphericalCoordinate sphericalCoordinate;

    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);

        sphericalCoordinate = new SphericalCoordinate(transform.position);
        transform.position = sphericalCoordinate.GetCartesianCoord() + targetToFollow.position;
    }
    public void MoveCamera(float horMove, float verMove)
    {
        sphericalCoordinate.Rotate(horMove * horRotateSpped * Time.deltaTime, verMove * verRotateSpped * Time.deltaTime);
        transform.position = sphericalCoordinate.GetCartesianCoord() + targetToFollow.position;

        transform.LookAt(targetToFollow.position);
    }
}
