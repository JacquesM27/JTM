namespace JTM.Services.BrokerService.BrokerSenderStrategy
{
    public interface IBrokerSender
    {
        void PublishMessage(string message);
    }
}
