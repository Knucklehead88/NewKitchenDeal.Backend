using Core.Entities.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ResponsePersonalInfoDto
    {
            
            [Required]
            public string FirstName { get; set; }

            [Required]
            public string LastName { get; set; }

            public List<LocationDto> Locations { get; set; }

            [Phone]
            public string WhatssAppNumber { get; set; }
            
            [Phone]
            public string PhoneNumber { get; set; }
            public string FacebookProfile { get; set; }
            public string TwitterProfile { get; set; }
            public string TikTokProfile { get; set; }
            public bool CanReceiveTextMessages { get; set; }
            public string ProfilePicture { get; set; }

    }
}
