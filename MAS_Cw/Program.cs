using System;
using ActressMas;

namespace MAS_Cw
{
    class Program
    {
        static void Main(string[] args)
        {
            var env = new EnvironmentMas(randomOrder: false, parallel: false);
            int No_house = new Random().Next(5, 13);
            //int No_house = 6;

            for (int i = 1; i <= No_house; i++) {
                var HouseAgent = new BaseAgent();
                env.Add(HouseAgent, "House"+i);
                Console.WriteLine($"{HouseAgent.Name} ready");
                           
            }

            var Enviroment = new EnvironmentAgent();
            env.Add(Enviroment, "Government");

            Console.WriteLine($"{Enviroment.Name} ");

            var auction = new Broker(); env.Add(auction, "auction");
            Console.WriteLine($"{auction.Name} ready");
            env.Start();

            
            Console.WriteLine("Enviroment set up done");
            Console.ReadLine();
        }
    }
}
