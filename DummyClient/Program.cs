// See https://aka.ms/new-console-template for more information

//DNS (Domain Name System)
using System.Net;
using System.Net.Sockets;
using System.Text;

string host = Dns.GetHostName(); //local 컴퓨터의 host name
IPHostEntry ipHost = Dns.GetHostEntry(host);
IPAddress ipAddr = ipHost.AddressList[0];
IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);


//1. 휴대폰 설정
Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

try
{
    //2. 입장 문의
    socket.Connect(endPoint);
    Console.WriteLine($"Connected To {socket.RemoteEndPoint.ToString()}");

    //3. 보낸다.
    byte[] sendBuff = Encoding.UTF8.GetBytes("Hello World!");
    int sendBytes = socket.Send(sendBuff);

    //4. 받는다.
    byte[] recvBuff = new byte[1024];
    int recvBytes = socket.Receive(recvBuff);
    string recvData = Encoding.UTF8.GetString(recvBuff);
    Console.WriteLine($"[From Server] {recvData}");

    //5. 나간다.
    socket.Shutdown(SocketShutdown.Both);
    socket.Close();
}
catch(Exception ex)
{
    Console.WriteLine(ex.ToString());
}

