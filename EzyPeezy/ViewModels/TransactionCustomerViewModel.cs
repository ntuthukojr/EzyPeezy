using System.Collections.Generic;
using EzyPeezy.Models;

namespace EzyPeezy.ViewModels
{
    /// <summary>
    /// For displaying a list of all Customers on the Transaction Checkout page.
    /// </summary>
    public class TransactionCustomerViewModel
    {
        public Transaction Transaction { get; set; }
        public int? PickedCustomerID { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
    }
}