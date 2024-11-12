using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Data;

namespace Unshackled.Fitness.Core.Data;

public class FitnessDbContext : BaseDbContext
{
	public FitnessDbContext(DbContextOptions<FitnessDbContext> options,
		ConnectionStrings connectionStrings,
		DbConfiguration dbConfig) : base(options, connectionStrings, dbConfig) { }

	public FitnessDbContext(DbContextOptions options, 
		ConnectionStrings connectionStrings, 
		DbConfiguration dbConfig) : base(options, connectionStrings, dbConfig) { }

	public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();
	public DbSet<ActivityPlanEntity> ActivityPlans => Set<ActivityPlanEntity>();
	public DbSet<ActivityPlanTargetEntity> ActivityPlanTargets => Set<ActivityPlanTargetEntity>();
	public DbSet<ActivityTargetEntity> ActivityTargets => Set<ActivityTargetEntity>();
	public DbSet<ActivityTypeEntity> ActivityTypes => Set<ActivityTypeEntity>();
	public DbSet<ExerciseEntity> Exercises => Set<ExerciseEntity>();
	public DbSet<ExportFileEntity> ExportFiles => Set<ExportFileEntity>();
	public DbSet<MetricDefinitionGroupEntity> MetricDefinitionGroups => Set<MetricDefinitionGroupEntity>();
	public DbSet<MetricDefinitionEntity> MetricDefinitions => Set<MetricDefinitionEntity>();
	public DbSet<MetricPresetEntity> MetricPresets => Set<MetricPresetEntity>();
	public DbSet<MetricEntity> Metrics => Set<MetricEntity>();
	public DbSet<ProgramEntity> Programs => Set<ProgramEntity>();
	public DbSet<ProgramTemplateEntity> ProgramTemplates => Set<ProgramTemplateEntity>();
	public DbSet<WorkoutEntity> Workouts => Set<WorkoutEntity>();
	public DbSet<WorkoutSetEntity> WorkoutSets => Set<WorkoutSetEntity>();
	public DbSet<WorkoutSetGroupEntity> WorkoutSetGroups => Set<WorkoutSetGroupEntity>();
	public DbSet<WorkoutTaskEntity> WorkoutTasks => Set<WorkoutTaskEntity>();
	public DbSet<WorkoutTemplateEntity> WorkoutTemplates => Set<WorkoutTemplateEntity>();
	public DbSet<WorkoutTemplateSetEntity> WorkoutTemplateSets => Set<WorkoutTemplateSetEntity>();
	public DbSet<WorkoutTemplateSetGroupEntity> WorkoutTemplateSetGroups => Set<WorkoutTemplateSetGroupEntity>();
	public DbSet<WorkoutTemplateTaskEntity> WorkoutTemplateTasks => Set<WorkoutTemplateTaskEntity>();

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfiguration(new ActivityEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ActivityPlanEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ActivityPlanTargetEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ActivityTargetEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ActivityTypeEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ExerciseEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ExportFileEntity.TypeConfiguration());
		builder.ApplyConfiguration(new MetricDefinitionEntity.TypeConfiguration());
		builder.ApplyConfiguration(new MetricDefinitionGroupEntity.TypeConfiguration());
		builder.ApplyConfiguration(new MetricEntity.TypeConfiguration());
		builder.ApplyConfiguration(new MetricPresetEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ProgramEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ProgramTemplateEntity.TypeConfiguration());
		builder.ApplyConfiguration(new WorkoutEntity.TypeConfiguration());
		builder.ApplyConfiguration(new WorkoutSetEntity.TypeConfiguration());
		builder.ApplyConfiguration(new WorkoutSetGroupEntity.TypeConfiguration());
		builder.ApplyConfiguration(new WorkoutTaskEntity.TypeConfiguration());
		builder.ApplyConfiguration(new WorkoutTemplateEntity.TypeConfiguration());
		builder.ApplyConfiguration(new WorkoutTemplateSetEntity.TypeConfiguration());
		builder.ApplyConfiguration(new WorkoutTemplateSetGroupEntity.TypeConfiguration());
		builder.ApplyConfiguration(new WorkoutTemplateTaskEntity.TypeConfiguration());
		
		base.OnModelCreating(builder);
	}
}
