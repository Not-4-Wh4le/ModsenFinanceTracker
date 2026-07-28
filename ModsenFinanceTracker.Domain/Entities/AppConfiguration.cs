using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Domain.Entities
{
    public class AppConfiguration
    {
        public string Currency { get; set; } 
        public string DataFilePath { get; set; }
        public string DateFormat { get; set; }

        public AppConfiguration(string currency, string dataFilePath, string dateFormat)
        {
            Currency = string.IsNullOrEmpty(currency) ? currency : "";
            DataFilePath = string.IsNullOrEmpty(dataFilePath) ? dataFilePath : "data.json";
            DateFormat = string.IsNullOrEmpty(dateFormat) ? dateFormat : "dd.MM.yyyy";
        }
    }
}
