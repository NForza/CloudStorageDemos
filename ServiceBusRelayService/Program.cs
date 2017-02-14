using Microsoft.ServiceBus;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusRelayService
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost sh = new ServiceHost(typeof(CalculatorImplementation));

            ListenOnLocalEndpoint(sh);

            //ListenOnServicebusRelayEndpoint(sh);

            sh.Open();
            Console.WriteLine("Press ENTER to close");
            Console.ReadLine();
            sh.Close();
        }

        private static void ListenOnServicebusRelayEndpoint(ServiceHost sh)
        {
            sh.AddServiceEndpoint(
               typeof(ICalculator), new NetTcpRelayBinding(),
               ServiceBusEnvironment.CreateServiceUri("sb", "namespace", "calculator"))
                .Behaviors.Add(new TransportClientEndpointBehavior
                {
                    TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey", "<yourKey>")
                });
        }

        private static void ListenOnLocalEndpoint(ServiceHost sh)
        {
            sh.AddServiceEndpoint(
               typeof(ICalculator), new NetTcpBinding(),
               "net.tcp://localhost:9358/calculator");
        }
    }
}
