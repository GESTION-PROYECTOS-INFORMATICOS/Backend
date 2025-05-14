using backGestion.Models;
namespace backGestion.Interfaces;
public interface IRequestService
{
    Task<string> CreateRequestAsync(Request r); //enviar solicitud
    // Task<string> ApproveRequestAsync(); //Aprobar la solicitud
    // Task<string> RejectRequestAsync(); //Rechazar la solicitud
    // // Task<string> UpdateStatusAsync(); //Cambiar a cualquier estado arbitrario si es necesario
    // Task<string> GetRequestByIdAsync(string id); //Consultar una solicitud específica
    // Task<IEnumerable<string>> GetRequestsByStatusAsync(string status); //Consultar solicitudes pendientes

    // Futuro:
    // Task<string> ModifyRequestAsync(); 
}
