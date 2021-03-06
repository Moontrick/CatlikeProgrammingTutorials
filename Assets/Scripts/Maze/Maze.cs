﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour 
{


    [SerializeField]
    private IntVector2 size;

    [SerializeField]
    private MazeCell cellPrefab;

    [SerializeField]
    private float generationStepDelay;

    [SerializeField]
    private MazePassage passagePrefab;

    [SerializeField]
    private MazeWall[] wallPrefabs;

    [SerializeField]
    private MazeDoor doorPrefab;

    [SerializeField]
    [Range(0f, 1f)]
    private float doorProbability;

    [SerializeField]
    private MazeRoomSettings[] roomSettings;


    private MazeCell[,] cells;
    private List<MazeRoom> rooms = new List<MazeRoom>();


    // Returns an IntVector2 with random x and z
    public IntVector2 RandomCoordinates
    { get { return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z)); } }


    // Checks to see if an IntVector2 coordinate is within the bounds of the size of the maze
    public bool ContaintsCoordinates(IntVector2 coordinate)
    { return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z; }


    // ----------------------------------------------
    // Mono Functions


    // ----------------------------------------------
    // Public Functions


    public MazeCell GetCell(IntVector2 coordinate)
    { return cells[coordinate.x, coordinate.z]; }


    public MazeRoom CreateRoom(int indexToExclude)
    {
        MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom>();
        newRoom.settingsIndex = Random.Range(0, roomSettings.Length);
        
        if (newRoom.settingsIndex == indexToExclude)
            newRoom.settingsIndex = (newRoom.settingsIndex + 1) % roomSettings.Length;

        newRoom.settings = roomSettings[newRoom.settingsIndex];
        rooms.Add(newRoom);
        return newRoom;
    }


    public IEnumerator Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);

        cells = new MazeCell[size.x, size.z];
        List<MazeCell> activeCells = new List<MazeCell>();
        DoFirstGenerationStep(activeCells);

        while (activeCells.Count > 0)
        {
            yield return delay;
            DoNextGenerationStep(activeCells);
        }

        for (int i = 0; i < rooms.Count; i++)
            rooms[i].Hide();
    }


    // ----------------------------------------------
    // Private Functions


    private void DoFirstGenerationStep(List<MazeCell> activeCells)
    {
        MazeCell newCell = CreateCell(RandomCoordinates);
        newCell.Initialize(CreateRoom(-1));
        activeCells.Add(newCell);
    }


    private void DoNextGenerationStep(List<MazeCell> activeCells)
    {
        int currentIndex = activeCells.Count - 1;
        MazeCell currentCell = activeCells[currentIndex];

        // Before we do anything, check if this cell is fully initialized
        if (currentCell.IsFullyInitialized)
        {
            activeCells.RemoveAt(currentIndex);
            return;
        }

        MazeDirection direction = currentCell.RandomUninitializedDirection;
        IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();

        if (ContaintsCoordinates(coordinates))
        {
            MazeCell neighbor = GetCell(coordinates);
            if (neighbor == null)
            {
                neighbor = CreateCell(coordinates);
                CreatePassage(currentCell, neighbor, direction);
                activeCells.Add(neighbor);
            }
            else if (currentCell.Room.settingsIndex == neighbor.Room.settingsIndex)
                CreatePassageInSameRoom(currentCell, neighbor, direction);
            else
                CreateWall(currentCell, neighbor, direction);
        }
        else
            CreateWall(currentCell, null, direction);
    }


    private MazeCell CreateCell(IntVector2 coordinates)
    {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        cells[coordinates.x, coordinates.z] = newCell;

        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);

        return newCell;
    }


    private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazePassage prefab = Random.value < doorProbability ? doorPrefab : passagePrefab;
        MazePassage passage = Instantiate(prefab) as MazePassage;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(prefab) as MazePassage;

        if (passage is MazeDoor)
            otherCell.Initialize(CreateRoom(cell.Room.settingsIndex));
        else
            otherCell.Initialize(cell.Room);

        passage.Initialize(otherCell, cell, direction.GetOpposite());
    }


    private void CreatePassageInSameRoom(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazePassage passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(otherCell, cell, direction.GetOpposite());

        if (cell.Room != otherCell.Room)
        {
            MazeRoom roomToAssimilate = otherCell.Room;
            cell.Room.Assimilate(roomToAssimilate);
            rooms.Remove(roomToAssimilate);
            Destroy(roomToAssimilate);
        }
    }


    private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazeWall wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
        wall.Initialize(cell, otherCell, direction);
        if (otherCell != null)
        {
            wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
            wall.Initialize(otherCell, cell, direction.GetOpposite());
        }
    }


}
