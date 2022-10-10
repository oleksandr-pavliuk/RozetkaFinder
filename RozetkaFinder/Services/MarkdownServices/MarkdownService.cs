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

        // Method for getting markdown goods from RozetkaAPI by naming. 
        public async Task<List<GoodDTO>> GetMarkdownsAsync(string naming)
        {
            return await _jsonService.GetMarkdownGoods(naming);
        }

        // Method for subscribtion markdown goods from RozetkaAPI by naming.
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

        // Method for getting all markdowns from markdown subscription data base.
        public async Task<IEnumerable<SubscriptionMarkdown>> GetAllMarkdownsAsync()
        {
            return await _repository.GetAllAsync();
        }

        // Method for cheking markdown count in RozetkaAPI.
        public async Task<bool> CheckMarkdownCountAsync(SubscriptionMarkdown markdown)
        {
            int newCount = await _jsonService.GetMarkdownCount(markdown.Naming);
            return newCount > markdown.Count ? true : false;
        }

        //Method for deleting markdown from data base which was used.
        public async void DeleteMarkdownAsync(SubscriptionMarkdown markdown)
        {
            await _repository.DeleteAsync(markdown);
        }
    }
}
