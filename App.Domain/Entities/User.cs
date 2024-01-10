namespace App.Domain.Entities
{
    /// <summary>
    /// Represents a user entity.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        public string Email { get; set; } = default!;

        /// <summary>
        /// Gets or sets the user age.
        /// </summary>
        public int Age { get; set; }
    }
}
