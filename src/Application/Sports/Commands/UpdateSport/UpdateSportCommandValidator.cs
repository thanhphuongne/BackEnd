using BackEnd.Application.Common.Interfaces;

namespace BackEnd.Application.Sports.Commands.UpdateSport;

public class UpdateSportCommandValidator : AbstractValidator<UpdateSportCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateSportCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(v => v.SportName)
            .NotEmpty().WithMessage("Sport name is required.")
            .MaximumLength(100).WithMessage("Sport name must not exceed 100 characters.")
            .MustAsync(BeUniqueName).WithMessage("The specified sport name already exists.");

        RuleFor(v => v.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }

    public async Task<bool> BeUniqueName(UpdateSportCommand model, string name, CancellationToken cancellationToken)
    {
        return await _context.Sports
            .Where(l => l.Id != model.Id)
            .AllAsync(l => l.SportName != name, cancellationToken);
    }
}
