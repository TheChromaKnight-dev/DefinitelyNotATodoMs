using BusinessLogic.Blas;
using BusinessLogic.Enums;
using DataLayer.Daos;

namespace BusinessLogic.DaoToBlaMappers
{
    public static class ToDoDaoToBlaMapper
    {
        public static ToDoEntityBla MapToBla(this ToDoDao dao)
        {
            return new ToDoEntityBla(dao.Id, dao.Title, dao.Description, Enum.Parse<StatusBlaEnum>(dao.Status, true));
        }
        public static ToDoDao MapToDao(this ToDoEntityBla bla)
        {
            return new ToDoDao
            {
                Id = bla.Id,
                Title = bla.Title,
                Description = bla.Description,
                Status = bla.Status.ToString()
            };
        }
    }
}
