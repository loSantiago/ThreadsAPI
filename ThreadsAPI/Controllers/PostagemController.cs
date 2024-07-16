using Microsoft.AspNetCore.Mvc;
using ThreadsAPI.Context;
using ThreadsAPI.Models;

namespace ThreadsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostagemController : ControllerBase
{
    private readonly ThreadsContext _db;
    
    public PostagemController(ThreadsContext context)
    {
        _db = context;
    }

    [HttpGet("/ObterPostagem/{idPostagem}")]
    public IActionResult ObterUmPost(int idPostagem)
    {
        var post = _db.Postagens.Find(idPostagem);
        
        //if null, not found.
        if(post is null) return NotFound();
        
        //is not null
        return Ok(post);
    }

    [HttpGet("/ListarTodasPostagens")]
    public IActionResult ObterPostagens()
    {
        var posts = _db.Postagens.Where(x => x.Conteudo != null || x.Conteudo != "");
        
        //if null, not found.
        if(!posts.Any()) return NotFound("Não foi possivel devolver qualquer postagem.");

        return Ok(posts);
    }
    
    [HttpGet("/ListarPostagensPorUsuario/{nomeUsuario}")]
    public IActionResult ObterPostagemPorUsuario(string nomeUsuario)
    {
        //Find user by username
        var contaNomeUsuario = _db.Contas.Where(c => c.NomeUsuario == nomeUsuario);
        var usuario = contaNomeUsuario.First();
        
        //Find all post of user.
        var posts = _db.Postagens.Where(
            x => x.Autor.Contains(nomeUsuario) 
                         && x.IdConta.Id == usuario.Id
                         );
        
        //if null, not found.
        if(!posts.Any()) return NotFound("Não foi possivel devolver qualquer postagem.");

        return Ok(posts);
    }

    [HttpPost("/CriarPostagem")]
    public IActionResult CriarPostagem(Postagem post)
    {
        //FindUsername by id
        var conta = _db.Contas.Find(post.IdConta.Id);
        
        //conta is null
        if (conta is null)
            return Ok(new
            {
                Error = "Postagem não criada pois o usuario é inexistente, corrijá e tente novamente.",
                Postagem = post
            });
        
        //If diferent null
        conta.Id = post.IdConta.Id;
        post.IdConta = conta;
        post.Autor = conta.NomeUsuario;
        
        _db.Add(post);
        _db.SaveChanges();

        return Ok(CreatedAtAction(
            nameof(ObterUmPost),
            new {id = post.Id},
            post)
        );
    }

    [HttpPut("/Editar/{id}")]
    public IActionResult EditarPostagem(int id, Postagem postEdit)
    {
        //Search post in db.
        var post = _db.Postagens.Find(id);
        
        //Verify if post and postEdit is null and return a text.
        if (post is null) return NotFound("Postagem não existe ou foi deletada.");
        if (postEdit is null) return NotFound("Um ou mais campos devem ser preenchidos.");
        
        //Edit post Title or Content if some else is not null;
        post.Titulo = (postEdit.Titulo.Equals("string") || postEdit.Titulo.Length < 5)
            ? post.Titulo
            : postEdit.Titulo;
        post.Conteudo = postEdit.Conteudo.Equals("string")
            ? post.Conteudo
            : postEdit.Conteudo;

        //Save in DB.
        _db.Postagens.Update(post);
        _db.SaveChanges();

        return Ok(
            CreatedAtAction(nameof(ObterUmPost),
                new { post.Id },
                post)
        );
    }

    [HttpDelete("/Deletar/{id}")]
    public IActionResult DeletarPostagem(int id)
    {
        //Find post to delete.
        var postDeletar = _db.Postagens.Find(id);
        
        //Check if is null
        if (postDeletar is null)
            return NotFound("A postagem já foi deletada ou não existe.");
        
        //Remove
        _db.Postagens.Remove(postDeletar);
        _db.SaveChanges();

        return Ok(new {
            Success = $"O post: '{postDeletar.Titulo}', foi apagando com sucesso."
        });
    }
}