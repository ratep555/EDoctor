using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface ISpecialtyRepository
    {
        Task<List<Specialty>> GetAllSpecialties();
        Task<List<Specialty>> GetSpecialtiesAttributedToDoctors();

    }
}