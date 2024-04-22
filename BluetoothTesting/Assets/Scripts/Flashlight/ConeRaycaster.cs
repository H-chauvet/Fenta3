using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConeRaycaster : MonoBehaviour
{
        public Light spotLight; // Reference to the Spot Light component
        public float rangeAmplifier; //Value that determines the relation between the light's intensity and the range of detect area
        //public Material replacedMaterial;
    

        //[SerializeField] private Camera camera;
    
        [HideInInspector]public Transform tip; // Tip of the cone
        [HideInInspector]public Transform centerBase; //Center of the base of the cone
        [HideInInspector]public Transform midLength; //Middle of the height of the cone
        [HideInInspector]public Transform[] basePoints; // Array of 4 points determining the circle at the bottom

        public GameObject lightDetectArea; //Sphere collider attached to the light; SWITCH BACK TO PRIVATE AFTER TESTING
        
        
        void Start()
        {
             // Initialize tip and basePoints
             InitializeConePoints();
            
        }
    
        void Update()
        {
            // Update cone points positions based on Spot Light parameters
             UpdateConePoints();
            
            midLength.tag = "Extremity";
            centerBase.tag = "Extremity";
            basePoints[2].tag = "Extremity";
            basePoints[3].tag = "Extremity";
            // Shoot raycasts between all pairs of points
             ShootRaycasts();
        }

        void InitializeConePoints()
        {
            // Create the tip and basePoints objects
            tip = new GameObject("ConeTip").transform;
            tip.SetParent(transform);
            
            centerBase = new GameObject("ConeCenterBase")
            {
                tag = "Extremity",
                transform = {parent = this.transform}
            }.transform;

            midLength = new GameObject("MidLength")
            {
                tag = "Extremity",
                transform = {parent = this.transform}
            }.transform;
            
            basePoints = new Transform[4];
            for (int i = 0; i < basePoints.Length; i++)
            {
                basePoints[i] = new GameObject("BasePoint" + i)
                {
                    tag = "Extremity",
                    transform = {parent = this.transform}
                }.transform;
                
            }

            SphereCollider centerCol = centerBase.AddComponent<SphereCollider>();
            centerCol.isTrigger = true;
            centerCol.radius = Mathf.Epsilon;

            SphereCollider middleCol = midLength.AddComponent<SphereCollider>();
            middleCol.isTrigger = true;
            middleCol.radius = Mathf.Epsilon;
            
            for (int i = 0; i < basePoints.Length; i++)
            {
                SphereCollider sideCol = basePoints[i].AddComponent<SphereCollider>();
                sideCol.isTrigger = true;
                sideCol.radius = Mathf.Epsilon;
            }
        }
    
        void UpdateConePoints()
        {
            // Update tip position based on Spot Light position
            tip.position = spotLight.transform.position;
            
            
            // Update base points positions based on Spot Light parameters
            float coneAngle = spotLight.spotAngle;
            float coneHeight = spotLight.range;
            
            float radius = coneHeight * Mathf.Tan(Mathf.Deg2Rad * (coneAngle / 2));
    
            float angleIncrement = 360f / basePoints.Length;
            
            //Update center base position based on Spot Light dimensions
            centerBase.position = spotLight.transform.position + spotLight.transform.forward * spotLight.range;

            midLength.position = spotLight.transform.position + spotLight.transform.forward * spotLight.range / 2;

            //Offsets
            float posOff = radius;
            float negOff = -1 * (radius);
            
            //corners
            basePoints[0].position = centerBase.position + spotLight.transform.up * posOff;
            basePoints[1].position = centerBase.position + spotLight.transform.up * negOff;
            basePoints[2].position = centerBase.position + spotLight.transform.right * posOff;
            basePoints[3].position = centerBase.position + spotLight.transform.right * negOff;
            
        }
    
        void ShootRaycasts()
        {
            
            
            // Shoot raycasts and handle collisions
            foreach (Transform basePoint in basePoints) //Tip -> corner
            {
                Vector3 direction = (basePoint.position - tip.position).normalized;
                float distance = Vector3.Distance(tip.position, basePoint.position);
        
                RaycastHit[] hits = Physics.RaycastAll(tip.position, direction, distance);
                
                foreach (RaycastHit hit in hits) 
                {
                    // Handle collisions here
                    //Debug.Log("Edge" + basePoint.name + " collision with " + hit.collider.gameObject.name);
                    
                }
                
                foreach (Transform otherPoint in basePoints) //Corner -> other corners
                {
                    if (otherPoint == basePoint) continue;
                    Vector3 oDirection = (otherPoint.position - basePoint.position).normalized;
                    float oDistance = Vector3.Distance(basePoint.position, otherPoint.position);
        
                    RaycastHit[] otherHits = Physics.RaycastAll(basePoint.position, oDirection, oDistance);
                    
                    
                    
                    foreach (RaycastHit oHit in otherHits)
                    {
                        // Handle collisions here
                        //Debug.Log("Base between " + basePoint.name + "and " + otherPoint.name + " collision with " + oHit.collider.gameObject.name);
                        
                    }
                }
            }
            //Tip -> right
            Vector3 rightDirection = (basePoints[2].position - tip.position).normalized;
            float rightDistance = Vector3.Distance(tip.position, basePoints[2].position);
        
            RaycastHit[] rightHits = Physics.RaycastAll(tip.position, rightDirection, rightDistance);
                
            foreach (RaycastHit rightHit in rightHits) 
            {
                // Handle collisions here
                //Debug.Log("Mid collision with " + midHit.collider.gameObject.name);
                
            }
            
            //Tip -> left
            Vector3 leftDirection = (basePoints[3].position - tip.position).normalized;
            float leftDistance = Vector3.Distance(tip.position, basePoints[3].position);
        
            RaycastHit[] leftHits = Physics.RaycastAll(tip.position, leftDirection, leftDistance);
                
            foreach (RaycastHit leftHit in leftHits) 
            {
                // Handle collisions here
                //Debug.Log("Mid collision with " + midHit.collider.gameObject.name);
                
            }
            
            //Tip -> center
            Vector3 midDirection = (centerBase.position - tip.position).normalized;
            float midDistance = Vector3.Distance(tip.position, centerBase.position);
        
            RaycastHit[] midHits = Physics.RaycastAll(tip.position, midDirection, midDistance);
            //if(midHits.Length != 0 && (!midHits[0].collider.gameObject.CompareTag("Enemy") || !midHits[0].collider.gameObject.CompareTag("Extremity"))) midHits[0].collider.tag = "Untagged";
            Debug.DrawRay(tip.position, midDirection, Color.black);
            foreach (RaycastHit midHit in midHits) 
            {
                // Handle collisions here
                //Debug.Log("Mid collision with " + midHit.collider.gameObject.name);
                if (midHit.collider.gameObject == centerBase.gameObject) continue;
                
            }
        }
        
        public SphereCollider CreateSphere()
        {
            //lightDetectArea.tag = "LightArea";
            SphereCollider newSphere = lightDetectArea.AddComponent<SphereCollider>();
            newSphere.isTrigger = true;
            Vector3 pos = (centerBase.position + tip.transform.position) / 2;
            float rad = spotLight.range / rangeAmplifier;
            newSphere.center = transform.InverseTransformPoint(pos);
            newSphere.radius = rad;
            return newSphere;
        }
        
        // void OnDrawGizmos()
        // {
        //     if (tip == null) return;
        //     // Visualize the cone using Gizmos
        //     Gizmos.color = Color.cyan;
        //     Gizmos.DrawLine(tip.position, basePoints[0].position);
        //     Gizmos.color = Color.blue;
        //     Gizmos.DrawLine(tip.position, basePoints[1].position);
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawLine(tip.position, basePoints[2].position);
        //     Gizmos.color = Color.yellow;
        //     Gizmos.DrawLine(tip.position, basePoints[3].position);
        //     Gizmos.color = Color.magenta;
        //     //Gizmos.DrawLine(tip.position, centerBase.position);
        //
        //     Gizmos.color = Color.green;
        //     Gizmos.DrawLine(basePoints[0].position, basePoints[1].position);
        //     Gizmos.DrawLine(basePoints[1].position, basePoints[2].position);
        //     Gizmos.DrawLine(basePoints[2].position, basePoints[3].position);
        //     Gizmos.DrawLine(basePoints[3].position, basePoints[0].position);
        //     
        // }

        
}
