﻿using System.Text.Json.Serialization;
using PrivateLibrary.Util;

namespace PrivateLibrary.Models.Books
{
    public class AuthorModel
    {
        public Guid? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? DateOfDeath { get; set; }
    }
}
