using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wt_desktop.ef.Entity;

public abstract class WtEntity
{

    public abstract void OnModelCreating(ModelBuilder modelBuilder);
}
