using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace App.Domain.DTOs
{
    /// <summary>
    /// Represents a user model.
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        [MinLength(3, ErrorMessage = "Must be at least 3 characters")]
        [MaxLength(50, ErrorMessage = "It must have a maximum of 50 characters.")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        [EmailAddress(ErrorMessage = "Invalid email")]
        [DataType(DataType.EmailAddress)]
        [MinLength(5, ErrorMessage = "Must be at least 5 characters")]
        public string Email { get; set; } = default!;

        /// <summary>
        /// Gets or sets the user age.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        [Range(18, 60, ErrorMessage = "Age range not accepted")]
        public int Age { get; set; }

        /// <summary>
        /// Gets the user's name and age.
        /// </summary>
        public string NameAge => $"{Name} - {Age}";
    }
}
