using backGestion.Models;
public interface IAsignaturaService
{
    public Task<IEnumerable<Asignatura>> GetAsignaturaMallaSemestre(string malla, int semestre);
}