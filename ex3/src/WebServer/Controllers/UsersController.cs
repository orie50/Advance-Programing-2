using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using WebServer.Models;

namespace WebServer.Controllers
{
    public class UsersController : ApiController
    {
        private UserContext db = new UserContext();

        /// <summary>
        /// return all the users
        /// </summary>
        /// <returns></returns>
        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        /// <summary>
        /// get user by username (id)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(string id, string password)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            if(!(ComputeHash(password).Equals(user.Password)))
            {
                return NotFound();
            }

            user.Password = password;
            return Ok(user);
        }

        /// <summary>
        /// put user in data base
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(string id, User user)
        {
            // if not valid arguments
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // if the id isn't match to the user
            if (id != user.Id)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            // try to save the changes 
            try
            {
                db.SaveChanges();
            }
            // if the save failed
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// add new user to data base
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult AddUser(User user)
        {
            // find the current date
            user.JoinDate = findDate(DateTime.Today.ToString());
            // if not valid arguments
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // encrypt the password with SHA1
            user.Password = ComputeHash(user.Password);

            // add user to data base
            db.Users.Add(user);
            // add rank of the user with 0 wins & 0 losses
            Rank rank = new Rank() { Id = user.Id, JoinDate = user.JoinDate, GamesWon = 0, GamesLost = 0 };
            db.Ranks.Add(rank);
            
            // try to save in data base
            try
            {
                db.SaveChanges();
            }
            // catch if save failed
            catch (DbUpdateException)
            {
                if (UserExists(user.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            // return that user added
            return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
        }
        
        /// <summary>
        /// delete user from data base
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(string id)
        {
            // find user by username
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            // remove and save
            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// check if given user is exist
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool UserExists(string id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// compute hash (SHA1) for string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string ComputeHash(string input)
        {
            SHA1 sha = SHA1.Create();
            byte[] buffer = Encoding.ASCII.GetBytes(input);
            byte[] hash = sha.ComputeHash(buffer);
            string hash64 = Convert.ToBase64String(hash);
            return hash64;
        }

        /// <summary>
        /// split day from string of date (d/m/y format)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private string findDate(string date)
        {
            return date.Split()[0];
        }


    }
}