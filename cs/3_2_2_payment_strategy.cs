using System;
using System.Collections.Generic;

namespace Behavioral.PaymentStrategy
{
    class Card
    {
        public string Number;
        public string System;
        public DateTime Expires;

        public double Balance;
        public Card(String System, string Number, DateTime Expires, double Balance)
        {
            this.Number = Number;
            this.System = System;
            this.Expires = Expires;
            this.Balance = Balance;
        }
    }

    class MasterCard : Card
    {
        public MasterCard(string Number, DateTime Expires, double Balance) :
         base("MASTER", Number, Expires, Balance)
        { }
    }

    class Visa : Card
    {
        public Visa(string Number, DateTime Expires, double Balance) :
        base("VISA", Number, Expires, Balance)
        { }
    }


    class Bill
    {
        public double Sum;
        public Bill(double Sum)
        {
            this.Sum = Sum;
        }
    }
    interface IPayment
    {
        public bool Pay(double PaySum, Card CreditCard);
    }

    class PaymentProcessor
    {
        public Dictionary<string, IPayment> strategies;
        public PaymentProcessor()
        { }

        public bool Checkout(Bill bill, Card card)
        {
            if (!strategies.ContainsKey(card.System))
                return false;
            return this.strategies.GetValueOrDefault(card.System).Pay(bill.Sum, card);
        }
    }

    class VisaPayment : IPayment
    {
        public bool Pay(double PaySum, Card CreditCard)
        {
            if (CreditCard.System != "VISA")
                throw new Exception("Wrong System");
            if (DateTime.Now > CreditCard.Expires)
            {
                Console.WriteLine("Expired");
                return false;
            }
            if (PaySum > CreditCard.Balance)
            {
                Console.WriteLine("Not enough money");
                return false;
            }
            CreditCard.Balance -= PaySum;
            Console.WriteLine("Payment by Visa");
            return true;
        }
    }

    class MasterCardPayment : IPayment
    {
        public bool Pay(double PaySum, Card CreditCard)
        {
            if (CreditCard.System != "MASTER")
                throw new Exception("Wrong System");
            if (PaySum > CreditCard.Balance)
            {
                Console.WriteLine("Not enough money");
                return false;
            }
            if (DateTime.Now > CreditCard.Expires)
            {
                Console.WriteLine("Expired");
                return false;
            }
            CreditCard.Balance -= PaySum;
            Console.WriteLine("Payment by MasterCard");
            return true;
        }
    }
}