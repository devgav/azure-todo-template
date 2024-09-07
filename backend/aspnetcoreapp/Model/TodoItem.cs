namespace TodoApi.Models {
    public class TodoItem {
        public string task {get; set;} = "No task";
        public bool isComplete {get; set;}
        public int? Id {get; set;}
    }
}