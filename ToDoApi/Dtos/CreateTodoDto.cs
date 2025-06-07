using System.ComponentModel.DataAnnotations;

namespace ToDoApi.Dtos
{
    public class CreateTodoDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        public bool IsCompleted { get; set; }

    }
}
