using System;
using GraphQL.Types;

namespace Tracker.Api.Graphs.Schemas
{
    public class TrackerSolutionSchema:Schema
    {
        public TrackerSolutionSchema(Func<Type, GraphType> resolve) : base(resolve)
        {
            Query = (TrackerSolutionQuery)resolve(typeof(TrackerSolutionQuery));
            Mutation = (TrackerSolutionMutation)resolve(typeof(TrackerSolutionMutation));
        }
    }

}