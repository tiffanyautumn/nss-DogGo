using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DogGo.Models.ViewModels
{
    public class WalkFormViewModel
    {
        public Walk Walk { get; set; }
        public List<Neighborhood> Neighborhoods { get; set; }
        public IEnumerable<SelectListItem> DogIds { get; set; }
        public List<Walker> Walkers { get; set; }   

        //public IEnumerable<SelectListItem> SelectedDogs { get; set; } 

    }
}
