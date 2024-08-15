using UnityEngine;

[CreateAssetMenu(fileName = "CarConfiguration", menuName = "ScriptableObjects/CarConfiguration")]
public class CarConfiguration : ScriptableObject
{
    public float Mass;
    public float Deceleration;
    public float BrakeForce;
    public float Acceleration;
    public float TopSpeed;
    public float BottomSpeed;
    public float RotateSpeed;
}