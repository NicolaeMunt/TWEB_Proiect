﻿using System;

namespace Domain.Entities.User
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}