using UnityEngine;
using System.Collections;

public class MazeCell : MonoBehaviour 
{
    public IntVector2 coordinates;
    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];
    private int initializedEdgeCount;

    private MazeRoom _room;
    public MazeRoom Room
    {
        get { return _room; }
        set { _room = value; }
    }


    public void Initialize(MazeRoom room)
    {
        room.Add(this);
        transform.GetChild(0).GetComponent<Renderer>().material = room.settings.floorMaterial;
    }
    
    public MazeCellEdge GetEdge(MazeDirection direction)
    { return edges[(int)direction]; }

    public void SetEdge(MazeDirection direction, MazeCellEdge edge)
    { 
        edges[(int)direction] = edge;
        initializedEdgeCount += 1;
    }

    public bool IsFullyInitialized
    { get { return initializedEdgeCount == MazeDirections.Count; } }

    public void OnPlayerEntered()
    {
        Room.Show();

        for (int i = 0; i < edges.Length; i++)
            edges[i].OnPlayerEntered();
    }

    public void OnPlayerExited()
    {
        Room.Hide();

        for (int i = 0; i < edges.Length; i++)
            edges[i].OnPlayerExited();
    }

    public void Hide()
    { gameObject.SetActive(false); }

    public void Show()
    { gameObject.SetActive(true); }

    public MazeDirection RandomUninitializedDirection
    {
        get
        {
            int skips = Random.Range(0, MazeDirections.Count - initializedEdgeCount);
            for (int i = 0; i < MazeDirections.Count; i++)
            {
                if (edges[i] == null)
                {
                    if (skips == 0)
                    {
                        return (MazeDirection)i;
                    }

                    skips -= 1;
                }
            }

            throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
        }
    }
}
