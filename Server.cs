using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System;
public class Server : MonoBehaviour
{   
    public InputField PortInput;
    List<ServerClient> clients;
    
    List<ServerClient> disconnectList;
    TcpListener server;
    bool ServerStarted;
    public void ServerCreate(){
        clients =new List<ServerClient>();
        disconnectList=new List<ServerClient>();
        try{
            int port=PortInput.text==""?7777:int.Parse(PortInput.text);
            server=new TcpListener(IPAddress.Any,port);
            server.Start();
            StartListening();
            ServerStarted=true;
            Chat.instance.ShowMessage($"서버가 {port}에서 시작되었습니다.");
        }catch(Exception e){
            Chat.instance.ShowMessage($"Socket error:{e.Message}");
        }
    }
    void Update()
    {
       
        if(!ServerStarted)return;
        foreach(ServerClient c in clients){
            if(!IsConnected(c.tcp)){
                c.tcp.Close();
                disconnectList.Add(c);
                continue;
            }
            else{
                NetworkStream s =c.tcp.GetStream();
                if(s.DataAvailable){
                    string data =new StreamReader(s,true).ReadLine();
                    if(data!=null){
                        OnIncomingData(c,data);
                    }
                }
            }
        }
        for (int i = 0; i < disconnectList.Count - 1; i++)
		{
            Broadcast($"{disconnectList[i].clientName} 연결이 끊어졌습니다", clients);

            clients.Remove(disconnectList[i]);
            disconnectList.RemoveAt(i);
		}
    }
    	bool IsConnected(TcpClient c)
	{
        try
        {
            if (c != null && c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);

                return true;
            }
            else
                return false;
        }
        catch 
        {
            return false;
        }
	}
    void StartListening()
	{
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
	}
       void AcceptTcpClient(IAsyncResult ar) 
    {
        TcpListener listener = (TcpListener)ar.AsyncState;
        clients.Add(new ServerClient(listener.EndAcceptTcpClient(ar)));
        StartListening();
        Broadcast("%NAME", new List<ServerClient>() { clients[clients.Count - 1] });
    }
      void OnIncomingData(ServerClient c, string data)
    {
        if (data.Contains("&NAME")) 
        {
            c.clientName = data.Split('|')[1];
            Broadcast($"{c.clientName}이 연결되었습니다", clients);
            return;
        }

        Broadcast($"{c.clientName} : {data}", clients);
    }
    void Broadcast(string data, List<ServerClient> cl) 
    {
        foreach (var c in cl) 
        {
            try 
            {
                StreamWriter writer = new StreamWriter(c.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch (Exception e) 
            {
                Chat.instance.ShowMessage($"쓰기 에러 : {e.Message}를 클라이언트에게 {c.clientName}");
            }
        }
    }

}
public class ServerClient
{
    public TcpClient tcp;
    public string clientName;
    public ServerClient(TcpClient ClientSocket){
        clientName="Guest";
        tcp=ClientSocket;
    }
}