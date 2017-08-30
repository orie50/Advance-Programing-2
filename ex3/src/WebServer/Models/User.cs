using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebServer.Models
{

	/// <summary>
	/// player information
	/// </summary>
	public class User
    {
        public string Id { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public string JoinDate { get; set; }
    }
}