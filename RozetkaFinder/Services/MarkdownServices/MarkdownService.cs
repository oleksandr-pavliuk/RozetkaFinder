using RozetkaFinder.DTOs;
using RozetkaFinder.Models.GoodObjects;
using RozetkaFinder.Repository;
using RozetkaFinder.Services.JSONServices;

namespace RozetkaFinder.Services.MarkdownServices
{
    public interface IMarkdownService
    {
        Task<List<GoodDTO>> GetMarkdownsAsync(string naming);
        Task<bool> SubscribeMarkdownAsync(string naming, string email);
        Task<IEnumerable<SubscriptionMarkdown>> GetAllMarkdownsAsync();
        Task<bool> CheckMarkdownCountAsync(SubscriptionMarkdown markdown);
        void DeleteMarkdownAsync(SubscriptionMarkdown markdown);

    }
    public class MarkdownService : IMarkdownService
    {
        private readonly IJsonService _jsonService;
        private readonly IRepository<SubscriptionMarkdown> _repository;
        public MarkdownService(IJsonService jsonService, IRepository<SubscriptionMarkdown> repository)
        {
            _jsonService = jsonService;
            _repository = repository;
        }
        public async Task<List<GoodDTO>> GetMarkdownsAsync(string naming)
        {
            return await _jsonService.GetMarkdownGoods(naming);
        }

        public async Task<bool> SubscribeMarkdownAsync(string naming, string email)
        {
            SubscriptionMarkdown subscriptionMarkdown = new SubscriptionMarkdown();
            subscriptionMarkdown.Naming = naming;
            subscriptionMarkdown.Href = "https://rozetka.com.ua/search/?text=уцінка+" + naming.Replace(" ", "+");
            subscriptionMarkdown.Count = await _jsonService.GetMarkdownCount(naming);
            subscriptionMarkdown.UserEmail = email;
            await _repository.CreateAsync(subscriptionMarkdown);
            return true;
        }

        public async Task<IEnumerable<SubscriptionMarkdown>> GetAllMarkdownsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<bool> CheckMarkdownCountAsync(SubscriptionMarkdown markdown)
        {
            int newCount = await _jsonService.GetMarkdownCount(markdown.Naming);
            return newCount > markdown.Count ? true : false;
        }

        public async void DeleteMarkdownAsync(SubscriptionMarkdown markdown)
        {
            await _repository.DeleteAsync(markdown);
        }
    }
}
