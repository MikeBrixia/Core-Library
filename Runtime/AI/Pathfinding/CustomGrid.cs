using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class CustomGrid
    {
        public int width { get; private set; }
        public int height { get; private set; }
        public float cellSize { get; private set; }
        public List<Vector2> gridPoints { get; private set; }
        private Vector2 origin;

        public CustomGrid(int width, int height, float cellSize, Vector2 origin)
        {
            // Initialize grid
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.origin = origin;
            this.gridPoints = new List<Vector2>();

            ComputeGridPoints();
        }

        public Vector2 GetWorldPosition(int x, int y)
        {
            return new Vector2(x, y) * cellSize + origin;
        }

        public void DrawGrid()
        {
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.black);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.black);
                }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.black);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.black);
        }

        private void ComputeGridPoints()
        {
            gridPoints.Clear();
            for (int i = 0; i < width * height; i++)
            {
                for(int x = 0; x < width; x++)
                    for(int y = 0; y < height; y++)
                    {
                        Vector2 pos = GetWorldPosition(x, y) + new Vector2(cellSize, cellSize) * 0.5f;
                        if(!gridPoints.Contains(pos))
                            gridPoints.Add(pos);
                    }
            }
        }

        ///<summary>
        /// Get grid cell x and y from world position.
        ///</summary>
        ///<returns> The width(x) and height(y) of the cell the world position correspond on</returns>
        public void GetXY(Vector2 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt(worldPosition.x / cellSize - origin.x);
            y = Mathf.FloorToInt(worldPosition.y / cellSize - origin.y);
        }

        public Vector2Int[] GetNearbyCells(Vector2 worldPosition)
        {
            int x = 0;
            int y = 0;
            GetXY(worldPosition, out x, out y);
            Vector2Int[] nearbyCells = new Vector2Int[8];
            //east cell
            nearbyCells[0] = SetCell(x+1, y);
            //west cell
            nearbyCells[1] = SetCell(x-1, y);
            //north cell
            nearbyCells[2] = SetCell(x, y+1);
            //south cell
            nearbyCells[3] = SetCell(x, y-1);
            //north-east cell
            nearbyCells[4] = SetCell(x+1, y+1);
            //north-west cell
            nearbyCells[5] = SetCell(x-1, y+1); 
            //south-east cell
            nearbyCells[6] = SetCell(x+1, y-1); 
            //south_west cell
            nearbyCells[7] = SetCell(x-1, y-1);
            return nearbyCells;
        }
        
        private Vector2Int SetCell(int x, int y)
        {
            Vector2Int cell = Vector2Int.zero;
            if((x > 0 && y > 0) && (x < width && y < height))
                cell = new Vector2Int(x, y);
            //Gizmos.DrawWireSphere(GetWorldPosition(cell.x, cell.y) + new Vector2(cellSize, cellSize) * 0.5f, .1f);
            return cell;
        }

        public override bool Equals(object obj)
        {
            CustomGrid other = obj as CustomGrid;
            return width == other.width & height == other.height & cellSize == other.cellSize;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

