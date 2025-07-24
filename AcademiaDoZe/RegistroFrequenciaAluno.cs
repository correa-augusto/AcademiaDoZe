using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaDoZe
{
    public class RegistroFrequenciaAluno
    {
        public int Id { get; set; }
        public Matricula Matricula { get; set; } = null!;

        public Aluno Aluno { get; set; }
        public DateTime DataHoraEntrada { get; private set; }
        public DateTime? DataHoraSaida { get; private set; }

        public TimeSpan TempoPermanencia =>
       (DataHoraSaida.HasValue ? DataHoraSaida.Value : DateTime.Now) - DataHoraEntrada;


        public void RegistrarEntrada()
        {
            if (!Matricula.Ativa)
                throw new InvalidOperationException("Não é possível registrar entrada. Matrícula inativa.");

            DataHoraEntrada = DateTime.Now;

            int diasRestantes = Matricula.DiasRestantesDoPlano();
            Console.WriteLine($"Entrada registrada. Dias restantes do plano: {diasRestantes}");
        }

   
        public void RegistrarSaida()
        {
            if (DataHoraEntrada == default)
                throw new InvalidOperationException("Entrada não registrada.");

            if (DataHoraSaida != null)
                throw new InvalidOperationException("Saída já registrada.");

            DataHoraSaida = DateTime.Now;

            TimeSpan tempoPermanencia = DataHoraSaida.Value - DataHoraEntrada;
            Console.WriteLine($"Saída registrada. Tempo de permanência: {tempoPermanencia.Hours}h {tempoPermanencia.Minutes}min.");
        }
    }
}
