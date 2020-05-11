using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UdpMeasurementsReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World receiver ready press enter!");
            Console.ReadLine();
            PostService postMeasure = new PostService();
            Measurement.Measurement measurement;


            UdpClient receiveUdp = new UdpClient(11111);

            IPEndPoint receiveEndPoint = new IPEndPoint(IPAddress.Any, 0);
            
            Console.WriteLine("ready to receive data");

            try
            {
                while (true)
                {
                    Byte[] receiveBytes = receiveUdp.Receive(ref receiveEndPoint);
                    string receivedData = Encoding.ASCII.GetString(receiveBytes);
                    
                    Console.WriteLine("This message was sent from " +
                                      receiveEndPoint.Address.ToString() +
                                      " on their port number " +
                                      receiveEndPoint.Port.ToString());

                    Console.WriteLine(receivedData);

                    string[] txtSplit = receivedData.Split(" ");
                    for (int i = 0; i < txtSplit.Length; i++)
                    {
                        Console.WriteLine(txtSplit[i]);
                    }

                   measurement   = new Measurement.Measurement(Convert.ToDateTime(txtSplit[0]+" "+txtSplit[1]) , txtSplit[2], txtSplit[3], Convert.ToDouble(txtSplit[4]) , txtSplit[5], receiveEndPoint.Port.ToString(), receiveEndPoint.Address.ToString(), txtSplit[8]);
                   postMeasure.PostItemHttpTask(measurement);
                   Thread.Sleep(3000);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
