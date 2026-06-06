using Microsoft.ML.Data;

namespace Unilife.Models.ML
{
    public class EventoPrediccion
    {
        [ColumnName("PredictedLabel")]
        public bool Interesa { get; set; }

        public float Probability { get; set; }
        public float Score { get; set; }
    }
}