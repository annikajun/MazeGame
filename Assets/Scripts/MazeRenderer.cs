using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{

    [SerializeField] MazeGenerator mazeGenerator;
    [SerializeField] GameObject MazeCellPrefab;

    //This is the physical size of the maze cell
    public float CellSize = 1f;

    private void Start() {

        // Gets MazeGenerator script to make a maze
        MazeCell[,] maze = mazeGenerator.GetMaze();

        //Loop through every cell in the maze
        for (int x = 0; x < mazeGenerator.mazeWidth; x++) {
            for (int y = 0; y < mazeGenerator.mazeHeight; y++)
            {

                //Instantiate a new maze cell prefab as a child of the MazeRenderer object
                GameObject newCell = Instantiate(MazeCellPrefab, new Vector3((float)x * CellSize, 0f, (float)y * CellSize), Quaternion.identity, transform);

                //Get a reference to the cell's MazeCellPrefab script
                MazeCellObject mazeCell = newCell.GetComponent<MazeCellObject>();

                //Determine which wall needs to be active
                bool top = maze[x, y].topWall;
                bool left = maze[x, y].leftWall;

                //Bottom and right walls are deactivated by default unless position is at the bottom or right edge of the maze
                bool right = false;
                bool bottom = false;
                if (x == mazeGenerator.mazeWidth - 1) right = true;
                if (y == 0) bottom = true;

                mazeCell.Init(top, bottom, right, left);
                

            }
            
        }

    }

}

