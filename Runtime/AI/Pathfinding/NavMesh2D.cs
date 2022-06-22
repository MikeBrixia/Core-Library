using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class NavMesh2D : MonoBehaviour
    {
        public int width;
        public int height;
        public float cellSize;
        
        [SerializeField] private List<Vector2> points = new List<Vector2>();
        private CustomGrid grid;

        // Start is called before the first frame update
        void Start()
        {
    
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnDrawGizmos()
        {
            grid = new CustomGrid(width, height, cellSize, transform.position);
            
            //foreach(Vector2 point in grid.gridPoints)
            //    Gizmos.DrawWireSphere(point, .1f);
            
            ComputeNavigableSurface();

           foreach(Vector2 point in points)
                Gizmos.DrawWireSphere(point, .1f);
        
            //grid.DrawGrid();
        }

        private void ComputeNavigableSurface()
        {
            points.Clear();
            foreach(Vector2 point in grid.gridPoints)
            {
                if(!points.Contains(point) && Physics2D.OverlapCircle(point, .1f) == null)
                    points.Add(point);

                Vector2Int[] nearbyCells = grid.GetNearbyCells(point);
                foreach(Vector2Int cell in nearbyCells)
                {
                    Vector2 targetPosition = grid.GetWorldPosition(cell.x, cell.y) + new Vector2(cellSize, cellSize) * 0.5f;
                    Vector2 pointDirection = Math.GetUnitDirectionVector(point, targetPosition);
                    float distance = Vector2.Distance(point, targetPosition);
                    RaycastHit2D result = Physics2D.Raycast(point, pointDirection, distance);
                    if(result.collider == null && !points.Contains(targetPosition))
                        points.Add(targetPosition);
                }
            }
        }
    }
}

