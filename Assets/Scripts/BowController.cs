using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BowArrow
{

    public class BowController : MonoBehaviour
    {
        // the prefab to use as a projectile - needs to have an ArrowController.cs attached to it
        [SerializeField] private GameObject projectilePF = null;
        // draw distance is measured from the grip to the string at max pull (make sure arrows are long enough)
        [SerializeField] private float maxDrawDistance = 0.74f;
        // the force required to fully draw the string
        [SerializeField] private float maxDrawForce = 450.0f;
        // this is the time it takes the user to fully draw the bow in seconds
        [SerializeField] private float maxDrawTime = 1.0f;
        // brace height is the distance from the grip to the unloaded string position (geometry dependent)
        [SerializeField] private float braceHeight = 0.18f;
        // Time it takes to reload arrow
        [SerializeField] private float reloadTime = 0.5f;

        private bool readyToFire = false;
        private GameObject spawnPoint = null;
        private ArrowController projectileController = null;
        private float actualMaxDraw = 0;
        private float drawDistance = 0;
        private float drawRate = 0;

        // Start is called before the first frame update
        void Start()
        {
            actualMaxDraw = maxDrawDistance - braceHeight;
            spawnPoint = transform.Find("DrawPoint").gameObject;
            SpawnProjectile();
            drawRate = (actualMaxDraw) / maxDrawTime;
            readyToFire = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0) && readyToFire && drawDistance < actualMaxDraw)
            {
                drawDistance += drawRate * Time.deltaTime;
                UpdateDrawPoint(drawDistance);
            }
            else if (Input.GetMouseButtonUp(0) && drawDistance > 0.05f)
            {
                FireProjectile();
                drawDistance = 0;
                UpdateDrawPoint(drawDistance);
                readyToFire = false;
                StartCoroutine(ReloadTimer());
            }
            else if (Input.GetMouseButtonUp(0))
            {
                drawDistance = 0;
                UpdateDrawPoint(drawDistance);
            }
        }

        IEnumerator ReloadTimer()
        {
            yield return new WaitForSeconds(reloadTime);
            SpawnProjectile();
            readyToFire = true;
        }

        void UpdateDrawPoint(float drawDistance)
        {
            float z = -braceHeight - drawDistance;
            spawnPoint.transform.localPosition = new Vector3(0, 0, z);
        }

        void SpawnProjectile()
        {
            GameObject projectile = Instantiate(projectilePF, spawnPoint.transform);
            projectileController = projectile.GetComponent<ArrowController>();
            float length = projectileController.Length;
            projectile.transform.position = projectile.transform.position + transform.forward * length / 2;

        }

        void FireProjectile()
        {
            float fractionOfMax = drawDistance / actualMaxDraw;
            float energy = 0.5f * drawDistance * maxDrawForce * fractionOfMax;
            float mass = projectileController.RB.mass;
            float vel = Mathf.Sqrt(2 * energy / mass);
            projectileController.Launch(vel);
        }

    }
}