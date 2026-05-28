using System.Collections;

namespace RFControllers
{
    public class DataRowsResult(IEnumerable rows)
    {
        public IEnumerable Rows { get; set; } = rows;
    }
}
