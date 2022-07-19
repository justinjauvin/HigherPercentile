using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HigherPercentile.Models
{
    public class User
    {
        [Key]
        public int? Id { get; set; }
        public String? Username { get; set; }
        
        public String? Password { get; set; }

        [NotMapped]
        public String? ConfirmPassword { get; set; } = ""; //the ="" for the confirmpassword is needed to fill the void when login in

        public User()
        {
        }

        public User(string username, int id)
        {
            Username = username;
            Id = id;
        }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }


    }
}
