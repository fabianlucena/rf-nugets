using System.Collections;

namespace RFService.Data
{
    public class DataRowsResult(IEnumerable rows)
    {
        public IEnumerable Rows { get; set; } = rows;
    }
}
