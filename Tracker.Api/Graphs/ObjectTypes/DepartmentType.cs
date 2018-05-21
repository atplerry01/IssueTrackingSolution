using GraphQL.Types;
using Tracker.Api.Models.Lookups;

namespace Tracker.Api.Graphs.ObjectTypes
{
  
    public class DepartmentType : ObjectGraphType<Department>
    {
        public DepartmentType()
        {
            Name = "Department";
            Description = "Department";

            Field(x => x.Id);
            Field(x => x.Name);
            Field(x => x.ShortCode);
        }
    }

}