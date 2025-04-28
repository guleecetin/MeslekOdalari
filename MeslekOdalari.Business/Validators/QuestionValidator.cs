using FluentValidation;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.Business.Validators
{
    public class QuestionValidator:AbstractValidator<Quest>
    {
        public QuestionValidator()
        {
            RuleFor(x => x.Question).NotEmpty().WithMessage("Soru Boş Bırakılamaz.");
            RuleFor(x => x.Question).MinimumLength(10).WithMessage("Minimum 10 karakterlik bir soru giriniz");
            RuleFor(x => x.Question).MaximumLength(200).WithMessage("Maksimum 200 karakterlik bir soru giriniz");
            RuleFor(x => x.Answer).NotEmpty().WithMessage("Cevap Boş Bırakılamaz");
            RuleFor(x => x.Answer).MinimumLength(10).WithMessage("Minimum 10 Karakterik bir cevap giriniz");
            RuleFor(x => x.Answer).MaximumLength(200).WithMessage("Maksimum 200 karakterlik bir cevap giriniz");
        }
    }
}
