using System.ComponentModel.DataAnnotations;

namespace ToDoApi.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be 3-100 characters.")]
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}
