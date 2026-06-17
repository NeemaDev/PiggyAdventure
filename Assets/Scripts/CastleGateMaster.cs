using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CastleGateMaster : MonoBehaviour
{
    public GameObject gate;

    Dictionary<string, bool> buttonStates = new Dictionary<string, bool>()
    {
        {"left", false},
        {"right", false}
    };
    
    public void RegisterButtonPressed(string id, bool state)
    {
        Debug.Log($"Event handler triggered with id: {id} and state {state}");

        if (buttonStates.ContainsKey(id))
        {
            buttonStates[id] = state;
        }

        Debug.Log($"Left Button State: {buttonStates["left"]}");
        Debug.Log($"Right Button State: {buttonStates["right"]}");

        if(buttonStates.All(button => button.Value))
        {
            OpenGate();
        }
    }

    private void OnEnable()
    {
        GateButton.OnButtonStateChanged += RegisterButtonPressed;
    }

    private void OnDisable()
    {
        GateButton.OnButtonStateChanged -= RegisterButtonPressed;       
    }

    private void OpenGate()
    {
        if(gate != null && gate.activeInHierarchy)
        {
            gate.SetActive(false);
        }
    }
}
