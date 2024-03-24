namespace Common.Models;

public class ServiceInfo
{
	public string ServiceTag { get; set; }
	public List<string> ServiceUrls { get; set; }
	public ServiceConfigurationStateEnum ServiceConfigurationState { get; set; }
}