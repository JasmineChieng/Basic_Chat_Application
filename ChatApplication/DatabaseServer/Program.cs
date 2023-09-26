using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;


namespace DatabaseServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Database server");
            //This is the actual host service system
            ServiceHost host;
            //This represents a tcp/ip binding in the Windows network stack
            NetTcpBinding tcp = new NetTcpBinding();
            tcp.MaxReceivedMessageSize = 2147483647; // Set your desired size here
            //Bind server to the implementation of DataServer
            host = new ServiceHost(typeof(ServerInterfaceImpl));
            /*Present the publicly accessible interface to the client. 0.0.0.0 tells .net to
            accept on any interface. :8100 means this will use port 8100. DataService is a name for the
            actual service, this can be any string.*/

            host.AddServiceEndpoint(typeof(ServerInterface), tcp, "net.tcp://0.0.0.0:8100/DataService");
            //And open the host for business!
            host.Open();
            Console.WriteLine("System Online");
            Console.ReadLine();
            //Don't forget to close the host after you're done!
            host.Close();
        }
    }
}
