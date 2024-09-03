using MyProjectJWT.DTO;
using MyProjectJWT.Models;

namespace MyProjectJWT.Interfaces
{
    public interface IChildService
    {
        public List<ChildDTO> GetChildDetails();
        public ChildDTO GetChildById(int id);
        public ChildDTO AddChild(CreateChildDTO createChildDto);
        public ChildDTO UpdateChild(ChildDTO childDto);
        public bool DeleteChild(int id);
    }
}
