using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TelegramBotNotification
{
    public class JsonService
    {
        public TgToken? Token { get; set; }
        public JsonService()
        {
            using (StreamReader sr = new StreamReader("./token.json"))
            {
                var json = sr.ReadToEnd();
                Token = JsonConvert.DeserializeObject<TgToken>(json);
            }
        }
    }
}
