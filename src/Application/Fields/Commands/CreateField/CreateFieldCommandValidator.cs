using BackEnd.Application.Common.Interfaces;

namespace BackEnd.Application.Fields.Commands.CreateField;

public class CreateFieldCommandValidator : AbstractValidator<CreateFieldCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateFieldCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.SportId)
            .NotEmpty().WithMessage("Sport ID is required.")
            .MustAsync(SportExists).WithMessage("The specified sport does not exist.");

        RuleFor(v => v.FieldName)
            .NotEmpty().WithMessage("Field name is required.")
            .MaximumLength(100).WithMessage("Field name must not exceed 100 characters.")
            .MustAsync(BeUniqueFieldName).WithMessage("The specified field name already exists for this sport.");

        RuleFor(v => v.Location)
            .MaximumLength(200).WithMessage("Location must not exceed 200 characters.");

        RuleFor(v => v.City)
            .MaximumLength(100).WithMessage("City must not exceed 100 characters.");

        RuleFor(v => v.District)
            .MaximumLength(100).WithMessage("District must not exceed 100 characters.");

        RuleFor(v => v.Address)
            .MaximumLength(300).WithMessage("Address must not exceed 300 characters.");

        RuleFor(v => v.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(v => v.FieldType)
            .MaximumLength(50).WithMessage("Field type must not exceed 50 characters.");

        RuleFor(v => v.ImageUrl)
            .MaximumLength(500).WithMessage("Image URL must not exceed 500 characters.");

        RuleFor(v => v.OwnerId)
            .NotEmpty().WithMessage("Owner ID is required.")
            .MustAsync(OwnerExists).WithMessage("The specified owner does not exist.");

        RuleFor(v => v.Capacity)
            .GreaterThan(0).When(v => v.Capacity.HasValue)
            .WithMessage("Capacity must be greater than 0.");
    }

    public async Task<bool> SportExists(int sportId, CancellationToken cancellationToken)
    {
        return await _context.Sports
            .AnyAsync(s => s.Id == sportId, cancellationToken);
    }

    public async Task<bool> OwnerExists(int ownerId, CancellationToken cancellationToken)
    {
        return await _context.CustomUsers
            .AnyAsync(u => u.Id == ownerId && u.Role == "FieldOwner", cancellationToken);
    }

    public async Task<bool> BeUniqueFieldName(CreateFieldCommand model, string fieldName, CancellationToken cancellationToken)
    {
        return await _context.Fields
            .AllAsync(f => f.SportId != model.SportId || f.FieldName != fieldName, cancellationToken);
    }
}
