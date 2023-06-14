using UnityEngine;

public class ReplayFrame
{
    public Vector3 CarLocation { get; private set; }
    public Quaternion CarRotation { get; private set; }
    public NPC PawnToDeactivated { get; private set; }
    public Location LocationToActivate { get; private set; }

    public ReplayFrame(Vector3 carLocation, Quaternion carRotation, NPC pawnToDeactivated, Location locationToActivate)
    {
        CarLocation = carLocation;
        CarRotation = carRotation;
        PawnToDeactivated = pawnToDeactivated;
        LocationToActivate = locationToActivate;
    }
}