using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class FollowPlr : MonoBehaviour
{
        public Transform target;
        private NavMeshAgent agent;
        public float Radius = 100f;


        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            // inRange = Vector3();
        }
        // Update is called once per frame
        void Update()
        {
            if(Math.Abs(target.transform.position.x - this.transform.position.x) < Radius && Math.Abs(target.transform.position.y - this.transform.position.y) < Radius)
            agent.SetDestination(target.position);
        }


}
