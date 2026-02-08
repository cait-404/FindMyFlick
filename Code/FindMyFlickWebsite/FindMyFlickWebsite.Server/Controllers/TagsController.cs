using FindMyFlickWebsite.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FindMyFlickWebsite.Server.Controllers
{
    public class TagsController : Controller
    {

        private readonly List<TagsView> _tags;

        //public TagsController()
        //{
        //    //will need to get movie list from api or db here. 

        //    //generated with copilot using the json schema and DTO 
        //    // Seed in-memory sample data matching the DTO shape
        //    _tags = new List<Tags>
        //    {
        //        _tags = new Tags
        //        {
        //            PlotTags = new List<Tags.PlotTag> { new Tags.PlotTag { TagID = 2, TagType = "plot", TagName = "Twist", Upvotes = 12, Downvotes = 2 } },
        //            TriggerTags = new List<Tags.TriggerTag> { new Tags.TriggerTag { TagID = 1, TagType = "trigger", TagName = "Violence", Upvotes = 12, Downvotes = 2 } },
        //            PersonTags = new List<Tags.PersonTag> { new Tags.PersonTag { TagID = 4, TagType = "person", TagName = "Supporting Actor", Upvotes = 12, Downvotes = 2 } }
        //        },
        //    };
        //}


        // GET: TagsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TagsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TagsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TagsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TagsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TagsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TagsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TagsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
