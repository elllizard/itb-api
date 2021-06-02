using System;
using Microsoft.Extensions.Configuration;

namespace itb_api.Models.Configurtations
{
    public class DatabaseConfiguration
    {
        public string Url { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }
    }
}