using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float speed = 100f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] bool moveInLine = false;
    [SerializeField] List<Transform> waypoints;
    List<Vector3> bezierWaypointsPosition;
    Vector3 gizmoPos;
    Vector3 nextPos;
    int currentWaypoint = 0;
    int currentBezierWaypoint = 0;
    float wpRadius = 0.01f;
    bool coroutineActive = false;
    bool moveInCurveLoopEnded = false;

    // for OnGizmoDraw to work outside runtime we make waypoints a SerializedField
    // void Awake()
    // {
    //     waypoints = new List<Transform>();
    // }

    // Start is called before the first frame update
    void Start()
    {
        //waypoints = new List<Transform>();
        bezierWaypointsPosition = new List<Vector3>();
        //GetChildWaypoints();
        GetBezierWaypoints();
        if (bezierWaypointsPosition.ToArray().Length > 0)
        {
            target.transform.position = bezierWaypointsPosition[0];
        }
        else
        {
            target.transform.position = waypoints[0].position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (moveInLine)
        {
            MoveInLine();
        }
        else
        {
            MoveInCurve();
        }
        //if (!coroutineActive) StartCoroutine(MoveInCurve());
    }

    void OnDrawGizmos()
    {
        int currentindex = 0;
        if (waypoints == null) return;
        for (int i = 0; i < waypoints.ToArray().Length; i++)
        {
            for (float t = 0; t <= 1; t += 0.01f)
            {
                gizmoPos = CalculateCubicBezier(currentindex, t);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(gizmoPos, 5f);
            }
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(new Vector3(waypoints[CalculateIndex(i)].position.x, waypoints[CalculateIndex(i)].position.y, waypoints[CalculateIndex(i)].position.z),
                            new Vector3(waypoints[CalculateIndex(i, 1)].position.x, waypoints[CalculateIndex(i, 1)].position.y, waypoints[CalculateIndex(i, 1)].position.z));
            //Gizmos.DrawLine(new Vector3(waypoints[CalculateIndex(i, 2)].transform.position.x, waypoints[CalculateIndex(i, 2)].transform.position.y, waypoints[CalculateIndex(i, 2)].transform.position.z),
            //                new Vector3(waypoints[CalculateIndex(i, 3)].transform.position.x, waypoints[CalculateIndex(i, 3)].transform.position.y, waypoints[CalculateIndex(i, 3)].transform.position.z));
            currentindex += 3;
        }
    }

    int CalculateIndex(int index, int add = 0)
    {
        return (index + add) % waypoints.Count;
    }

    Vector3 CalculateCubicBezier(int iIndex, float tIndex)
    {
        return Mathf.Pow(1 - tIndex, 3) * waypoints[CalculateIndex(iIndex)].position +
               3 * Mathf.Pow(1 - tIndex, 2) * tIndex * waypoints[CalculateIndex(iIndex, 1)].position +
               3 * Mathf.Pow(tIndex, 2) * (1 - tIndex) * waypoints[CalculateIndex(iIndex, 2)].position +
               Mathf.Pow(tIndex, 3) * waypoints[CalculateIndex(iIndex, 3)].position;
    }

    void MoveInLine()
    {
        if (!target) return;
        // for loop wont work in Update method
        if (Vector3.Distance(waypoints[currentWaypoint].position, target.transform.position) < wpRadius)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Count)
            {
                currentWaypoint = 0;
            }
        }
        //  target.gameObject.transform.LookAt(waypoints[currentWaypoint].transform.position);
        var targetRotation = Quaternion.LookRotation(waypoints[currentWaypoint].position - target.transform.position);
        target.transform.rotation = Quaternion.Slerp(target.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        target.transform.position = Vector3.MoveTowards(target.transform.position, waypoints[currentWaypoint].position, Time.deltaTime * speed);
    }

    void MoveInCurve()
    {
        if (!target) return;
        if (Vector3.Distance(bezierWaypointsPosition[currentBezierWaypoint], target.transform.position) < wpRadius)
        {
            currentBezierWaypoint++;
            if (currentBezierWaypoint >= bezierWaypointsPosition.Count)
            {
                currentBezierWaypoint = 0;
            }
        }
        var targetRotation = Quaternion.LookRotation(bezierWaypointsPosition[currentBezierWaypoint] - target.transform.position);
        target.transform.rotation = Quaternion.Slerp(target.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        target.transform.position = Vector3.MoveTowards(target.transform.position, bezierWaypointsPosition[currentBezierWaypoint], Time.deltaTime * speed);
    }

    //IEnumerator MoveInCurve()
    //{
    //    coroutineActive = true;
    //    if (!target) yield break;
    //    if (waypoints == null) yield break;
    //    int currentindex = 0;
    //    for (int i = 0; i < waypoints.ToArray().Length; i++)
    //    {
    //        for (float t = 0; t <= 1; t += 0.03f)
    //        {
    //            nextPos = CalculateCubicBezier(currentindex, t);
    //            var targetRotation = Quaternion.LookRotation(nextPos - target.transform.position);
    //            target.transform.rotation = Quaternion.Slerp(target.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    //            target.gameObject.transform.position = Vector3.MoveTowards(target.gameObject.transform.position, nextPos, Time.deltaTime * speed);
    //        }
    //        currentindex += 3;
    //        moveInCurveLoopEnded = true;
    //        yield return new WaitUntil(GetMoveInCurveLoopEnded);
    //    }
    //    moveInCurveLoopEnded = false;
    //    coroutineActive = false;
    //}

    bool GetMoveInCurveLoopEnded()
    {
        return moveInCurveLoopEnded;
    }

    void GetChildWaypoints()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            waypoints.Add(gameObject.transform.GetChild(i).gameObject.transform);
        }
    }

    void GetBezierWaypoints()
    {
        int currentindex = 0;
        for (int i = 0; i < waypoints.ToArray().Length; i++)
        {
            for (float t = 0; t <= 1; t += 0.05f)
            {
                bezierWaypointsPosition.Add(CalculateCubicBezier(currentindex, t));
            }
            currentindex += 3;
        }
    }
}
