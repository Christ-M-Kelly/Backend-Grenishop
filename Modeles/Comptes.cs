using System.Text.Json.Serialization;

namespace BackendGrenishop.Modeles
{
    public class Compte
    {
        public int id_compte { get; set; }
        
        public required string Nom { get; set; }

        [JsonPropertyName("Prénom")]
        public required string Prenom { get; set; }

        public required string Email { get; set; }
        
        public required string MotDePasseHash { get; set; }
        
        public required DateTime Date_inscription { get; set; }
    }
}
