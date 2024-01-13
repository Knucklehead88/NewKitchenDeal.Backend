using Core.Entities.Identity;

namespace API.Dtos
{
    public class PersonalInfoDto
    {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public AppUserLocationDto Location { get; set; }
            public string WhatssAppNumber { get; set; }
            public string PhoneNumber { get; set; }
            public string FacebookProfile { get; set; }
            public string TwitterProfile { get; set; }
            public string TikTokProfile { get; set; }
            public string ProfilePicture { get; set; }
            public string AppUserEmail { get; set; }

    }
}
