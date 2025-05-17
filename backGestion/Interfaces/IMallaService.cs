using backGestion.Models;
namespace backGestion.Services;
public interface IMallaService
{
    public Task<IEnumerable<Malla>> GetAllMallas();
}