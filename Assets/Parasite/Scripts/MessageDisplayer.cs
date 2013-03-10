using UnityEngine;
using System.Collections;
 
// Use this script on a guiText object to have status messages
// Just call messageDisplayerObject.DisplayMessage("hello") and you'll
// get a line of self disappearing messages.
 
public class MessageDisplayer : MonoBehaviour
{
    public ArrayList messages = new ArrayList();
 
    public void DisplayMessage(string message)
    {
        messages.Add(message);
        UpdateDisplay();
        Invoke("DeleteOldestMessage", 5F);
    }
 
    void DeleteOldestMessage()
    {
        if (messages.Count > 0)
        {
            messages.RemoveAt(0);
            UpdateDisplay();
        }
    }
 
    void UpdateDisplay()
    {
        string formattedMessages = "";
 
        foreach (string message in messages)
        {
            formattedMessages += message + "\n";
        }
 
        guiText.text = formattedMessages;
    }
}