namespace ModsenFinanceTracker.Domain.Configutaion;

public class AppConfiguration
{
    public const string DefaultCurrency = "Byn";
    public const string DefaultDataFilePath= "data.json";
    public const string DefaultDateFormat = "dd.MM.yyyy";

    public string Currency { get; set; } 
    public string DataFilePath { get; set; }
    public string DateFormat { get; set; }

    public AppConfiguration(string currency, string dataFilePath, string dateFormat)
    {
        Currency = string.IsNullOrEmpty(currency) ? DefaultCurrency : currency;
        DataFilePath = string.IsNullOrEmpty(dataFilePath) ? DefaultDataFilePath : dataFilePath; ;
        DateFormat = string.IsNullOrEmpty(dateFormat) ? DefaultDateFormat : dateFormat;
    }
}
