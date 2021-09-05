using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TrailRenderer))]

public class GravitySimulation : MonoBehaviour
{
    private Rigidbody M1;
    private Rigidbody M2;
    private TrailRenderer Trail;

    public static List<GravitySimulation> Simulations;

    public float X_Position;
    public float Y_Position;
    public float Z_Position;

    public float Pitch = 0.0f;
    public float Yaw = 0.0f;
    
    private Vector3 N_Force;
    private float N_Centripetal;
    private float N_Radius;

    private float ForceMagnitude;
    private float Velocity;

    private const float G = 667.4f;

    private void OnEnable()
    {
        if(Simulations == null)
        {
            Simulations = new List<GravitySimulation>();
        }

        Simulations.Add(this);
    }

    private void OnDisable()
    {
        Simulations.Remove(this);
    }

    void Start()
    {
        M1 = GetComponent<Rigidbody>();
        Trail = GetComponent<TrailRenderer>();

        //this.transform.position = new Vector3(X_Position, Y_Position, Z_Position);
        //this.transform.rotation = Quaternion.Euler(Pitch, 0, Yaw);

        Trail.startWidth = 0.5f;
        Trail.endWidth = 0.0f;

        Trail.startColor = new Color(1,1,1,0.1f);
        Trail.endColor = new Color(0,0,0,0);
    }

    private void FixedUpdate()
    {
        foreach (GravitySimulation Simulation in Simulations)
        {
            if (Simulation != this)
            {
                NBodySimulation(Simulation);
            }
        }
    }

    private float Radius(Rigidbody M1, Rigidbody M2)
    {
        return (M1.position - M2.position).magnitude;
    }

    private Vector3 Force(Rigidbody M1, Rigidbody M2, float Radius)
    {
        ForceMagnitude = G * (M1.mass * M2.mass) / Mathf.Pow(Radius, 2);

        return (M1.position - M2.position).normalized * ForceMagnitude;
    }

    private float Cenripetal(Rigidbody M1, Rigidbody M2, float Radius)
    {
        Velocity = Mathf.Sqrt(G * M1.mass * Radius) / (M2.mass * Mathf.Pow(Radius, 2));

        return (M1.mass * Mathf.Pow(Velocity, 2)) / Radius;
    }

    private void NBodySimulation(GravitySimulation OtherMass)
    {
        M2 = OtherMass.M1;

        N_Radius = Radius(M1, M2);

        if(N_Radius == 0)
        {
            return;
        }

        N_Force = Force(M1, M2, N_Radius);

        N_Centripetal = Cenripetal(M1, M2, N_Radius);

        M2.AddTorque(M2.transform.up * -N_Centripetal);

        M2.AddForce(M2.transform.forward * M2.mass);

        M2.AddForce(N_Force);
    }
}
