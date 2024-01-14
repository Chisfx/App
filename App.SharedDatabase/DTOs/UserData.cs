using Bogus.Extensions;
using System.Collections;
namespace App.SharedTest.DTOs
{
    public class UserData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new UserFaker().Generate(),
                0
            };// No errors expected

            yield return new object[]
            {
                new UserFaker(
                    k => k.Name.FullName().ClampLength(min: 51, max: 100)
                    ).Generate(),
                1
            };// Error in Name

            yield return new object[]
            {
                new UserFaker(
                    k => k.Name.FullName().ClampLength(min: 51, max: 100),
                    k => k.Name.FirstName()
                    ).Generate(),
                2
            };// Error in Name and Email

            yield return new object[]
            {
                new UserFaker(
                    k => k.Name.FullName().ClampLength(min: 51, max: 100),
                    k => k.Name.FirstName(),
                    k => k.Random.Int(61, 100)
                    ).Generate(),
                3
            };// Error in Name, Email and Age
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
