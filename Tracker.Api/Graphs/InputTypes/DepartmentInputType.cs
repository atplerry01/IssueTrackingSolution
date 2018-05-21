using GraphQL.Types;

namespace Tracker.Api.Graphs.InputTypes
{
    public class DepartmentInputType: InputObjectGraphType
    {
        public DepartmentInputType()
        {
            Name = "DepartmentInputType";
            Field<IntGraphType>("Id");
            Field<NonNullGraphType<StringGraphType>>("Name");
            Field<NonNullGraphType<StringGraphType>>("ShortCode");
        }
    }

}