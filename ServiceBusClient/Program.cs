using Microsoft.ServiceBus;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<ICalculator> cf = GetLocalConnection();

            var ch = cf.CreateChannel();
            Console.WriteLine(ch.Add(4, 5));

            Console.ReadLine();
        }

        private static ChannelFactory<ICalculator> GetLocalConnection()
        {
            var cf = new ChannelFactory<ICalculator>(
                                new NetTcpBinding(),
                                new EndpointAddress("net.tcp://localhost:9358/calculator"));

            return cf;
        }

        private static ChannelFactory<ICalculator> GetServiceBusRelayConnection()
        {
            var cf = new ChannelFactory<ICalculator>(
                                new NetTcpRelayBinding(),
                                new EndpointAddress(ServiceBusEnvironment.CreateServiceUri("sb", "namespace", "calculator")));

            cf.Endpoint.Behaviors.Add(new TransportClientEndpointBehavior
            { TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey", "<yourKey>") });
            return cf;
        }
    }
}
