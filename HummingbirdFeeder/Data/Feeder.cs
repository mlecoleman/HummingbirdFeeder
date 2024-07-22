using System;
using System.ComponentModel.DataAnnotations;

namespace HummingbirdFeeder.Data

{
    public class Feeder
	{
		[Key]
		public int FeederId { get; set;}
		[MaxLength(128)]
		public string? FeederName { get; set; }
		[Required]
		[MaxLength(5)]
        public string Zipcode { get; set; }
        [MaxLength(8)]
        public int LastChangeDate { get; set; }
        public bool? ChangeFeeder { get; set; }
    }
}
