using AutoMapper.Configuration.Conventions;

namespace RozetkaFinder.Models
{
    public class Good
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string Link { get; set; }
    }
}
