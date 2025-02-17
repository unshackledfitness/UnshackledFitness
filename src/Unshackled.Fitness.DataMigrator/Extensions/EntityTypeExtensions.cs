using Microsoft.EntityFrameworkCore;

namespace Unshackled.Fitness.DataMigrator.Extensions;

public static class EntityTypeExtensions
{
	public static Task EnableIdentityInsert<T>(this DbContext context) => SetIdentityInsert<T>(context, enable: true);
	public static Task DisableIdentityInsert<T>(this DbContext context) => SetIdentityInsert<T>(context, enable: false);

	private static Task SetIdentityInsert<T>(DbContext context, bool enable) {
		var entityType = context.Model.FindEntityType(typeof(T));
		string value = enable ? "ON" : "OFF";
		string query = $"SET IDENTITY_INSERT {entityType!.GetSchema()}.{entityType!.GetTableName()} {value}";
		return context.Database.ExecuteSqlRawAsync(query);
	}

	public static async Task SaveChangesWithIdentityInsert<T>(this DbContext context) {
		using var transaction = await context.Database.BeginTransactionAsync();
		await context.EnableIdentityInsert<T>();
		await context.SaveChangesAsync();
		await context.DisableIdentityInsert<T>();
		await transaction.CommitAsync();
	}

}