using Core.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class PersonalInfoDto
    {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            
            [Required]
            public string FirstName { get; set; }

            [Required]
            public string LastName { get; set; }

            public LocationDto Location { get; set; }

            [RegularExpression("^(\\+\\d{1,2}\\s?)?\\(?\\d{3}\\)?[\\s.-]?\\d{3}[\\s.-]?\\d{4}$")]
            public string WhatssAppNumber { get; set; }

            [RegularExpression("^(\\+\\d{1,2}\\s?)?\\(?\\d{3}\\)?[\\s.-]?\\d{3}[\\s.-]?\\d{4}$")]
            public string PhoneNumber { get; set; }
            public string FacebookProfile { get; set; }
            public string TwitterProfile { get; set; }
            public string TikTokProfile { get; set; }
            public string ProfilePicture { get; set; }

    }
}
