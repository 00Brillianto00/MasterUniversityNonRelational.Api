using MasterUniversityNonRelational.Api.Interfaces;

namespace MasterUniversityNonRelational.Api.Models
{
    public class DBSettings : IDatabaseSettings
    {
        public string CollectionName { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
    }
}
