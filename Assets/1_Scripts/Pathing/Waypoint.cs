﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD47.Pathing
{
    public class Waypoint : MonoBehaviour
    {
        private const string TAG = "Player";

        [SerializeField] bool isPickedUp = false;
        [Header("Waypoint Rotation")]
        [SerializeField] float degreesPerSecond = 15.0f;
        [SerializeField] float amplitude = 0.5f;
        [SerializeField] float frequency = 1f;

        Vector3 posOffset = new Vector3();
        Vector3 tempPos = new Vector3();
        GameObject obj_player;
        ParticleSystem cmp_waypointFX;
        MeshRenderer cmp_meshRenderer;

        void Awake()
        {
            obj_player = GameObject.FindWithTag(TAG);
            posOffset = transform.position;
        }

        void Update()
        {
            if (isPickedUp)
            {
                transform.position = obj_player.transform.position;
            }
            else
            {
                Rotate();
                OscillateVertically();
            }     
        }

        void OscillateVertically()
        {
            tempPos = posOffset;
            tempPos.y += (Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude) + amplitude;

            transform.position = tempPos;
        }

        void Rotate()
        {
            transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
        }

        void PickupWaypoint()
        {
            cmp_meshRenderer.enabled = false;
            cmp_waypointFX.IsAlive(false);
            isPickedUp = true;

        }

        void PlaceWaypoint()
        {
            cmp_meshRenderer.enabled = true;
            cmp_waypointFX.IsAlive(true);
            isPickedUp = false;
        }

    }

}