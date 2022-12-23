using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HTTP_Server_Demo
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            /* Сега ние ще играем ролята на Уеб Сървъра, а Клиента ще бъде Браузъра.
             * Тоест ще получаваме Request-и и ще ги обработваме и ще връщаме Response-и! */
            /* Being a Web Server, we should choose a Port and wait for Requests being sent into that Port! */
            /* We will open an TCP Socket and choose a Port and we will begin to wait for other Programs to send us Requests! */

            /* We will use "TcpListener" class to choose a Port! And we will receive everything being sent to that Port! */
            /* "IpAddress.Loopback" means our localhost, our own Machine! We are saying our own Machine will be our 
             * Server Program which on our Local Network we will open Port 80! And we are saying that we want to
             * receive that Port, meaning only we will get the Requests being sent to that Port! */
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 80);

            /* 2nd Step is to Active that Port by saying "tcpListener.Start()"! Above we are only initializing that Port! 
             ".Start()" is saying give me that Port and we Begin to operate on that Port! */
            tcpListener.Start();

            /* Here we are saying while we are receiving Client Requests, get the Request and process it! */
            while (true)
            {
                /* This is the "ReadLine of the TCP's!" and we safe the information into variable and that variable
                 * has different methods which help us communicate with the client! */
                // Here we are Accepting the Client Request!
                TcpClient client = tcpListener.AcceptTcpClient();

                // Get the client's stream, we are using one of the methods that the class "TcpClient" has!
                //This variable helps us because it has everything that we need to communicate with the Client!
                var stream = client.GetStream();


                /* How to read the Request from the Stream */

                //1st - We make a buffer which will store what we have read!
                byte[] buffer = new byte[1000000];

                //2nd - We get the length of the Stream!
                /* Here with stream.Read we say to store the information in the "buffer" starting from 0
                 * and to read the length of buffer.Length! */
                // Този метод, записва Стрийма и ни връща, колко на брой символа е прочел!
                var lengthOfRead = stream.Read(buffer, 0, buffer.Length);

                //We convert the stream into a string!
                string requestString = Encoding.UTF8.GetString(buffer, 0, lengthOfRead);
                Console.WriteLine(requestString);

                Console.WriteLine(new String('=', 70));

                var html = $"<h1> Hello from my Server! :) You are trying to connect at:{DateTime.Now} </h1>";

                // We will send a reponse to the Client which send us a Request! 
                string response = "HTTP/1.1 200 OK\r\n" +
                                  "Server: VelichkoServer 2022 \r\n" +
                                  "Content-Type: text/html;\r\n" +
                                  "Content-Length: " + html.Length + "\r\n" +
                                  "\r\n" +
                                  html;

                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                stream.Write(responseBytes);

                /* 1. До тук си отворихме един порт
                 * 2. Стартирахме си tcpListener-a 
                 * 3. Прочетохме Request-a от Клиента
                 * 4. Взехме Стрийма на Клиента
                 * 5. Прочетохме и записахме във буфер всичко, което клиента ни е дал
                 * 6. След това превърнахме това, което клиента ни е написал от масив от байтове във текст
                 * 7. И накрая изписваме това на Конзолата! */
            }
        }
    }
}