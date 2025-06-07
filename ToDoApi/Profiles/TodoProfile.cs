using AutoMapper;
using ToDoApi.Dtos;
using ToDoApi.Models;

namespace ToDoApi.Profiles
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<TodoItem, TodoItemDto>();
            CreateMap<CreateTodoDto, TodoItem>();
        }
    }
}
