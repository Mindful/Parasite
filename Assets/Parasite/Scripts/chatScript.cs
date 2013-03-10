using UnityEngine;
using System.Collections;

public class chatScript : MonoBehaviour 
{
    public GUISkin myskin;

    private Rect windowRect = new Rect(Screen.width-200, 0, 300, 400);
    private string messBox = "", messageToSend = "", user = "";
	private bool chatting;

    private void OnGUI()
    {
        GUI.skin = myskin;
        if (NetworkPeerType.Disconnected != Network.peerType)
            windowRect = GUI.Window(1, windowRect, windowFunc, "");
    }
	public void Update()
	{
		if (chatting)
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				networkView.RPC("SendMessage", RPCMode.All, user + ": " + messageToSend + "\n");
	            messageToSend = "";
			}
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				chatting = true;
			}
		}
			
	}
    private void windowFunc(int id)
    {
        GUILayout.Box(messBox, GUILayout.Height(300));

        GUILayout.BeginHorizontal();
        messageToSend = GUILayout.TextField(messageToSend);
        if (GUILayout.Button("Send" , GUILayout.Width(75)))
        {
            networkView.RPC("SendMessage", RPCMode.All, user + ": " + messageToSend + "\n");
            messageToSend = "";
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("User:");
        user = GUILayout.TextField(user);

        GUILayout.EndHorizontal();

       // GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }

    [RPC]
    private void SendMessage(string mess)
    {
        messBox += mess;
    }
}
