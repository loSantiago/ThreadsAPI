namespace ThreadsAPI.Models;

public class Postagem
{
    public int Id { get; set; }
    public Conta IdConta { get; set; }
    public string Autor { get; set; }
    public string Titulo { get; set; }
    public string Conteudo { get; set; }
    public DateTime DataCriacao = DateTime.Now;
}