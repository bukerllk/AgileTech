﻿using System.Net;

namespace AgileTech_Web.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccess { get; set; } = true;
        
        public List<string> Errors { get; set; }

        public object Result { get; set; }
    }
}
