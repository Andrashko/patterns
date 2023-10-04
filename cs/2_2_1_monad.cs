using System;
using System.Collections.Generic;
using System.Linq;
namespace Structural.Monads
{
    public class Optional<Type> where Type : class
    {
        private readonly Type value;

        public readonly static Optional<Type> None = new Optional<Type>();

        public Optional(Type value)
        {
            if (value == null)
                throw new ArgumentNullException("Expected not null object");
            this.value = value;
        }
        private Optional() { }
        public Optional<ResultType> Bind<ResultType>(
            Func<Type, ResultType> func
        ) where ResultType : class
        {
            if (value == null)
                return Optional<ResultType>.None;
            return OptionalExtension.CreateFrom(func(value));
        }

        public Optional<ResultType> Bind<ResultType>(
            Func<Type, Optional<ResultType>> func
        ) where ResultType : class
        {
            if (value == null)
                return Optional<ResultType>.None;
            return func(value);
        }

        public Type GetValue()
        {
            return value;
        }
    }

    public static class OptionalExtension
    {
        public static Optional<Type> CreateFrom<Type>(this Type value) where Type : class
        {
            if (value == null)
                return Optional<Type>.None;
            return new Optional<Type>(value);
        }
    }

    public class Customer
    {
        public int id;
        public string name;

        public Address address;
    }

    public class Address
    {
        public int id;
        public string street;

        public Order lastOrder;
    }

    public class Order
    {
        public int id;
        public string description;
        public Shipper shipper;
    }

    public class Shipper
    {
        public string name;

        public override string ToString()
        {
            return name;
        }
    }

    public interface ITraditionalRepository
    {
        Customer GetCustomer(int id);
        Address GetAddress(int id);
        Order GetOrder(int id);
    }

    public class TraditionalRepository : ITraditionalRepository
    {
        private List<Customer> customers = new List<Customer>() {
            new Customer()
            {
                id = 1,
                name = "Yurii",
                address =
                    new Address()
                    {
                        id = 1,
                        street = "Franka",
                        lastOrder = new Order(){
                            id = 1,
                            description = "Samsung S21",
                            shipper =  new Shipper() {
                                name = "Nova Poshta"
                            }
                        }
                    }

            }
        };

        public Customer GetCustomer(int id)
            => customers
            .Where(customer => customer.id == id)
            .FirstOrDefault();
        public Address GetAddress(int id)
            => customers
            .Select(customer => customer.address)
            .Where(address => address.id == id)
            .First();
        public Order GetOrder(int id)
            => customers
            .Select(customer => customer.address)
            .Select(address => address.lastOrder)
            .Where(order => order.id == id)
            .First();
    }

    public interface IMonadicRepository
    {
        Optional<Customer> GetCustomer(int id);
        Optional<Address> GetAddress(int id);
        Optional<Order> GetOrder(int id);
    }

    public class MonadicRepository : IMonadicRepository
    {
        private TraditionalRepository repo = new TraditionalRepository();
        public Optional<Customer> GetCustomer(int id)
            => OptionalExtension.CreateFrom(repo.GetCustomer(id));
        public Optional<Address> GetAddress(int id)
          => OptionalExtension.CreateFrom(repo.GetAddress(id));
        public Optional<Order> GetOrder(int id)
           => OptionalExtension.CreateFrom(repo.GetOrder(id));
    }

}