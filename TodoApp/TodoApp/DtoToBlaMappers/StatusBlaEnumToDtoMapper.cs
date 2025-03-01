using BusinessLogic.Enums;
using CodeGen;

namespace TodoApp.DtoToBlaMappers
{
    public static class StatusBlaEnumToDtoMapper
    {
        public static StatusEnum MapToDto(this StatusBlaEnum bla)
        {
            return bla switch
            {
                StatusBlaEnum.Completed => StatusEnum.Completed,
                StatusBlaEnum.NotStarted => StatusEnum.NotStarted,
                _ => throw new NotImplementedException("This Status value is not supported")
            };
        }
        public static StatusBlaEnum MapToBla(this StatusEnum bla)
        {
            return bla switch
            {
                StatusEnum.Completed => StatusBlaEnum.Completed,
                StatusEnum.NotStarted => StatusBlaEnum.NotStarted,
                _ => throw new NotImplementedException("This Status value is not supported")
            };
        }
    }

}
