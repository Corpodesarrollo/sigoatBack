namespace SIGOATS.api.Core.DTO
{
    public class EmailToSendDto
    {
        public int? Id { get; set; }
        public string? From { get; set; }
        public string? FromMask { get; set; }
        public string[]? To { get; set; }
        public string[]? CC { get; set; }
        public string[]? CCO { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public AttachmentFileDto[]? Attachments { get; set; }
        public DateTime? SentDate { get; set; }
        public int? Retries { get; set; }
        public string? IdUserResend { get; set; }
        public DateTime? ResendDate { get; set; }

        public bool Status { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(From))
                    ErrorMessage = "El campo 'De' es requerido.";
                else if (To == null || To.Length == 0)
                    ErrorMessage = "El campo 'Para' es requerido.";
                else if (string.IsNullOrEmpty(Subject))
                    ErrorMessage = "El campo 'Asunto' es requerido.";
                else if (string.IsNullOrEmpty(Body))
                    ErrorMessage = "El campo 'Cuerpo' es requerido.";

                return !string.IsNullOrEmpty(From) && To != null && To.Length > 0 && !string.IsNullOrEmpty(Subject) && !string.IsNullOrEmpty(Body);
            }
        }
    }
}
