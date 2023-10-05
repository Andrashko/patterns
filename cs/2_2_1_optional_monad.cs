using System;
using System.Collections.Generic;
using System.Linq;
namespace Structural.Monads
{
    public class Optional<Type>
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
        )
        {
            if (value == null)
                return Optional<ResultType>.None;
            return func(value).CreateFrom();
        }

        public Optional<ResultType> Bind<ResultType>(
            Func<Type, Optional<ResultType>> func
        )
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
        public static Optional<Type> CreateFrom<Type>(this Type value)
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

        public Address? address;
    }

    public class Address
    {
        public int id;
        public string street;

        public Order? lastOrder;
    }

    public class Order
    {
        public int id;
        public string description;
        public Shipper? shipper;
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
                        id = 22
                    }

            }
        };

        private List<Address> addresses = new List<Address>() {
            new Address()
            {
                id = 22,
                street = "Franka",
                lastOrder = new Order(){
                    id = 333,
                }
            }
        };

        private List<Order> orders = new List<Order>(){
            new Order(){
                id = 333,
                description = "Samsung S21",
                shipper =  new Shipper() {
                    name = "Nova Poshta"
                }
            }
        };

        public Customer GetCustomer(int id)
            => customers
            .Where(customer => customer.id == id)
            .FirstOrDefault();
        public Address GetAddress(int id)
            => addresses
            .Where(address => address.id == id)
            .First();
        public Order GetOrder(int id)
            => orders
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
            => repo.GetCustomer(id).CreateFrom();
        public Optional<Address> GetAddress(int id)
          => repo.GetAddress(id).CreateFrom();
        public Optional<Order> GetOrder(int id)
           => repo.GetOrder(id).CreateFrom();
    }

}