﻿using System.Collections.Generic;

namespace DogGo.Models.ViewModels
{
    public class WalkerViewModel
    {
       public Walker Walker { get; set; } 
       public List <Walk> Walks { get; set; }
       public Neighborhood Neighborhood { get; set; }
       public string TotalDuration { get; set; }
 
    }
}
