using LegacyApp.Application.Contracts;
using LegacyApp.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyApp.Application.Extensions
{
    public static class ClientModelExtensions
    {
        public static int GetCreditLimit(this Client client, int creditLimit)
        {
            if (client.Name == "ImportantClient")
            {
                return creditLimit * 2;
            }

            return creditLimit;
        }
    }
}
