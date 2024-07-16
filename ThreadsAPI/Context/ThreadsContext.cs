using Microsoft.EntityFrameworkCore;
using ThreadsAPI.Models;

namespace ThreadsAPI.Context;

public class ThreadsContext : DbContext
{
    //Reference de DBServer.
    public ThreadsContext(DbContextOptions<ThreadsContext> options) : base (options) { }
    
    //Contexts Tables
    public DbSet<Conta> Contas { get; set; }
    public DbSet<Postagem> Postagens { get; set; }
}