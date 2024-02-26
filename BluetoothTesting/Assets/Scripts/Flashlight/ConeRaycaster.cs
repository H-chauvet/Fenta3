using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeRaycaster : MonoBehaviour
{
    public Light spotLight; // Reference to the Spot Light component
    public float rangeAmplifier; //Value that determines the relation between the light's intensity and the range of detect area
    public Material replacedMaterial;
    

    //[SerializeField] private Camera camera;
    
        private Transform tip; // Tip of the cone
        private Transform centerBase; //Center of the base of the cone
        private Transform[] basePoints; // Array of 4 points determining the circle at the bottom

        private SphereCollider lightDetectArea;
        
        
        void Start()
        {
            // Initialize tip and basePoints
            InitializeConePoints();
            lightDetectArea = GetComponentInChildren<SphereCollider>();
        }
    
        void Update()
        {
            // Update cone points positions based on Spot Light parameters
            UpdateConePoints();
    
            // Shoot raycasts between all pairs of points
            ShootRaycasts();
        }
    
        void InitializeConePoints()
        {
            // Create the tip and basePoints objects
            tip = new GameObject("ConeTip").transform;
            centerBase = new GameObject("ConeCenterBase").transform;
            
            tip.SetParent(transform);
            centerBase.SetParent(transform);
            
            basePoints = new Transform[4];
            for (int i = 0; i < basePoints.Length; i++)
            {
                basePoints[i] = new GameObject("BasePoint" + i).transform;
                basePoints[i].SetParent(transform);
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

            lightDetectArea.gameObject.transform.position = (centerBase.position + tip.transform.position) / 2;
            lightDetectArea.radius = spotLight.range / rangeAmplifier;
            
            // float cX = spotLight.transform.forward.x; //Mathf.Cos(Mathf.Deg2Rad * angleIncrement) * Mathf.Tan(Mathf.Deg2Rad * coneAngle);
            // float cY = spotLight.transform.forward.y;
            // float cZ = spotLight.transform.forward.z; //Mathf.Sin(Mathf.Deg2Rad * angleIncrement) * Mathf.Tan(Mathf.Deg2Rad * coneAngle);
            
            
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
                    HandleCollisions(hit.collider);
                }
                
                foreach (Transform otherPoint in basePoints) //Corner -> other corners
                {
                    if (otherPoint == basePoint) continue;
                    Vector3 oDirection = (otherPoint.position - basePoint.position).normalized;
                    float oDistance = Vector3.Distance(basePoint.position, otherPoint.position);

                    RaycastHit[] otherHits = Physics.RaycastAll(basePoint.position, oDirection, oDistance);
                    
                    //Debug.Log("hey");
                    
                    foreach (RaycastHit oHit in otherHits)
                    {
                        // Handle collisions here
                        //Debug.Log("Base between " + basePoint.name + "and " + otherPoint.name + " collision with " + oHit.collider.gameObject.name);
                        HandleCollisions(oHit.collider);
                    }
                }
            }
            //Tip -> middle
            Vector3 midDirection = (centerBase.position - tip.position).normalized;
            float midDistance = Vector3.Distance(tip.position, centerBase.position);
    
            RaycastHit[] midHits = Physics.RaycastAll(tip.position, midDirection, midDistance);
                
            foreach (RaycastHit midHit in midHits) 
            {
                // Handle collisions here
                //Debug.Log("Mid collision with " + midHit.collider.gameObject.name);
                HandleCollisions(midHit.collider);
            }
        }
    
        void OnDrawGizmos()
        {
            if (tip == null) return;
            // Visualize the cone using Gizmos
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(tip.position, basePoints[0].position);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(tip.position, basePoints[1].position);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(tip.position, basePoints[2].position);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(tip.position, basePoints[3].position);
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(tip.position, centerBase.position);
        
            Gizmos.color = Color.green;
            Gizmos.DrawLine(basePoints[0].position, basePoints[1].position);
            Gizmos.DrawLine(basePoints[1].position, basePoints[2].position);
            Gizmos.DrawLine(basePoints[2].position, basePoints[3].position);
            Gizmos.DrawLine(basePoints[3].position, basePoints[0].position);
            
        }

        void HandleCollisions(Collider col)
        {
            //col.gameObject.GetComponent<Renderer>().material = replacedMaterial;
        }
}
