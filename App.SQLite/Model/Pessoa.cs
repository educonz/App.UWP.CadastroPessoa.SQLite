using SQLite.Net.Attributes;

namespace App.SQLite.Model
{
    public class Pessoa : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
    }
}
