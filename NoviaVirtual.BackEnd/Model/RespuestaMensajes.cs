namespace NoviaVirtual.BackEnd.Model
{
    public class RespuestaMensajes
    {
        public List<Datum> data { get; set; }
        public string first_id { get; set; }
        public string last_id { get; set; }
        public bool has_more { get; set; }
    }
    public class Content
    {
        public string type { get; set; }
        public Text text { get; set; }
    }

    public class Datum
    {
        public string id { get; set; }
        public string @object { get; set; }
        public int created_at { get; set; }
        public string assistant_id { get; set; }
        public string thread_id { get; set; }
        public string run_id { get; set; }
        public string role { get; set; }
        public List<Content> content { get; set; }
        public List<object> attachments { get; set; }
    }

    public class Text
    {
        public string value { get; set; }
        public List<object> annotations { get; set; }
    }


}
