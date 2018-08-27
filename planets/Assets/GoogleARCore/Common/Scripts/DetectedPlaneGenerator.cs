//-----------------------------------------------------------------------
// <copyright file="DetectedPlaneGenerator.cs" company="Google">
//
// Copyright 2018 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------


//MODIFIED TO DISABLE PLANE VISUALS ONCE A STAR HAS SPAWNED
namespace GoogleARCore.Examples.Common
{
    using System.Collections.Generic;
    using GoogleARCore;
    using UnityEngine;

    /// Manages the visualization of detected planes in the scene.
    public class DetectedPlaneGenerator : MonoBehaviour
    {
        /// A prefab for tracking and visualizing detected planes.
        public GameObject DetectedPlanePrefab;

        /// A list to hold new planes ARCore began tracking in the current frame. This object is used across
        /// the application to avoid per-frame allocations.
        List<DetectedPlane> m_NewPlanes = new List<DetectedPlane>();
        public List<GameObject> detectedPlanes; // list to hold plane visuals
        Star star; //star script on star

		public void Awake()
		{
            star = GameObject.FindWithTag("Star").GetComponent<Star>(); //initialize star script
            detectedPlanes = new List<GameObject>(); //initialize list
		}

		public void Update()
        {
            // Check that motion tracking is tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                return;
            }

            // Iterate over planes found in this frame and instantiate corresponding GameObjects to visualize them.
            Session.GetTrackables<DetectedPlane>(m_NewPlanes, TrackableQueryFilter.New);
            for (int i = 0; i < m_NewPlanes.Count; i++)
            {
                // Instantiate a plane visualization prefab and set it to track the new plane. The transform is set to
                // the origin with an identity rotation since the mesh for our prefab is updated in Unity World
                // coordinates.
                if (!star.starEnabled) //only add visuals if no star enabled
                {
                    GameObject planeObject = Instantiate(DetectedPlanePrefab, Vector3.zero, Quaternion.identity, transform);
                    planeObject.GetComponent<DetectedPlaneVisualizer>().Initialize(m_NewPlanes[i]);
                    detectedPlanes.Add(planeObject);
                }
            }

            if (star.starEnabled){ //disable visuals once star appears
                foreach (GameObject plane in detectedPlanes){
                    plane.SetActive(false);
                }
            }
        }
    }
}
