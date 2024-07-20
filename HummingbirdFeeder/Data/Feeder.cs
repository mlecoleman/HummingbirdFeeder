using System;
namespace HummingbirdFeeder.Data
{
    public class Feeder
	{
		public int FeederId { get; set;}
		public string? FeederName { get; set; }
		public int Zipcode { get; set; }
		public int LastChangeDate { get; set; }
        public bool ChangeFeeder { get; set; }
    }
}
