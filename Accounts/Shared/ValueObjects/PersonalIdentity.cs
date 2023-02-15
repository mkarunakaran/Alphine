using Sumday.BoundedContext.SharedKernel.ValueObjects;
using System.Collections.Generic;

namespace Sumday.BoundedContext.ShareHolder.Shared.ValueObjects
{
    public class PersonalIdentity : Identity
    {
        public PersonalIdentity(Ssn ssn, EmailAddress email, Name fullName, Gender gender, BirthDate dateOfBirth, Address address, Phone dayPhone, Phone eveningPhone = null)
          : base(ssn, fullName, address, dayPhone, eveningPhone)
        {
            var names = ParseName(fullName);
            this.FirstName = names.FirstName;
            this.LastName = names.LastName;
            this.DateOfBirth = dateOfBirth;
            this.Email = email;
            this.Gender = gender;
        }

        public Name FirstName { get; private set; }

        public Name LastName { get; private set; }

        public EmailAddress Email { get; private set; }

        public BirthDate DateOfBirth { get; }

        public Gender Gender { get; private set; }

        public Ssn Ssn => this.Tin as Ssn;

        public void ChangeGender(Gender gender)
        {
            this.Gender = gender;
        }

        public void ChangeEmail(EmailAddress email)
        {
            this.Email = email;
        }

        public void ChangeName(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            {
                return;
            }

            if (string.IsNullOrEmpty(firstName))
            {
                this.FullName = lastName;
                this.LastName = lastName;
            }

            if (string.IsNullOrEmpty(lastName))
            {
                this.FullName = firstName;
                this.FirstName = firstName;
            }

            // If either the first or last name contains a space in it,
            // we store it with two spaces between so that it can be parsed.
            if (firstName.Contains(' ') || lastName.Contains(' '))
            {
                this.FullName = string.Concat(firstName, ' ', ' ', lastName); // Double space
            }

            this.FullName = string.Concat(firstName, ' ', lastName); // Single space
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return base.GetEqualityComponents();
            yield return this.Email;
            yield return this.DateOfBirth;
            yield return this.Gender;
        }
    }
}
