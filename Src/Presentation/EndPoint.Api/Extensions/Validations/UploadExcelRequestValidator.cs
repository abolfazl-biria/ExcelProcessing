using EndPoint.Api.Models;
using FluentValidation;

namespace EndPoint.Api.Extensions.Validations;

public class UploadExcelRequestValidator : AbstractValidator<UploadExcelRequest>
{
    public UploadExcelRequestValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("No file uploaded.")
            .NotEmpty().WithMessage("File cannot be empty.")
            .Must(file => file.Length > 0).WithMessage("File must have content.")
            .Must(file => file.ContentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" or "application/vnd.ms-excel")
            .WithMessage("File must be in a valid Excel format (XLSX or XLS).");
    }
}