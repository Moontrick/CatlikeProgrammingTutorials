using UnityEngine;

public class MazeDoor : MazePassage 
{


    [SerializeField]
    private Transform hinge;

    private static Quaternion normalRotation = Quaternion.Euler(0f, -90f, 0f),
                              mirroredRotation = Quaternion.Euler(0f, 90f, 0f);
    private bool isMirrored;


    private MazeDoor OtherSideDoor
    { get { return otherCell.GetEdge(direction.GetOpposite()) as MazeDoor; } }


    public override void Initialize(MazeCell primary, MazeCell other, MazeDirection direction)
    {
        base.Initialize(primary, other, direction);

        if (OtherSideDoor != null)
        {
            hinge.localScale = new Vector3(-1f, 1f, 1f);
            Vector3 p = hinge.localPosition;
            p.x = -p.x;
            hinge.localPosition = p;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child != hinge)
                child.GetComponent<Renderer>().material = cell.Room.settings.wallMaterial;
        }
    }


    public override void OnPlayerEntered()
    { 
        OtherSideDoor.hinge.localRotation = hinge.localRotation = isMirrored ? mirroredRotation : normalRotation;
        OtherSideDoor.cell.Room.Show();
    }

    public override void OnPlayerExited()
    { 
        OtherSideDoor.hinge.localRotation = hinge.localRotation = Quaternion.identity;
        OtherSideDoor.cell.Room.Hide();
    }


}
