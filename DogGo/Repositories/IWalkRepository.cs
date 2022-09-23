using System.Collections.Generic;
using DogGo.Models;

namespace DogGo.Repositories
{
    public interface IWalkRepository
    {
        List<Walk> GetAll();
        List<Walk> GetWalksByWalkerId(int walkerId);
    }
}
