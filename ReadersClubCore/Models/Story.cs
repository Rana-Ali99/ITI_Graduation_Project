﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReadersClubCore.Models
{
    public class Story : BaseEntity
    {
        [MaxLength(200)]
        public string Title { get; set; }
        public string Cover { get; set; }
        public string Description { get; set; }
        [RegularExpression(@"^.*\.(pdf|PDF)$", ErrorMessage = "Only PDF files are allowed.")]
        public string File { get; set; }
        [RegularExpression(@"^.*\.(mp3|MP3)$", ErrorMessage = "Only MP3 audios are allowed.")]
        public string Audio { get; set; }
        public string Summary { get; set; }
        public bool IsActive { get; set; }  //Writer
        public long ViewsCount { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public bool IsValid { get; set; } = false;  //Should be true , Admin determine accept story or not
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
    public enum Status
    {
        Pending,
        Approved,
        Rejected
    }
}
