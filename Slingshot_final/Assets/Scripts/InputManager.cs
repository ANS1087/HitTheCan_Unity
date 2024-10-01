using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

using System;
// using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json.Linq;
public class InputManager : MonoBehaviour
{
    public SlingShot func;

    public bool grab = false;
    public bool isGrabbed = false;
    public Vector3 initialPosition;
    public Vector3 currentPosition;
    public bool right = false;
    public bool left = false;
    public bool up = false;
    public bool down = false;
    public float slowcontrol;
    public float DepthControl;

    //public float KeyMoveSpeed = 1f;
    //private Camera mainCamera;
    //public bool use3DInput = false;\
    private UdpClient udpClient;
    private int udpPort = 1122;
    Vector3 receivedVector;


    public Vector3 input3DCoordinates;
    public Vector3 firststate;
    public Vector3 new_position;

    private PlayerInput playerInput;
    private Main_Controls main_Controls;

    void Start()
    {
        Debug.Log("UDP Initialized");
        try
        {
            udpClient = new UdpClient(udpPort);
            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
            Debug.Log("UDP BeginReceive called");
        }
        catch (Exception ex)
        {
            Debug.LogError("UDP Initialization failed: " + ex.Message);
        }
        //mainCamera = Camera.main;
    }

    void Update()
    {
        Handle_depth();
        Handle_v2_composite();
        UpdateGrabState(grab);
        Handle3DInput();
    }

    public void UpdateGrabState(bool newGrabState)
    {
        // Check if the grab state has changed
        if (newGrabState != isGrabbed)
        {
            isGrabbed = newGrabState;
            firststate = input3DCoordinates;
        }
    }
    public void Fgrab()
    {
        //use3DInput = true;
        grab = !grab;
    }
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        main_Controls = new Main_Controls();
        main_Controls.Enable();
    }
    public void Handle_v2_composite()
    {
        Vector2 wasd = main_Controls.MAIN.DrawToMovement.ReadValue<Vector2>();
        input3DCoordinates = new Vector3(
            input3DCoordinates.x + (wasd.x * slowcontrol),
            input3DCoordinates.y + (wasd.y * slowcontrol),
            input3DCoordinates.z);
    }
    public void Handle_depth()
    {
        Vector2 depth = main_Controls.MAIN.DepthMovement.ReadValue<Vector2>();
        input3DCoordinates = new Vector3(
            input3DCoordinates.x,
            input3DCoordinates.y,
            input3DCoordinates.z + (depth.y * DepthControl));
    }
    void Handle3DInput()
    {
        if (grab)
        {
            new_position = input3DCoordinates - firststate;

            Debug.Log(new_position);
            initialPosition = func.DrawFrom.position;
            currentPosition = new Vector3(
                func.DrawFrom.position.x + new_position.x * 8,
                func.DrawFrom.position.y + new_position.y * 10,
                func.DrawFrom.position.z + new_position.z * 7
            );

            // Update the drawTo position based on x and y coordinates
            func.DrawTo.position = new Vector3(
                func.DrawFrom.position.x + new_position.x * 10,
                func.DrawFrom.position.y + new_position.y * 12,
                func.DrawTo.position.z
            );
        }
    }
    void ReceiveCallback(IAsyncResult ar)
    {
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, udpPort);

        // Receive data
        byte[] data = udpClient.EndReceive(ar, ref ipEndPoint);

        // Convert data to string (assuming UTF-8 encoding)
        string receivedText = Encoding.UTF8.GetString(data);

        grab = (bool)JObject.Parse(receivedText)["state"];
        float x = (float)JObject.Parse(receivedText)["centroid"][0] / 1000;
        float y = (float)JObject.Parse(receivedText)["centroid"][1] / 1000;
        float z = (float)JObject.Parse(receivedText)["centroid"][2] / 1000;


        // Create the 3D vector
        input3DCoordinates = new Vector3(-x, -y, z);

        // Log or use the state as needed
        Debug.Log("receivedText");
        Debug.Log(input3DCoordinates);

        udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
    }

    /*
    void MouseControl()
    {
        if (UnityEngine.Input.GetMouseButtonDown(0)) // Grab
        {
            grab = true;
            initialPosition = GetMouseWorldPosition();
        }

        if (UnityEngine.Input.GetMouseButtonUp(0)) // Release
        {
            grab = false;
        }
        currentPosition = GetMouseWorldPosition();

        HandleKeyInput();
    }
    Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = UnityEngine.Input.mousePosition;
        mouseScreenPosition.z = mainCamera.WorldToScreenPoint(func.DrawFrom.position).z;
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }
    void HandleKeyInput()
    {
        if (UnityEngine.Input.GetKey(KeyCode.D))
        {
            MoveDrawTo(Vector3.right);
        }
        if (UnityEngine.Input.GetKey(KeyCode.A))
        {
            MoveDrawTo(Vector3.left);
        }
        if (UnityEngine.Input.GetKey(KeyCode.W))
        {
            MoveDrawTo(Vector3.up);
        }
        if (UnityEngine.Input.GetKey(KeyCode.S))
        {
            MoveDrawTo(Vector3.down);
        }
    }
    void MoveDrawTo(Vector3 direction)
    {
        func.DrawTo.position += KeyMoveSpeed * Time.deltaTime * direction;
    }
    */
}
