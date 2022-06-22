using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class NavMesh2D : MonoBehaviour
    {
        public int width;
        public int height;
        public Vector2 cellSize;
        public bool showDebug = false;
        [SerializeField] private BoxCollider2D navMeshVolume;
        [SerializeField] private List<Vector2> points = new List<Vector2>();
        private CustomGrid grid;

        // Start is called before the first frame update
        void Start()
        {
            // Using the navMeshVolume determines the width and height of the nav mesh grid.
            width = Mathf.FloorToInt(navMeshVolume.size.x / cellSize.x);
            height = Mathf.FloorToInt(navMeshVolume.size.y / cellSize.y);
            // Create the nav mesh grid.
            grid = new CustomGrid(width, height, cellSize, navMeshVolume.bounds.center);
            // On game start compute nav mesh data
            ComputeNavigableSurface();
        }

        // Update is called once per frame
        void Update()
        {
           
        }

        void OnDrawGizmos()
        {
            if (showDebug)
            {
                width = Mathf.FloorToInt(navMeshVolume.size.x / cellSize.x);
                height = Mathf.FloorToInt(navMeshVolume.size.y / cellSize.y);
                
                if(grid == null || (grid.width != width || grid.height != height))
                    grid = new CustomGrid(width, height, cellSize, navMeshVolume.bounds.center - navMeshVolume.bounds.extents);
                
                // Compute points to visualize nav mesh data in debug mode
                ComputeNavigableSurface();

                foreach (Vector2 point in points)
                    Gizmos.DrawWireSphere(point, .1f);
                
                grid.DrawGrid(Color.grey);
            }
        }

        private void ComputeNavigableSurface()
        {
            points.Clear();
            foreach (Vector2 point in grid.gridPoints)
            {
                if (!points.Contains(point) && Physics2D.OverlapCircle(point, .1f) == null)
                    points.Add(point);

                Vector2Int[] nearbyCells = grid.GetNearbyCells(point);
                foreach (Vector2Int cell in nearbyCells)
                {
                    Vector2 targetPosition = grid.GetWorldPosition(cell.x, cell.y) + cellSize * 0.5f;
                    Vector2 pointDirection = Math.GetUnitDirectionVector(point, targetPosition);
                    float distance = Vector2.Distance(point, targetPosition);
                    RaycastHit2D result = Physics2D.Raycast(point, pointDirection, distance);
                    if (result.collider == null && !points.Contains(targetPosition))
                        points.Add(targetPosition);
                }
            }
        }
    }
}

