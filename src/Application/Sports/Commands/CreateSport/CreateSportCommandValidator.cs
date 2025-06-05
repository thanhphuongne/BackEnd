using BackEnd.Application.Common.Interfaces;

namespace BackEnd.Application.Sports.Commands.CreateSport;

public class CreateSportCommandValidator : AbstractValidator<CreateSportCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateSportCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.SportName)
            .NotEmpty().WithMessage("Sport name is required.")
            .MaximumLength(100).WithMessage("Sport name must not exceed 100 characters.")
            .MustAsync(BeUniqueName).WithMessage("The specified sport name already exists.");

        RuleFor(v => v.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.Sports
            .AllAsync(l => l.SportName != name, cancellationToken);
    }
}
