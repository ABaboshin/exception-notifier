using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestListener.Config;
using RestListener.Models;
using StackExchange.Redis;

namespace RestListener.Controllers {
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        private readonly IOptions<ListenerOptions> _optionsAccessor;

        public NotificationController(IOptions<ListenerOptions> optionsAccessor)
        {
            _optionsAccessor = optionsAccessor;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Notification item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            try {
                var ip = System.Net.Dns.GetHostEntryAsync(_optionsAccessor.Value.RedisHost).Result;
                var redis = ConnectionMultiplexer.Connect(ip.AddressList[0].ToString());
                var sub = redis.GetSubscriber();
                var json = JsonConvert.SerializeObject(item);
                sub.Publish(_optionsAccessor.Value.ChannelName, json);
            } catch (Exception ex) {
                return BadRequest(ex);
            }
            
            return Json("ok");
        }
    }
}