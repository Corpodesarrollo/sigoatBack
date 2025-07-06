using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.DTO
{
    public class TablerosDto : BaseDto
    {
        public string? Titulo { get; set; }
        public string? Url { get; set; }
        public bool Estado { get; set; }
    }
}
