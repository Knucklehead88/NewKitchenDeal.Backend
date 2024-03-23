﻿using Core.Entities.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ResponseBusinessInfoTradesDto
    {
        [Required]
        public string BusinessName { get; set; }
        public List<ResponseTradeDto> Trades { get; set; }
        public string HourlyRate { get; set; }
        public string DailyRate { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string VideoPresentation { get; set; }
        public string Projects { get; set; }
        public long StartDateOfWork { get; set; }

    }
}
