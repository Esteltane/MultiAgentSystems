using System;
using System.Collections.Generic;
using System.Text;
using ActressMas;

namespace MAS_Cw
{
    class Broker: Agent
    {
        private Dictionary<string, int> _housesSelling;
        private List<string> _houseBuying;
        private int _noHouse;
        private List<string> _done;
        public Broker()
        {
            _housesSelling = new Dictionary<string, int>();
            _houseBuying = new List<string>();
            
            
            
        }


        public override void Act(Message message)
        {
            try
            {
                Console.WriteLine($"\t{message.Format()}");

                string[] content = message.Content.Split(' ');

                switch (content[0])
                {
                    case "Selling":
                        Selling(message.Sender, content[1]);
                        break;
                    case "Buying":
                        Buying(message.Sender);
                        break;
                    case "Done":
                        Done(message.Sender);
                        break;
                    case "Search":
                        Search(message.Sender);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Selling(string house, string cost)
        {
            int price = Int32.Parse(cost);

            _housesSelling.Add(house, price);
            Console.WriteLine($"{house} has just became a seller of electricity");

        }
        
        public void Buying(string house)
        {
            _houseBuying.Add(house);
            int lowest = 15;
            string houselow = "";
            if (_housesSelling.Count == 0)
            {
                Send(house, "NoSellers");
            }
            else
            {
                foreach (var f in _housesSelling)
                {
                    if (f.Value < lowest)
                    {
                        lowest = f.Value;
                        houselow = f.Key;
                    }
                }
                if (_housesSelling.ContainsKey(houselow))
                {
                    Send(houselow, "Sold");
                    Send(house, $"Bought {lowest}");
                }
            }

        }

        public void Done(string house)
        {
            if (_housesSelling.ContainsKey(house))
            {
                _housesSelling.Remove(house);
            }
            else if (_houseBuying.Contains(house))
            {
                _houseBuying.Remove(house);
            }
            else
            {
                return;
            }
        }

        public void Search(string house)
        {
            if(_houseBuying.Count == 0)
            {
                Send(house, "NoBuyers");
            }
        }



    }
}
