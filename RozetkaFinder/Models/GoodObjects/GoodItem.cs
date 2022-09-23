using Microsoft.EntityFrameworkCore;

namespace RozetkaFinder.Models.GoodObjects
{
    public class GoodItem
    {
        public int Id { get; set; }
        public int IdGood { get; set; }
        public int Price { get; set; }
        public string Href { get; set; }
        public string UserId { get; set; }
    }
}
