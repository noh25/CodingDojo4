using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CodingDojo4
{
    public class ClientHandler
    {
        Socket clientSocket;
        byte[] buffer = new byte[256];
        Informer informAllClients;

        public ClientHandler(Socket clientSocket, Informer inf)
        {
            this.informAllClients = inf;
            this.clientSocket = clientSocket;
            ThreadPool.QueueUserWorkItem(StartReceive, null);
        }

        internal void SendData(string message)
        {
            clientSocket.Send(Encoding.ASCII.GetBytes(message));
        }

        public void StartReceive(object o)
        {
            int length;

            while (true)
            {
                length = clientSocket.Receive(buffer);
                String data = Encoding.ASCII.GetString(buffer, 0, length); 
                informAllClients(this, data);
                Console.Write(data);


            }
        }
    }
}
