namespace ThreadsAPI.Models;

public class Conta
{
    public int Id { get; set; }
    public string NomeUsuario { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    //public ICollection<Postagem> Postagens { get; set; }
}