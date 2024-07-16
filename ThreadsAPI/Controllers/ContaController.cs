using Microsoft.AspNetCore.Mvc;
using ThreadsAPI.Context;
using ThreadsAPI.Models;

namespace ThreadsAPI.Controllers;

[ApiController]
[Route("[Controller]")]
public class ContaController : ControllerBase
{
    private readonly ThreadsContext _db;

    public ContaController(ThreadsContext context)
    {
        _db = context;
    }
    
    [HttpGet("/Visualizar/{id}")]
    public IActionResult Visualizar(int id)
    {
        var perfil = _db.Contas.Find(id);

        //If not exist, return not found.
        if (perfil is null) return NotFound();

        return Ok(perfil);
    }

    [HttpPost("/Criar")]
    public IActionResult Criar(Conta conta)
    {
        _db.Add(conta);
        _db.SaveChanges();

        return CreatedAtAction(nameof(Visualizar), new { id = conta.Id }, conta);
    }

    [HttpPut("/Editar/{nomeUsuario}")]
    public IActionResult Editar(string nomeUsuario, string novoNomeUsuario)
    {
        var conta = _db.Contas.Where(
                user => user.NomeUsuario.Contains(nomeUsuario) && user != null
            );
        
        //if return anything continue, if not stop;
        if (!conta.Any()) return NotFound();

        //Select the first account
        var user = conta.First();
        
        //Is not null / Update this
        user.NomeUsuario = novoNomeUsuario;
        _db.Contas.Update(user);
        _db.SaveChanges();

        //Return save user.
        return Ok(user);
    }

    [HttpDelete("Deletar/{id}")]
    public IActionResult DeletarConta(int id)
    {
        var conta = _db.Contas.Find(id);
        
        //Verify, if account is null
        if (conta is null) return NotFound();

        _db.Contas.Remove(conta);
        _db.SaveChanges();

        return Ok(
            new {
                sucesso = $"Conta '{conta.NomeUsuario}' foi deletada com sucesso!",
            });
    }
    
}