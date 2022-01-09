using LanguageDictionary.Web.Helper;
using LanguageDictionary.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageDictionary.Web.Controllers
{
    public class HashTypeController : BaseController
    {
        public string hashKey { get; set; } = "dictionary";
        public HashTypeController(RedisService redisService) : base(redisService)
        {
        }

        public IActionResult Index()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            if (db.KeyExists(hashKey))
            {
                db.HashGetAll(hashKey).ToList().ForEach(x =>
                {
                    list.Add(x.Name, x.Value);
                });
            }
            return View(list);
        }
        [HttpGet("GetWord")]
        [HttpGet("/")]
        public IActionResult GetWord()
        {
            return View();
        }
        [HttpPost("GetWord")]
        public IActionResult GetWord(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                var word = db.HashGet(hashKey, key).ToString();
                ViewBag.word = word == null ? Messages.WordNotExist : word;
            }
            else
            {
                ViewBag.word = Messages.EnterWord;
            }
            return View();
        }
        [HttpPost]
        public IActionResult Add(string name, string val)
        {
            db.HashSet(hashKey, name, val);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteItem(string name)
        {
            db.HashDelete(hashKey, name);
            return RedirectToAction("Index");
        }
    }
}
