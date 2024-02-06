using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ServerCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //DNS (Domain Name System)
            string host = Dns.GetHostName(); //local 컴퓨터의 host name
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            //문지기(listen Socket)
            //address family : ip v4인지 ip v6인지
            //TCP 설정 : Stream && Tcp
            Socket listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //문지기 교육(찾은 주소 연동)
                listenSocket.Bind(endPoint);

                //영업시작
                //backlog : 최대 대기수
                listenSocket.Listen(10);

                while (true)
                {
                    Console.WriteLine("Listening...");

                    //1. 손님 입장 시키기(Session Socket 생성, Session Socket을 통해 client와 대화)
                    //Blocking 함수 : client가 입장 안하면 실행이 멈춤
                    Socket clientSocket = listenSocket.Accept();

                    //2. 받는다.
                    byte[] recvBuff = new byte[1024];
                    int recvBytes = clientSocket.Receive(recvBuff);
                    //받은 데이터(byte)를 문자열로 변환
                    //받은 데이터, 시작인덱스, 몇 바이트인지
                    string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);

                    Console.WriteLine($"[From Client] {recvData}");

                    //3. 전송한다.
                    byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to MMORPG Server !");
                    clientSocket.Send(sendBuff);

                    //4.쫓아낸다.
                    //shutdown : 듣기도 말하기도 싫다고 명시 (??)
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            

        }
    }
}