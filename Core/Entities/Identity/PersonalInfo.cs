using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Identity
{
    public class PersonalInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<PersonalInfoLocation> Locations { get; set; } = [];
        public string WhatssAppNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string FacebookProfile { get; set; }
        public string TwitterProfile { get; set; }
        public string TikTokProfile { get; set; }
        public string ProfilePicture { get; set; }
        public string AppUserId { get; set; }
        public bool CanReceiveTextMessages { get; set; }
        public AppUser AppUser { get; set; }

    }
}
