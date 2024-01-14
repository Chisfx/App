using App.Domain.DTOs;
using Bogus;
using Bogus.Extensions;
namespace App.SharedTest.DTOs
{
    public class UserFaker : Faker<UserModel>
    {
        public UserFaker()
        {
            RuleFor(x => x.Id, f => f.Random.Int(1, 1000));
            RuleFor(c => c.Name, k => k.Name.FullName().ClampLength(min: 3, max: 50));
            RuleFor(c => c.Email, (k, a) => k.Internet.Email(a.Name).ClampLength(min: 5));
            RuleFor(c => c.Age, k => k.Random.Int(18, 60));
        }
        public UserFaker(Func<Faker, object> setterName)
        {
            RuleFor(x => x.Id, f => f.Random.Int(1, 1000));
            RuleFor(c => c.Name, setterName);
            RuleFor(c => c.Email, (k, a) => k.Internet.Email(a.Name).ClampLength(min: 5));
            RuleFor(x => x.Age, f => f.Random.Int(18, 60));
        }
        public UserFaker(Func<Faker, object> setterName, Func<Faker, object> setterEmail)
        {
            RuleFor(x => x.Id, f => f.Random.Int(1, 1000));
            RuleFor(c => c.Name, setterName);
            RuleFor(c => c.Email, setterEmail);
            RuleFor(x => x.Age, f => f.Random.Int(18, 60));
        }
        public UserFaker(Func<Faker, object> setterName, Func<Faker, object> setterEmail, Func<Faker, object> setterAge)
        {
            RuleFor(x => x.Id, f => f.Random.Int(1, 1000));
            RuleFor(c => c.Name, setterName);
            RuleFor(c => c.Email, setterEmail);
            RuleFor(x => x.Age, setterAge);
        }
    }
}
