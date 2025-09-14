namespace Core.DotnetFunctions.AppSettings
{
    public class ServicebusSettings
    {
        public string ServicebusCoreEventsConnectionstring { get; set; }
        public string ServicebusCoreEventsUserIngestionTopicname { get; set; }
        public string ServicebusCoreEventsUserIngestionSubscription { get; set; }
    }
}
