namespace backGestion.Models;

public class GMDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string UsersCollectionName { get; set; } = null!;
    public string PdfsCollectionName { get; set; } = null!;

    public string RequestsCollectionName { get; set; } = null!;
    public string MallasCollectionName { get; set; } = null!;
    public string AsignaturasCollectionName { get; set; } = null!;

    
}