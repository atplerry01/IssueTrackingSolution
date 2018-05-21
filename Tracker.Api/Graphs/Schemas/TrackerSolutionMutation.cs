using GraphQL.Types;
using Tracker.Api.Graphs.InputTypes;
using Tracker.Api.Graphs.ObjectTypes;
using Tracker.Api.Models.Lookups;
using Tracker.Api.Persistence.Repository.Common;

namespace Tracker.Api.Graphs.Schemas
{
  
    public class TrackerSolutionMutation : ObjectGraphType<object>
    {
        public TrackerSolutionMutation(IGenericRepository<Department> department)
        {
            Name = "TrackerSolutionMutation";

            #region Department
            Field<DepartmentType>(
                "addDepartment",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<DepartmentInputType>> { Name = "Department" }
                ),
                resolve: context =>
                {
                    var departmentInput = context.GetArgument<Department>(Name = "Department");
                    return department.AddAsync(departmentInput);
                });

            Field<DepartmentType>(
               "deleteDepartment",
               arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "Id" }
               ),
               resolve: context =>
               {
                   var id = context.GetArgument<int>("Id");
                   return department.DeleteAsync(id);
               });

            Field<DepartmentType>(
               "updateDepartment",
               arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<DepartmentInputType>> { Name = "Department" }
               ),
               resolve: context =>
               {
                   var departmentInput = context.GetArgument<Department>("Department");
                   return department.UpdateAsync(1, departmentInput);
               });

            #endregion

          
            Description = "MarketApp Mutation Fields for, You can add, delete, update about categories, products, users, reviews and orders";
        }
    }
}