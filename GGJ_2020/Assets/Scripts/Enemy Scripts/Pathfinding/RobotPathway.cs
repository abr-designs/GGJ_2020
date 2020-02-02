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

        // foreach(Waypoint waypoint in pathway) {
        //     Gizmos.DrawLine(transform.position, target.position);
        // }

    }

}
