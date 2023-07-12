using PruebaSedemi_00.API.Enums;

namespace PruebaSedemi_00.API.Models
{
    public class ErrorAPI
    {
        public ErrorsEnumAPI ErrorCode { get; set; } = ErrorsEnumAPI.NoError;
        public string? ErrorMsg { get; set; }

    }
}
