using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserController : MonoBehaviour
{
    public GameObject pointLightPrefab;
    public Material laserMaterial;
    public float maxLaserLength = 10f;
    public float growthSpeed = 10f;
    public float lightInterval = 2f;

    private LineRenderer lineRenderer;
    private BoxCollider boxCollider;
    private float currentLength;
    private bool hitBoss;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        boxCollider = GetComponent<BoxCollider>();

        // Set the color of the LineRenderer
        lineRenderer.material = laserMaterial;

        boxCollider.isTrigger = true;
        currentLength = 0f;
        hitBoss = false;
    }

    private void Update()
    {
        if (currentLength < maxLaserLength)
        {
            currentLength += growthSpeed * Time.deltaTime;
            currentLength = Mathf.Min(currentLength, maxLaserLength);

            // Set the positions of the line renderer
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, new Vector3(0f, -currentLength, 0f));

            // Update the BoxCollider size and center
            boxCollider.size = new Vector3(lineRenderer.startWidth, currentLength, lineRenderer.startWidth);
            boxCollider.center = new Vector3(0f, -currentLength / 2, 0f);

            // If a light should be added this frame, add it
            if (Mathf.Floor(currentLength / lightInterval) > transform.childCount - 1)
            {
                GameObject newLight = Instantiate(pointLightPrefab, transform);
                newLight.transform.localPosition = new Vector3(0f, -currentLength, 0f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other collider's tag is "boss"
        if (!hitBoss && other.CompareTag("boss"))
        {
            hitBoss = true;

            // Try to get the boss script
            DemonBossController boss = other.GetComponent<DemonBossController>();

            // Call the method
            StartCoroutine(boss.LaserChangeColorNTimes(GlobalValues.nrHitFlashes));
        }
    }
}
