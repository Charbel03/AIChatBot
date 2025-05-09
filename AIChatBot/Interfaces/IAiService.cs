namespace AIChatBot.Interfaces
{
    public interface IAiService
    {
        Task<string> StartChat(string question);
        void resetChatMessages();
    }
}
