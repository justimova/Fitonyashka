using System;
namespace Fytonyashka.Pages.DataModels
{
    public class WeeklyWeightGroup
    {
        public string WeekTitle { get; set; }
        public List<WeightInputModel> Entries { get; set; }
    }
}

