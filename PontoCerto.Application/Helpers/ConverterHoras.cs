using System.Globalization;

namespace PontoCerto.Application.Helpers
{
    public static class ConverterHoras
    {
        public static DateTime ConverterDataParaDateTime(string data)
        {
            return DateTime.ParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        public static TimeSpan ConverterHoraEntradaParaTimeSpan(string horaEntrada)
        {
            return TimeSpan.ParseExact(horaEntrada, "hh\\:mm", CultureInfo.InvariantCulture);
        }

        public static TimeSpan ConverterHoraSaidaParaTimeSpan(string horaSaida)
        {
            return TimeSpan.ParseExact(horaSaida, "hh\\:mm", CultureInfo.InvariantCulture);
        }

        public static TimeSpan ConverterFimAlmocoParaTimeSpan(string fimAlmoco)
        {
            return string.IsNullOrEmpty(fimAlmoco)
                ? TimeSpan.Zero
                : TimeSpan.ParseExact(fimAlmoco, "hh\\:mm", CultureInfo.InvariantCulture);
        }

        public static TimeSpan ConverterInicioAlmocoParaTimeSpan(string inicioAlmoco)
        {
            return string.IsNullOrEmpty(inicioAlmoco)
                ? TimeSpan.Zero
                : TimeSpan.ParseExact(inicioAlmoco, "hh\\:mm", CultureInfo.InvariantCulture);
        }
    }
}
