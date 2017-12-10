using System;
using System.Net.Sockets;
using System.Threading;

namespace MessageBus
{
    public class Helper
    {
        public static void WaitForPortOpen(int timeout, string host, int port)
        {
            bool open = false;
            while (!open)
            {
                Thread.Sleep(timeout);
                Console.WriteLine("Waiting to open port");
                using (var client = new TcpClient())
                {
                    try
                    {
                        client.ReceiveTimeout = timeout;
                        client.SendTimeout = timeout;
                        var asyncResult = client.BeginConnect(host, port, null, null);
                        var waitHandle = asyncResult.AsyncWaitHandle;
                        try
                        {
                            if (!asyncResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(timeout), false))
                            {
                                // wait handle didn't came back in time
                                client.Close();
                            }
                            else
                            {
                                // The result was positiv
                                open = client.Connected;
                            }
                            // ensure the ending-call
                            client.EndConnect(asyncResult);
                        }
                        finally
                        {
                            // Ensure to close the wait handle.
                            waitHandle.Close();
                        }
                    }
                    catch
                    {
                    }
                }
            }
            Console.WriteLine("Port open");
        }
    }
}