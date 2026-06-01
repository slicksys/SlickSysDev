using System;


namespace SlickSysDev.ApiService
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public byte[] RowVersion { get; set; }
        public string TestText { get; set; }  //string item for T4 generated tests

    }
}
