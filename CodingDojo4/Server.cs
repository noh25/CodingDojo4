using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodingDojo4
{
    class Server
    {
        Socket serverSocket;
        List<ClientHandler> clients = new List<ClientHandler>();
        Informer informAllClients;

        public Server()
        {
            informAllClients = SendToAll; //<- kürzere Schreibweise für informAllClients = new Informer(SendToAll);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8090)); //IPAddress.Loopback bringt automatisch 127.0.0.1 .... wenn beliebige Adresse verwendet werden soll, kann IPAddress.Parse("127.x.x.x") verwendet werden)
            serverSocket.Listen(10); //10 ist hier maximale Anzahl an Connections in der Queue
            ThreadPool.QueueUserWorkItem(AcceptClients, null);
        }

        public void AcceptClients(object o)
        {
            while (true)
            {
                clients.Add(new ClientHandler(serverSocket.Accept(), informAllClients));
                clients.Last().SendData("Welcome");
                Console.WriteLine("Client accepted");
            }

        }

        public void SendToAll(ClientHandler sender, string data)
        {
            foreach (var item in clients)
            {
                if (item != sender)
                {
                    item.SendData(data);
                }
            }
        }
    }
}
