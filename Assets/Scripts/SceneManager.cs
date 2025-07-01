using UnityEngine;

public class SceneManager : MonoBehaviour
{

    public bool ElevatorIsCharged;
    public bool ElevatorIsProgressing;
    public bool exitElevatorIsCharged;
    public bool exitElevatorIsProgressing;


    private void Awake()
    {
        ElevatorIsCharged = true;
        ElevatorIsProgressing = false;
        exitElevatorIsCharged = false;
        exitElevatorIsProgressing = false;;
    }
}
