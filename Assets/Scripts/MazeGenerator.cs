using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{

    [Range(5, 1000)]
    public int mazeWidth = 5, mazeHeight = 5; //The dimension of the maze
    public int startX, startY;     //The position the algorithm will start
    MazeCell[,] maze;

    Vector2Int currentCell; //The maze cell we are currently looking at

    public MazeCell[,] GetMaze() {

        maze = new MazeCell[mazeWidth, mazeHeight];

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeWidth; y++)
            {

                maze[x, y] = new MazeCell(x, y);

            }
        }

        CarvePath(startX, startY);

        return maze;

    }

    List<Direction> directions = new List<Direction> {

        Direction.Up, Direction.Down, Direction.Left, Direction.Right,

    };

    List<Direction> GetRandomDirections()
    {

        //Make a copy of the directions list that the list can mess around with 
        List<Direction> dir = new List<Direction>(directions);

        //Make a directions list to put the randomised directions into
        List<Direction> rndDirv = new List<Direction>();

        while (dir.Count > 0)
        { //Log until rndDir list is empty

            int rnd = Random.Range(0, dir.Count); //Get random index in list
            rndDirv.Add(dir[rnd]); //Add the random direction to our list
            dir.RemoveAt(rnd); //Remove that direction so it can't choose it again

        }

        //When all four directions are in a random order, return the queue
        return rndDirv;

    }

    bool IsCellValid(int x, int y)
    {

        if (x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight - 1 || maze[x, y].visited) return false;
        else return true;
    }

    Vector2Int CheckNeighbours()
    {

        List<Direction> rndDir = GetRandomDirections();

        for (int i = 0; i < rndDir.Count; i++)
        {

            //Set neighbour coordinates to current cell for now
            Vector2Int neighbour = currentCell;

            switch (rndDir[i])
            {

                case Direction.Up:
                    neighbour.y++;
                    break;
                case Direction.Down:
                    neighbour.y--;
                    break;
                case Direction.Right:
                    neighbour.x++;
                    break;
                case Direction.Left:
                    neighbour.x--;
                    break;
            }

            //If the neighbour that was just tried is valid, that neighbour can be returned. If not, the loop goes again
            if (IsCellValid(neighbour.x, neighbour.y)) return neighbour;

        }

        // If all directions were tried and didn't fina a valid neighbour, currrentCell values are returned.
        return currentCell;

    }

    //Takes in two maze positions and sets the cells accordingly
    void BreakWalls(Vector2Int primaryCell, Vector2Int secondaryCell)
    {

        //Can only go in one direction at a time so if else statements can be used
        if (primaryCell.x > secondaryCell.x)
        { //Primary Cell's Left Wall

            maze[primaryCell.x, primaryCell.y].leftWall = false;

        }
        else if (primaryCell.x < secondaryCell.x)
        { //Secondary Cell's Left Wall

            maze[secondaryCell.x, secondaryCell.y].leftWall = false;

        }
        else if (primaryCell.y < secondaryCell.y)
        { //Primary Cell's Top Wall

            maze[primaryCell.x, primaryCell.y].topWall = false;

        }
        else if (primaryCell.y > secondaryCell.y)
        { //Secondary Cell's Top Wall

            maze[secondaryCell.x, secondaryCell.y].topWall = false;

        }
    }

    //Starting at the x,y passed in, carves a path through the maze until it encounters a "dead end" (a cell with no valid neighbours)
    void CarvePath(int x, int y)
    {

        //Perform a quick check to make sure the starting position is within the boundaries of the map
        // if not, set them to a default (0) and give a warning
        if (x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight - 1)
        {

            x = y = 0;
            Debug.LogWarning("Starting position is out of bounds, defaulting to 0, 0");

        }

        //Set current cell to the starting position that was passed
        currentCell = new Vector2Int(x, y);

        //A list to keep track of the current path
        List<Vector2Int> path = new List<Vector2Int>();

        //Loop until we encounter a dead end
        bool deadEnd = false;
        while (!deadEnd) {

            //Get the next cell 
            Vector2Int nextCell = CheckNeighbours();

            //If that cell has no valid neighbours, set dead end to true so loop stops
            if (nextCell == currentCell) {

                //If that cell has no valid neighbours, set dead end to true so loop is stopped
                for (int i = path.Count - 1; i >= 0; i--)
                {
                    currentCell = path[i]; //Set currentCell to the next step
                    path.RemoveAt(i); //Remove this step from the path
                    nextCell = CheckNeighbours(); //Check that cell to see if any other neighbours are valid

                    //IF a valid neighbour is found, break out of the loop
                    if (nextCell != currentCell) break;
                }

                if (nextCell == currentCell)
                    deadEnd = true;

            } else {

                BreakWalls(currentCell, nextCell); //Set wall flags on these two ccells
                maze[currentCell.x, currentCell.y].visited = true; //Sett cell to visited before moving on
                currentCell = nextCell; //Set the current cell to the valid neighbour that was found
                path.Add(currentCell); //Add this cell to the path
            }
        }

    }

}

public enum Direction {

    Up,
    Down,
    Left,
    Right

}

public class MazeCell {
    public bool visited;
    public int x, y;

    public bool topWall;
    public bool leftWall;

    //Return x and y as a Vector2Int for convenience
    public Vector2Int position {
        get {
            return new Vector2Int(x, y);
            }
    }

    public MazeCell(int x, int y) {

        //The coordinates of this cell in the maze grid
        this.x = x;
        this.y = y;

        //Weather the algorithm has visited this cell ot not - false to start 
        visited = false;

        //All walls are present until the algorithm removes them
        topWall = leftWall = true;
    }

}