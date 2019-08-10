using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uwpContacts.Classes {
    public class Contact {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public override string ToString() {
            return $"FirstName: {Name} \n " +
                   $"Email: {Email} \n " +
                   $"Phone Number {Phone}";
        }

    }
}
