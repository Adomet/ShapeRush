using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : MonoBehaviour
{

    public ShapeType ShapeType;
    public GameObject PointCubeCluster,PointTriangleCluster,PointPentagonCluster;

    public GameObject MyPointCluster;
    public int numberOfPointClusters;
    public float pointClusterRotateAngle;
    
    public float distanceBetweenPoints;

    private void Awake()
    {
        switch (ShapeType)
        {
            case ShapeType.Cube:
                MyPointCluster = PointCubeCluster;
                break;
            case ShapeType.Triangle:
                MyPointCluster = PointTriangleCluster;
                break;
            case ShapeType.Pentagon:
                MyPointCluster = PointPentagonCluster;
                break;
            default:
                MyPointCluster = PointCubeCluster;
                break;

        }




        float tempangle = 0;
        float tempdiff = 0;
        for (int i = 0; i < numberOfPointClusters; i++)
        {
            Instantiate(MyPointCluster, transform.position + new Vector3(tempdiff,0f,0f),Quaternion.Euler(new Vector3(tempangle,0f,0f)),transform);
            tempangle += pointClusterRotateAngle;
            tempdiff += distanceBetweenPoints;
        }
    }
}
