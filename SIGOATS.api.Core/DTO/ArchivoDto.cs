﻿namespace SIGOATS.api.Core.DTO
{
    public class ArchivoDto
    {
        public string? FileName { get; set; }
        public string? FileExtension { get; set; }
        public byte[]? File { get; set; }
    }
}
