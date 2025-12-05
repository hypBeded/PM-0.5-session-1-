using System.Data;

namespace DataBaseLogic
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Post {  get; set; }
        public User(string Name, string Email, string Password, string Post)
        {
            this.Name = Name;
            this.Email = Email; 
            this.Password = Password;   
            this.Post = Post;   
        }


    }
}
