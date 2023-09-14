namespace ConfigHelper.Configs;

public class EmailSettings
{
    public string Smtp { get; set; }
    public int Port { get; set; }
    public string ServiceMailAddress { get; set; }
    public string ServiceMailName { get; set; }
    public string Password { get; set; }
    public bool UseEmail { get; set; }
}