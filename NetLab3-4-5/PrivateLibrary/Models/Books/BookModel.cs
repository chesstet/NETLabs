﻿using System.ComponentModel.DataAnnotations;

namespace PrivateLibrary.Models.Books
{
    public class BookModel
    {
        public Guid Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public AuthorModel? Author { get; set; }
        public short? Year { get; set; }
        public DirectionModel? Direction { get; set; }
    }
}