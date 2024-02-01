using Core.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class PersonalInfoDto
    {
            
            [Required]
            public string FirstName { get; set; }

            [Required]
            public string LastName { get; set; }

            public LocationDto Location { get; set; }

            [Phone]
            public string WhatssAppNumber { get; set; }
            
            [Phone]
            public string PhoneNumber { get; set; }
            public string FacebookProfile { get; set; }
            public string TwitterProfile { get; set; }
            public string TikTokProfile { get; set; }
            public string ProfilePicture { get; set; }

    }
}
