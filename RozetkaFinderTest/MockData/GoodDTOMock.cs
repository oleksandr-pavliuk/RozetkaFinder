using RozetkaFinder.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RozetkaFinderTest.MockData
{
    public class GoodDTOMock
    {
        public static List<GoodDTO> GetGoodDTOs()
        {
           return new List<GoodDTO>
           {
                new GoodDTO
                {
                   Title = "Asus Zenbook",
                   Docket =" dsfsdfsd",
                   Id = "36356356f543",
                   Price = 50000
                }
            
            };

        }
    }
}
