﻿using MediatR;
using Unshackled.Fitness.My.Client.Features.Exercises.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Exercises.Actions;

public class SearchExercises
{
	public class Query : IRequest<SearchResult<ExerciseModel>>
	{
		public SearchExerciseModel Model { get; private set; }

		public Query(SearchExerciseModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseExerciseHandler, IRequestHandler<Query, SearchResult<ExerciseModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<SearchResult<ExerciseModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<SearchExerciseModel, SearchResult<ExerciseModel>>($"{baseUrl}search", request.Model) ??
				new SearchResult<ExerciseModel>();
		}
	}
}
