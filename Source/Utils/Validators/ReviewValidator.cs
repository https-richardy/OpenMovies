using FluentValidation;
using OpenMovies.Models;

namespace OpenMovies.Validators;

public class ReviewValidator : AbstractValidator<Review>
{
    public ReviewValidator()
    {
        RuleFor(review => review.Liked)
        .NotNull()
        .WithMessage("Please provide the Liked status.");

        RuleFor(review => review.Comment)
            .NotEmpty().WithMessage("Comment cannot be empty.")
            .MaximumLength(500).WithMessage("Comment cannot exceed 500 characters.");

        RuleFor(review => review.Classification)
        .IsInEnum()
        .WithMessage("Invalid Classification. Choose from Bad, Average, Good, Excellent.");
    }
}