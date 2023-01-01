using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HTTP_Async_Server_Demo
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 80);
            tcpListener.Start();

            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                ProcessClientAsync(client);

            }
        }

        /* We create a new method that is async Task and we paste the code inside the method and every
         * method in the code that has an option to be .SomethingAsync we change it to that and write
         * "await" so it can run asynchronously and that the Task Scheduler could handle it! 
         * 
         * Doing it this way this method passes every Client immediately on a new Task so it could Start
         * working and it immediately gets another client! It does not wait for the Client to finish it's
         * job so it could get another one but manages every new Client, because of the Tasks and the Task
         * Schedulers wonderfull job! */
        public static async Task ProcessClientAsync(TcpClient client)
        {
            using (var stream = client.GetStream())
            {
                byte[] buffer = new byte[1000000];

                var lengthOfRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                string requestString = Encoding.UTF8.GetString(buffer, 0, lengthOfRead);
                Console.WriteLine(requestString);

                Console.WriteLine(new String('=', 70));

                // 1) We are verifying if there is a Cookie in the request that we are receiving
                bool sessionSet = false;
                if (requestString.Contains("sid="))
                {
                    sessionSet = true;
                }

                var html = $"<h1> Hello from my Server! :) You are trying to connect at:{DateTime.Now} </h1>";

                /* We have added an "Set-Cookie" header, which we will receive as an response from the Server! */

                string response = "HTTP/1.1 200 OK \r\n" +
                                  "Server: VelichkoServer 2022 \r\n" +
                                  "Content-Type: text/html; \r\n" +
                                  // 2) If there isn't a cookie we send the cookie and if there is we aren't sending one!
                                  (!sessionSet ? ("Set-Cookie: sid=nxi1891623udxqisdxhqwoe180xe1909fbeqw; HttpOnly; Max-Age=300 \r\n") : String.Empty) +
                                  "Content-Length: " + html.Length + "\r\n" +
                                  "\r\n" +
                                  html + "\r\n";

                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                await stream.WriteAsync(responseBytes);
            }
        }
    }
}