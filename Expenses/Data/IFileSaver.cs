using System.Collections.Generic;

namespace Expenses.Data
{
    public interface IFileSaver 
    {
        void SavePersonas(List<Persona> personas);
        List<Persona> ReadPersonas();
    }
}
