using ActressMas;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAS_Cw
{
    class BaseAgent : Agent
    {
        private int _sellGov;
        private int _powerGen;
        private int _bal;
        private int _pwrNeeded;
        private int _nonRenPwr;
        private int _sparePwr;
        private int selling; 
        private int _initBal;

        //the following is for the second experiment
        private int count;

        public override void Setup()
        {
            _bal = new Random().Next(10, 25);
            
            _powerGen = 0;
            _pwrNeeded = 0;
            _sparePwr = 0;
       
            _sellGov = 0;
            _nonRenPwr = 0;

            _initBal = _bal;
            Send("Government", "start");

            count = 0;
        }

        public override void Act(Message message)
        {
            Console.WriteLine($"\t{message.Format()}");
            string[] Data = message.Content.Split(' ');
            int selling = new Random().Next(1, 9);
            if (Data[0] == "inform")
            {
                _pwrNeeded = int.Parse(Data[1]);
                _powerGen = int.Parse(Data[2]);
                _nonRenPwr = int.Parse(Data[3]);
                _sellGov = int.Parse(Data[4]);
                _sparePwr = _powerGen - _pwrNeeded;

                
                Console.WriteLine($"The current power needed for this household is -> {_pwrNeeded} and the power it makes is -> {_powerGen}, This house also has a bal of {_bal}");
                if (_sparePwr > 0)
                {
                    Console.WriteLine($"This house generates {_sparePwr} in spare power, and as such can become a seller of power");

                    Send("auction", $"Selling {selling}");
                    Send("auction", "Search");
                    Console.WriteLine(" ");
                }
                else if (_sparePwr < 0)
                {

                    Console.WriteLine($"this house is missing {Math.Abs(_sparePwr)} in power and will need to buy from another house or government");
                    Send("auction", "Buying");
                    Console.WriteLine(" ");
                }
                else
                {
                    Console.WriteLine("This houshold has managed to meet its minimun requirements for power and as such cannot sell or buy anymore.");
                    Delete();

                }
            }
            else if (Data[0] == "Sold")
            {
                _bal += selling;
                Console.WriteLine($"{Name} has just sold some of its power");
                _sparePwr--;
                
                if (_sparePwr < 0)
                {
                    Send("auction", "Done");
                    Delete();

                }
                Send("auction", "Search");
            }
            else if (Data[0] == "Bought")
            {
                if (_bal > 0)
                {
                    _bal -= int.Parse(Data[1]);
                    _powerGen++;
                    if (_powerGen < _pwrNeeded)
                    {
                        Send("auction", "Done");
                        Send("auction", "Buying");
                        
                    }
                    else if (_powerGen >= _pwrNeeded)
                    {
                        Console.WriteLine($"{Name} has managed to meet its minimun requirements for power and as such cannot sell or buy anymore.");
                        Send("auction", "Done");
                        Delete();

                    }
                }
                else if (_bal == 0)
                {
                    Send("auction", "Done");
                    Delete();
                }


            }
            else if (Data[0] == "NoSellers")
            {
                Console.WriteLine($"{Name} has had to buy from the government");
                _powerGen++;
                _bal -= _nonRenPwr;
                count++;
                if (_powerGen < _pwrNeeded)
                {
                    Send("auction", "Done");
                    Send("auction", "Buying");
                    
                }
                else if (_powerGen >= _pwrNeeded)
                {
                    Console.WriteLine($"{Name} has managed to meet its minimun requirements for power and as such cannot sell or buy anymore.");
                    Send("auction", "Done");
                    Delete();

                }

            }
            else if (Data[0] == "NoBuyers")
            {
                Console.WriteLine($"{Name} has had to sell some of its power to the government");
                _sparePwr--;
                _bal += _sellGov;

                if (_sparePwr == 0)
                {
                    Send("auction", "Done");
                    Delete();

                }
                else
                {
                    Send("auction", "Search");
                }
            }
            else {
                
            }


           

        }

        private void Delete()
        {
            Console.WriteLine($"{Name} started with a balance of {_initBal} and has ended up with a balance of {_bal}");
            Console.WriteLine($"{Name} bought from the utility comany {count}");
            
            
            
           

            
        }
    }
}
