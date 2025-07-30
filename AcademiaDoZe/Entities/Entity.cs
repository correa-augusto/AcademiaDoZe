//AUGUSTO DOS SANTOS CORREA
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaDoZe.Domain.Entities;
// PADRÃO DE DESIGN - classe base para todas as entidades
// Garantindo que tenham um Id e não permitindo sua edição diretamente
public abstract class Entity
{
    public int Id { get; protected set; }
    public Entity(int id = 0)
    {
        Id = id;
    }
}