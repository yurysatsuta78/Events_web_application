namespace Application.DTO
{
    public class ValidationExceptionDto
    {
        public string PropertyName { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
