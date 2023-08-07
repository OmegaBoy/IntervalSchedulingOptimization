using IntervalSchedulingOptimization;

class Program
{
    readonly static int slots = 48;
    static void Main(string[] args)
    {
        var facthosData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FacthosData>>(File.ReadAllText(args[0]));

        var minDate = args.Length > 1 ? DateTime.Parse(args[1]) : facthosData.OrderBy(fd => fd.FechaVigDesde).First().FechaVigDesde;
        var maxDate = args.Length > 2 ? DateTime.Parse(args[2]) : facthosData.OrderByDescending(fd => fd.FechaHasta).First().FechaHasta;

        for (int dayIndex = 0; dayIndex < (maxDate - minDate).Value.Days + 1; dayIndex++)
        {
            var fechaActual = minDate.AddDays(dayIndex);
            Console.WriteLine($"Fecha de analisis: {fechaActual.ToString("yyyy-MM-dd")}");
            var intervals = (facthosData ?? throw new Exception("Error en data de facthos")).Where(fd => fd.FechaVigDesde <= fechaActual && (fd.FechaHasta == null || fd.FechaHasta > fechaActual) && CheckDia(fechaActual, fd.Dia)).Select(fd => fd.ToInterval(slots)).ToList();
            if (!intervals?.Any() ?? false)
            {
                Console.WriteLine("No hay intervalos");
                continue;
            }
            PrintIntervals("Intervals:", intervals.Select(i => new List<Interval> { i }).ToList());

            List<List<Interval>> intervalSets = IntervalSchedulingService.ScheduleIntervals(intervals, restrictionIntervals: new List<Interval> { new Interval(0, 15, 0, true), new Interval(40, 47, 0, true) });
            PrintIntervals("Minimum Sets:", intervalSets, true, true);
        }
    }

    readonly static string freeSlot = "▓▓▓▓";
    readonly static string ocupiedSlot = "████";
    readonly static string restrictedSlot = "    ";

    private static void PrintIntervals(string title, List<List<Interval>> intervalSets, bool together = false, bool printOccupation = false)
    {
        Console.WriteLine(title);
        decimal averageOccupation = 0;
        for (int i = 0; i < intervalSets.Count; i++)
        {
            for (decimal j = 0; j < slots; j++)
            {
                string msj = (j / 2 % 1 == 0 ? (j / 2).ToString() : Math.Floor(j / 2) + "½").PadRight(4);
                Console.Write(msj);
            }
            Console.WriteLine();
            if (!together)
            {
                for (int k = 0; k < intervalSets[i].Count; k++)
                {
                    for (int j = 0; j < slots; j++)
                    {
                        if (j >= intervalSets[i][k].Start && j <= intervalSets[i][k].End)
                        {
                            if (!intervalSets[i][k].Restricted)
                            {
                                Console.Write(ocupiedSlot);
                            }
                            else
                            {
                                Console.Write(restrictedSlot);
                            }
                        }
                        else
                        {
                            Console.Write(freeSlot);
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < slots; j++)
                {
                    var ocupingInterval = intervalSets[i].SingleOrDefault(_i => j >= _i.Start && j <= _i.End);
                    if (ocupingInterval != null)
                    {
                        if (!ocupingInterval.Restricted)
                        {
                            Console.Write(ocupiedSlot);
                        }
                        else
                        {
                            Console.Write(restrictedSlot);
                        }
                    }
                    else
                    {
                        Console.Write(freeSlot);
                    }
                }
            }
            int currentOccupation = 0;
            for (int k = 0; k < slots; k++)
            {
                bool isOverlap = intervalSets[i].Any(_i => _i.Start <= k && _i.End >= k);
                if (isOverlap)
                {
                    currentOccupation++;
                }
            }
            averageOccupation = averageOccupation == 0 ? (decimal)currentOccupation / slots : (averageOccupation + (decimal)currentOccupation / slots) / 2;
            if (printOccupation)
            {
                Console.WriteLine($" O: {(decimal)currentOccupation / slots * 100:#.#}%"); 
            }
            Console.WriteLine();
        }
        if (printOccupation)
        {
            Console.WriteLine($"AO: {averageOccupation * 100:#.#}%"); 
        }
    }

    private static bool CheckDia(DateTime fechaActual, string dia) => dia switch
    {
        "Lunes" => fechaActual.DayOfWeek == DayOfWeek.Monday,
        "Martes" => fechaActual.DayOfWeek == DayOfWeek.Tuesday,
        "Miércoles" => fechaActual.DayOfWeek == DayOfWeek.Wednesday,
        "Jueves" => fechaActual.DayOfWeek == DayOfWeek.Thursday,
        "Viernes" => fechaActual.DayOfWeek == DayOfWeek.Friday,
        "Sábado" => fechaActual.DayOfWeek == DayOfWeek.Saturday,
        "Domingo" => fechaActual.DayOfWeek == DayOfWeek.Sunday,
        _ => false,
    };
}
