using GraphQL.Types;
using Tracker.Api.Graphs.ObjectTypes;
using Tracker.Api.Models.Lookups;
using Tracker.Api.Persistence.Repository.Common;

namespace Tracker.Api.Graphs.Schemas
{
    public class TrackerSolutionQuery : ObjectGraphType<object>
    {
        public TrackerSolutionQuery(IGenericRepository<Department> department)
        {
            Name = "TrackerSolutionQuery";

            #region Department
            Field<DepartmentType>(
                "department",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "Id" }
                ),
                resolve: context => department.FindById(context.GetArgument<int>("Id"))
            );

            Field<ListGraphType<DepartmentType>>(
                "departments",
                resolve: context => department.All()
            );
            #endregion

         
        }

    }
}