using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaDoZe
{
    public class RegistroFrequenciaColaborador
    {
        public int Id { get; set; }
        public Colaborador Colaborador { get; set; }
        public DateTime DataHoraEntrada { get; set; }
        public DateTime? DataHoraSaida { get; set; }
        public TimeSpan TempoPermanencia =>
       (DataHoraSaida.HasValue ? DataHoraSaida.Value : DateTime.Now) - DataHoraEntrada;

        public static TimeSpan CalcularPermanenciaTotalDoDia(List<RegistroFrequenciaColaborador> registrosDia)
        {
            TimeSpan total = TimeSpan.Zero;

            foreach (var registro in registrosDia)
            {
                total += registro.TempoPermanencia;
            }

            return total;
        }

        public static bool UltrapassouLimiteDiario(Colaborador colaborador, DateTime dataHoraEntrada, DateTime dataHoraSaida)
        {
            TimeSpan tempoPermanencia = dataHoraSaida - dataHoraEntrada;

            int limiteHoras = colaborador.Tipo == TipoColaborador.Clt ? 8 : 6;
            TimeSpan limite = TimeSpan.FromHours(limiteHoras);

            // Retorna true se ultrapassou o limite
            return tempoPermanencia > limite;
        }

    }
}
