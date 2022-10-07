using Microsoft.EntityFrameworkCore;

namespace RozetkaFinder.Models.GoodObjects
{
    public class SubscribtionGood
    {
        public int Id { get; set; }
        public int IdGood { get; set; }
        public int Price { get; set; }
        public string Href { get; set; }
        public string UserEmail { get; set; }
    }

    public class SubscriptionMarkdown
    {
        public int Id { get; set;  }
        public string Naming { get; set; }
        public int Count { get; set; }
        public string Href { get; set; }
        public string UserEmail { get; set; }
    }
}
