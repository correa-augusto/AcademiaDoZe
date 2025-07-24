using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaDoZe
{
    public class RelatorioService
    {
        private readonly List<RegistroFrequenciaAluno> _registrosAlunos;
        private readonly List<RegistroFrequenciaColaborador> _registrosColaboradores;

        public RelatorioService(
            List<RegistroFrequenciaAluno> registrosAlunos,
            List<RegistroFrequenciaColaborador> registrosColaboradores)
        {
            _registrosAlunos = registrosAlunos;
            _registrosColaboradores = registrosColaboradores;
        }

        // 1. Horários de maior procura
        public void HorariosDeMaiorProcura()
        {
            var horarios = _registrosAlunos
                .GroupBy(r => r.DataHoraEntrada.Hour)
                .Select(g => new { Hora = g.Key, Contagem = g.Count() })
                .OrderByDescending(g => g.Contagem)
                .ToList();

            Console.WriteLine("Horários de maior procura:");
            foreach (var h in horarios)
                Console.WriteLine($"- {h.Hora}:00 → {h.Contagem} entradas");
        }

        // 2. Permanência média dos alunos na academia
        public void PermanenciaMediaAlunos()
        {
            var media = TimeSpan.FromMinutes(_registrosAlunos
                .Where(r => r.DataHoraSaida.HasValue)
                .Average(r => r.TempoPermanencia.TotalMinutes));

            Console.WriteLine($"Permanência média dos alunos: {media.TotalMinutes:F2} minutos");
        }

        // 3. Tendência de evasão (alunos que não aparecem há mais de 15 dias)
        public void TendenciaEvasaoERetencao()
        {
            var hoje = DateTime.Now;
            var alunosEvasao = _registrosAlunos
                .GroupBy(r => r.Aluno.Id)
                .Where(g => (hoje - g.Max(r => r.DataHoraEntrada)).TotalDays > 15)
                .Select(g => g.First().Aluno.Nome)
                .ToList();

            Console.WriteLine("Alunos com tendência de evasão (15+ dias sem frequentar):");
            alunosEvasao.ForEach(a => Console.WriteLine($"- {a}"));

            var total = _registrosAlunos.Select(r => r.Aluno.Id).Distinct().Count();
            var evadidos = alunosEvasao.Count;
            Console.WriteLine($"Taxa de retenção: {(total - evadidos) * 100.0 / total:F2}%");
        }

        // 4. Horas trabalhadas por dia por colaborador
        public void HorasTrabalhadasPorDia()
        {
            var agrupado = _registrosColaboradores
                .Where(r => r.DataHoraSaida.HasValue)
                .GroupBy(r => new { r.Colaborador.Nome, Dia = r.DataHoraEntrada.Date })
                .Select(g => new
                {
                    g.Key.Nome,
                    g.Key.Dia,
                    TotalHoras = g.Sum(r => r.TempoPermanencia.TotalHours)
                });

            Console.WriteLine("Horas trabalhadas por colaborador (por dia):");
            foreach (var item in agrupado)
            {
                Console.WriteLine($"- {item.Nome} em {item.Dia.ToShortDateString()}: {item.TotalHoras:F2}h");
            }
        }
    }
}
