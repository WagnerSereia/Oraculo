using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oraculo
{
    class Program
    {
        private const string RedisConnectionString = "localhost";
        private static ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(RedisConnectionString);

        private const string ChatChannel = "perguntas";
        private static string oraculo_group = string.Empty;

        static void Main()
        {
            oraculo_group = "wagner_group";

            // Create pub/sub
            var pubsub = connection.GetSubscriber();

            // Subscriber subscribes to a channel
            pubsub.Subscribe(ChatChannel, (channel, message) => MessageAction(message));

            // Notify subscriber(s) if you're joining
            pubsub.Publish(ChatChannel, $": {oraculo_group} entrou esta participando do grupo de oraculos.");

            // Messaging here
            while (true)
            {
                pubsub.Publish(ChatChannel, $"{oraculo_group}: {Console.ReadLine()}");
            }
        }

        static void MessageAction(string message)
        {
            int initialCursorTop = Console.CursorTop;
            int initialCursorLeft = Console.CursorLeft;

            Console.MoveBufferArea(0, initialCursorTop, Console.WindowWidth,
                1, 0, initialCursorTop + 1);
            Console.CursorTop = initialCursorTop;
            Console.CursorLeft = 0;

            // Print the message here
            var PerguntaNumero = message.ToString().Substring(0, message.ToString().IndexOf(":"));
            var Pergunta = message.ToString().Substring(message.ToString().IndexOf(":") + 1); ;
            Console.WriteLine(Pergunta);

            Console.CursorTop = initialCursorTop + 1;
            Console.CursorLeft = initialCursorLeft;
        }
    }
}
