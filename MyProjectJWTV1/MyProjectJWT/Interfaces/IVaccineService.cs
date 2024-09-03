using MyProjectJWT.DTO;
using MyProjectJWT.Models;

namespace MyProjectJWT.Interfaces
{
    public interface IVaccineService
    {
        List<VaccineDTO> GetAllVaccines();
        VaccineDTO GetVaccineById(int id);
        VaccineDTO AddVaccine(CreateVaccineDTO createVaccine);
        VaccineDTO UpdateVaccine(VaccineDTO vaccineDTO);
        bool DeleteVaccine(int id);
    }
}
