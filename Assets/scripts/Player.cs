using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class Player : Agent
{
    //Declaracion del componente Rigidbody para mover los palos
    Rigidbody rbody;

    //parametros torque del palo, incrementar en Unity si se requiere giros mas rapidos
    public float maxAngVel;

    private Vector3 startingPosition;
    private int score = 0;

    // public event Action OnReset;

    // Start es llamado en el primer frame del juego
    public override void Initialize()
    {
        //conectamos el rbody con el componente rigidbody del palo
        rbody = GetComponent<Rigidbody>();
        //asignamos la velocidad del giro
        rbody.maxAngularVelocity = maxAngVel;
        startingPosition = transform.position;
    }

    // Update es llamado durante cada frame del juego
    public override void OnActionReceived(ActionBuffers acciones)
    {
        // Rotar Reloj
        if (acciones.DiscreteActions[0] == 1)
            // float h = 10000f * Time.deltaTime;
            rbody.AddTorque(transform.forward * 10000f * Time.deltaTime * 1000);
        
        // Rotar Contrarreloj
        if (acciones.DiscreteActions[1] == 1)
            // float h = -10000f * Time.deltaTime;
            rbody.AddTorque(transform.forward * -10000f * Time.deltaTime * 1000);

        // Empujar Palo
        if (acciones.DiscreteActions[2] == 1)
            // float v = 1;
            rbody.AddForce(0,0, 300f);
        
        // Jalar Palo
        if (acciones.DiscreteActions[3] == 1)
            // float v = -1;
            rbody.AddForce(0,0, -300f);
            
        // Ejecutar con las direcciones decididas
        // rbody.AddForce(0,0, v * 300f);
        // rbody.AddTorque(transform.forward * h * 1000);
    }

    public override void OnEpisodeBegin() {
        score = 0;
        // Reset Position
        transform.position = startingPosition;
        rbody.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Pelota"))
            // Incrementar trainingscore por hacer colisión exitosa.
            AddReward(.1f);
    }

    public override void Heuristic(in ActionBuffers acciones){
        var acc = acciones.DiscreteActions;
        // acc = [0,0,0,0];

        if(Input.GetKey("left"))
            acc[0] = 1;
        if(Input.GetKey("right"))
            acc[1] = 1;
        if(Input.GetKey("up"))
            acc[2] = 1;
        if(Input.GetKey("down"))
            acc[3] = 1;
    }
}
