using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPathway : MonoBehaviour
{
    
    public List<Waypoint> pathway;

    void OnDrawGizmos() {
         Gizmos.color = Color.blue;
        //  Gizmos.DrawWireCube(transform.position, new Vector3(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z));

        // for each waypoint less than count, add line
        Vector3 prevPoint = pathway[0].transform.position;

        for(int i = 1; i < pathway.Count; i += 1) {
            Vector3 nextPoint = pathway[i].transform.position;
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }

    }

}
