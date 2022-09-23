using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace DogGo.Controllers
{
    public class WalkersController : Controller
    {
        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalkRepository _walkRepo;
        private readonly INeighborhoodRepository _neighborhoodRepo;
        private readonly IOwnerRepository _ownerRepo;
        private readonly IDogRepository _dogRepo;

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public WalkersController(IWalkerRepository walkerRepository,
            IWalkRepository walkRepository,
            INeighborhoodRepository neighborhoodRepository,
            IOwnerRepository ownerRepository,
            IDogRepository dogRepository)
        {
            _walkerRepo = walkerRepository;
            _walkRepo = walkRepository;
            _neighborhoodRepo = neighborhoodRepository;
            _ownerRepo = ownerRepository;
            _dogRepo = dogRepository;
        }
        // GET: HomeController1
        public ActionResult Index()
        {
            
                int userId = GetCurrentUserId();
                Owner owner = _ownerRepo.GetOwnerById(userId);
                List<Walker> walkers = new List<Walker>();
            if(owner != null)
            {
                walkers = _walkerRepo.GetAllWalkers().Where(w => w.NeighborhoodId == owner.NeighborhoodId).ToList();
            }
            else
            {
                walkers = _walkerRepo.GetAllWalkers();
            }
            
                return View(walkers);

        }
        
        // GET: HomeController1/Details/5
        public ActionResult Details(int id)
        {
                Walker walker = _walkerRepo.GetWalkerById(id);
                List<Walk> walks = _walkRepo.GetWalksByWalkerId(id);
                Neighborhood neighborhood = _neighborhoodRepo.GetNeighborhoodById(walker.NeighborhoodId);
            


            WalkerViewModel vm = new WalkerViewModel()
            {
                Walker = walker,
                Walks = walks,
                Neighborhood = neighborhood,
                TotalDuration = TimeSpan.FromSeconds(walks.Sum(w => w.Duration)).ToString(@"hh\:mm")
            };

                if (walker == null)
                {
                    return NotFound();
                }

                return View(vm);
        }

        // GET: HomeController1/Create
        public ActionResult Create()
        {
            return View();
        }

       

        // POST: HomeController1/Create
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
        // GET: HomeController1/Create
        public ActionResult CreateWalk()
        {
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAll();
            List<Dog> dogs = _dogRepo.GetAllDogs();
            List<int> dogIds = dogs.Select(d => d.Id).ToList();

            List<Walker> walkers = _walkerRepo.GetAllWalkers();

            WalkFormViewModel vm = new WalkFormViewModel()
            {
                Walkers = walkers,
                Neighborhoods = neighborhoods,
                DogIds = dogIds
        };

            return View(vm);
        }



        // POST: HomeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateWalk(List<int> dogIds)
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
        // GET: HomeController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController1/Edit/5
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

        // GET: HomeController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController1/Delete/5
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

        private int GetCurrentUserId()
        {
            try
            {
                string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return int.Parse(id);
            }
           catch(Exception)
            {
                int id = 0;
                return id;
            }
            
        }
    }
}
