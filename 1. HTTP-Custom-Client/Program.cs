using System;
using System.Net.Http;

namespace HTTP_Client_Demo
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            /* Ще накараме C# да направи някаква заявка(Request) и да получи някакъв отговор(Response),
             * тоест C#, нашето приложение ще играе ролята на Клиент! Ние ще направим сега конзолно приложение
             * и ще се правим на "Браузър". */

            //Here we will read the websites URL address.
            string url = Console.ReadLine();

            //We will use "HttPClient" for sending HTTP Requests and receiving HTTP Responses from the Web Server behind the url above.
            HttpClient httpClient = new HttpClient();

            //We will use the method "GetStringAsync()" to do an HTTP Request.
            var html = await httpClient.GetStringAsync(url);

            Console.WriteLine(html);
        } 
    }
}
