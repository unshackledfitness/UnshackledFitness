INSERT INTO [dbo].[ufd_Users]
(
		 [Id]
		,[UserName]
		,[NormalizedUserName]
		,[Email]
		,[NormalizedEmail]
		,[EmailConfirmed]
		,[PasswordHash]
		,[SecurityStamp]
		,[ConcurrencyStamp]
		,[PhoneNumber]
		,[PhoneNumberConfirmed]
		,[TwoFactorEnabled]
		,[LockoutEnd]
		,[LockoutEnabled]
		,[AccessFailedCount]
)
SELECT 
		 [Id]
		,[UserName]
		,[NormalizedUserName]
		,[Email]
		,[NormalizedEmail]
		,[EmailConfirmed]
		,[PasswordHash]
		,[SecurityStamp]
		,[ConcurrencyStamp]
		,[PhoneNumber]
		,[PhoneNumberConfirmed]
		,[TwoFactorEnabled]
		,[LockoutEnd]
		,[LockoutEnabled]
		,[AccessFailedCount]
FROM [dbo].[ul_Users];

SET IDENTITY_INSERT [dbo].[ufd_Members] ON;
INSERT INTO [dbo].[ufd_Members]
( 
		 [Id]
		,[Email]
		,[DateCreatedUtc]
		,[DateLastModifiedUtc]
		,[IsActive]
)
SELECT 
		 [Id]
		,[Email]
		,[DateCreatedUtc]
		,[DateLastModifiedUtc]
		,[IsActive]
FROM [dbo].[ul_Members];
SET IDENTITY_INSERT [dbo].[ufd_Members] OFF;

SET IDENTITY_INSERT [dbo].[ufd_MemberMeta] ON;
INSERT INTO [dbo].[ufd_MemberMeta]
(
		 [Id]
		,[MemberId]
		,[MetaKey]
		,[MetaValue]
)
SELECT 
		 [Id]
		,[MemberId]
		,[MetaKey]
		,[MetaValue]
FROM [dbo].[ul_MemberMeta];
SET IDENTITY_INSERT [dbo].[ufd_MemberMeta] OFF;

SET IDENTITY_INSERT [dbo].[ufd_Households] ON;
INSERT INTO [dbo].[ufd_Households]
( 
		 [Id]
		,[Title]
		,[DateCreatedUtc]
		,[DateLastModifiedUtc]
		,[MemberId]
)
SELECT 
		 [Id]
		,[Title]
		,[DateCreatedUtc]
		,[DateLastModifiedUtc]
		,[MemberId]
FROM [dbo].[ul_Groups];
SET IDENTITY_INSERT [dbo].[ufd_Households] OFF;

INSERT INTO [dbo].[ufd_HouseholdMembers]
(
		 [HouseholdId]
		,[MemberId]
		,[PermissionLevel]
)
SELECT 
		 [GroupId]
		,[MemberId]
		,[PermissionLevel]
FROM [dbo].[ul_GroupMembers];

SET IDENTITY_INSERT [dbo].[ufd_Products] ON;
INSERT INTO [dbo].[ufd_Products]
( 
		 [Id]
		,[Brand],[Title],[Description],[IsArchived],[HasNutritionInfo],[ServingSize],[ServingSizeMetric],[ServingSizeMetricN],[ServingSizeMetricUnit]
		,[ServingSizeN],[ServingSizeUnit],[ServingSizeUnitLabel],[ServingsPerContainer],[Calories],[Fat],[FatUnit],[FatN],[Carbohydrates],[CarbohydratesUnit],[CarbohydratesN]
		,[Protein],[ProteinUnit],[ProteinN],[DateCreatedUtc],[DateLastModifiedUtc],[HouseholdId]
)
SELECT 
		 [Id]
		,[Brand],[Title],[Description],[IsArchived],[HasNutritionInfo],[ServingSize],[ServingSizeMetric],[ServingSizeMetricN],[ServingSizeMetricUnit]
		,[ServingSizeN],[ServingSizeUnit],[ServingSizeUnitLabel],[ServingsPerContainer],[Calories],[TotalFat],[TotalFatUnit],[TotalFatN],[TotalCarbohydrates],[TotalCarbohydratesUnit],[TotalCarbohydratesN]
		,[Protein],[ProteinUnit],[ProteinN],[DateCreatedUtc],[DateLastModifiedUtc],[GroupId]
FROM [dbo].[ul_Products];
SET IDENTITY_INSERT [dbo].[ufd_Products] OFF;

SET IDENTITY_INSERT [dbo].[ufd_ProductBundles] ON;
INSERT INTO [dbo].[ufd_ProductBundles]
( 
		 [Id]
		,[Title]
		,[DateCreatedUtc]
		,[DateLastModifiedUtc]
		,[HouseholdId]
)
SELECT 
		 [Id]
		,[Title]
		,[DateCreatedUtc]
		,[DateLastModifiedUtc]
		,[GroupId]
FROM [dbo].[ul_ProductBundles];
SET IDENTITY_INSERT [dbo].[ufd_ProductBundles] OFF;

INSERT INTO [dbo].[ufd_ProductBundleItems]
(
		 [ProductBundleId]
		,[ProductId]
		,[Quantity]
)
SELECT 
		 [ProductBundleId]
		,[ProductId]
		,[Quantity]
FROM [dbo].[ul_ProductBundleItems];

SET IDENTITY_INSERT [dbo].[ufd_Stores] ON;
INSERT INTO [dbo].[ufd_Stores]
( 
		 [Id]
		,[Title]
		,[Description]
		,[DateCreatedUtc]
		,[DateLastModifiedUtc]
		,[HouseholdId]
)
SELECT 
		 [Id]
		,[Title]
		,[Description]
		,[DateCreatedUtc]
		,[DateLastModifiedUtc]
		,[GroupId]
FROM [dbo].[ul_Stores] 
SET IDENTITY_INSERT [dbo].[ufd_Stores] OFF;

SET IDENTITY_INSERT [dbo].[ufd_StoreLocations] ON;
INSERT INTO [dbo].[ufd_StoreLocations]
( 
		 [Id]
		,[StoreId]
		,[Title]
		,[Description]
		,[SortOrder]
		,[DateCreatedUtc]
		,[DateLastModifiedUtc]
		,[HouseholdId]
)
SELECT 
		 [Id]
		,[StoreId]
		,[Title]
		,[Description]
		,[SortOrder]
		,[DateCreatedUtc]
		,[DateLastModifiedUtc]
		,[GroupId]
FROM [dbo].[ul_StoreLocations];
SET IDENTITY_INSERT [dbo].[ufd_StoreLocations] OFF;

INSERT INTO [dbo].[ufd_StoreProductLocations]
(
		 [StoreId]
		,[ProductId]
		,[StoreLocationId]
		,[SortOrder]
)
SELECT 
		 [StoreId]
		,[ProductId]
		,[StoreLocationId]
		,[SortOrder]
FROM [dbo].[ul_StoreProductLocations];

SET IDENTITY_INSERT [dbo].[ufd_ShoppingLists] ON;
INSERT INTO [dbo].[ufd_ShoppingLists]
( 
		 [Id]
		,[Title]
		,[StoreId]
		,[DateCreatedUtc]
		,[DateLastModifiedUtc]
		,[HouseholdId]
)
SELECT 
		 [Id]
		,[Title]
		,[StoreId]
		,[DateCreatedUtc]
		,[DateLastModifiedUtc]
		,[GroupId]
FROM [dbo].[ul_ShoppingLists];
SET IDENTITY_INSERT [dbo].[ufd_ShoppingLists] OFF;

INSERT INTO [dbo].[ufd_ShoppingListItems]
(
		 [ShoppingListId]
		,[ProductId]
		,[Quantity]
		,[IsInCart]
		,[RecipeAmountsJson]
)
SELECT 
		 [ShoppingListId]
		,[ProductId]
		,[Quantity]
		,[IsInCart]
		,[RecipeAmountsJson]
FROM [dbo].[ul_ShoppingListItems];

SET IDENTITY_INSERT [dbo].[ufd_Recipes] ON;
INSERT INTO [dbo].[ufd_Recipes]
( 
		 [Id],[Title],[Description],[CookTimeMinutes],[PrepTimeMinutes],[TotalServings]
		,[IsAustralianCuisine],[IsCajunCreoleCuisine],[IsCaribbeanCuisine],[IsCentralAfricanCuisine],[IsCentralAmericanCuisine],[IsCentralAsianCuisine]
		,[IsCentralEuropeanCuisine],[IsChineseCuisine],[IsEastAfricanCuisine],[IsEastAsianCuisine],[IsEasternEuropeanCuisine],[IsFilipinoCuisine]
		,[IsGermanCuisine],[IsGreekCuisine],[IsFrenchCuisine],[IsFusionCuisine],[IsIndianCuisine],[IsIndonesianCuisine],[IsItalianCuisine],[IsJapaneseCuisine]
		,[IsKoreanCuisine],[IsMexicanCuisine],[IsNativeAmericanCuisine],[IsNorthAfricanCuisine],[IsNorthAmericanCuisine],[IsNorthernEuropeanCuisine],[IsOceanicCuisine]
		,[IsPakistaniCuisine],[IsPolishCuisine],[IsPolynesianCuisine],[IsRussianCuisine],[IsSoulFoodCuisine],[IsSouthAfricanCuisine],[IsSouthAmericanCuisine]
		,[IsSouthAsianCuisine],[IsSoutheastAsianCuisine],[IsSouthernEuropeanCuisine],[IsSpanishCuisine],[IsTexMexCuisine],[IsThaiCuisine],[IsWestAfricanCuisine]
		,[IsWestAsianCuisine],[IsWesternEuropeanCuisine],[IsVietnameseCuisine]
		,[IsGlutenFree],[IsNutFree],[IsVegetarian],[IsVegan]
		,[DateCreatedUtc],[DateLastModifiedUtc],[HouseholdId]
)
SELECT 
		 [Id],[Title],[Description],[CookTimeMinutes],[PrepTimeMinutes],[TotalServings]
		,[IsAustralianCuisine],[IsCajunCreoleCuisine],[IsCaribbeanCuisine],[IsCentralAfricanCuisine],[IsCentralAmericanCuisine],[IsCentralAsianCuisine]
		,[IsCentralEuropeanCuisine],[IsChineseCuisine],[IsEastAfricanCuisine],[IsEastAsianCuisine],[IsEasternEuropeanCuisine],[IsFilipinoCuisine]
		,[IsGermanCuisine],[IsGreekCuisine],[IsFrenchCuisine],[IsFusionCuisine],[IsIndianCuisine],[IsIndonesianCuisine],[IsItalianCuisine],[IsJapaneseCuisine]
		,[IsKoreanCuisine],[IsMexicanCuisine],[IsNativeAmericanCuisine],[IsNorthAfricanCuisine],[IsNorthAmericanCuisine],[IsNorthernEuropeanCuisine],[IsOceanicCuisine]
		,[IsPakistaniCuisine],[IsPolishCuisine],[IsPolynesianCuisine],[IsRussianCuisine],[IsSoulFoodCuisine],[IsSouthAfricanCuisine],[IsSouthAmericanCuisine]
		,[IsSouthAsianCuisine],[IsSoutheastAsianCuisine],[IsSouthernEuropeanCuisine],[IsSpanishCuisine],[IsTexMexCuisine],[IsThaiCuisine],[IsWestAfricanCuisine]
		,[IsWestAsianCuisine],[IsWesternEuropeanCuisine],[IsVietnameseCuisine]
		,[IsGlutenFree],[IsNutFree],[IsVegetarian],[IsVegan]
		,[DateCreatedUtc],[DateLastModifiedUtc],[GroupId]
FROM [dbo].[ul_Recipes];
SET IDENTITY_INSERT [dbo].[ufd_Recipes] OFF;

SET IDENTITY_INSERT [dbo].[ufd_RecipeIngredientGroups] ON;
INSERT INTO [dbo].[ufd_RecipeIngredientGroups]
( 
		 [Id]
		,[RecipeId]
		,[SortOrder]
		,[Title]
		,[DateCreatedUtc]
		,[DateLastModifiedUtc]
		,[HouseholdId]
)
SELECT 
		 [Id]
		,[RecipeId]
		,[SortOrder]
		,[Title]
		,[DateCreatedUtc]
		,[DateLastModifiedUtc]
		,[GroupId]
FROM [dbo].[ul_RecipeIngredientGroups] 
SET IDENTITY_INSERT [dbo].[ufd_RecipeIngredientGroups] OFF;

SET IDENTITY_INSERT [dbo].[ufd_RecipeIngredients] ON;
INSERT INTO [dbo].[ufd_RecipeIngredients]
( 
		 [Id]
		,[RecipeId]
		,[Key]
		,[Title]
		,[ListGroupId]
		,[SortOrder]
		,[Amount]
		,[AmountN]
		,[AmountUnit]
		,[AmountLabel]
		,[AmountText]
		,[PrepNote]
		,[DateCreatedUtc]
		,[DateLastModifiedUtc]
		,[HouseholdId]
)
SELECT 
		 [Id]
		,[RecipeId]
		,[Key]
		,[Title]
		,[ListGroupId]
		,[SortOrder]
		,[Amount]
		,[AmountN]
		,[AmountUnit]
		,[AmountLabel]
		,[AmountText]
		,[PrepNote]
		,[DateCreatedUtc]
		,[DateLastModifiedUtc]
		,[GroupId]
FROM [dbo].[ul_RecipeIngredients];
SET IDENTITY_INSERT [dbo].[ufd_RecipeIngredients] OFF;

SET IDENTITY_INSERT [dbo].[ufd_RecipeSteps] ON;
INSERT INTO [dbo].[ufd_RecipeSteps]
( 
		 [Id]
		,[RecipeId]
		,[SortOrder]
		,[Instructions]
		,[DateCreatedUtc]
		,[DateLastModifiedUtc]
		,[HouseholdId]
)
SELECT 
		 [Id]
		,[RecipeId]
		,[SortOrder]
		,[Instructions]
		,[DateCreatedUtc]
		,[DateLastModifiedUtc]
		,[GroupId]
FROM [dbo].[ul_RecipeSteps];
SET IDENTITY_INSERT [dbo].[ufd_RecipeSteps] OFF;

INSERT INTO [dbo].[ufd_RecipeStepIngredients]
(
		 [RecipeStepId]
		,[RecipeIngredientId]
		,[RecipeId]
)
SELECT 
		 [RecipeStepId]
		,[RecipeIngredientId]
		,[RecipeId]
FROM [dbo].[ul_RecipeStepIngredients];

SET IDENTITY_INSERT [dbo].[ufd_RecipeNotes] ON;
INSERT INTO [dbo].[ufd_RecipeNotes]
( 
		 [Id]
		,[RecipeId]
		,[SortOrder]
		,[Note]
		,[DateCreatedUtc]
		,[DateLastModifiedUtc]
		,[HouseholdId]
)
SELECT 
		 [Id]
		,[RecipeId]
		,[SortOrder]
		,[Note]
		,[DateCreatedUtc]
		,[DateLastModifiedUtc]
		,[GroupId]
FROM [dbo].[ul_RecipeNotes];
SET IDENTITY_INSERT [dbo].[ufd_RecipeNotes] OFF;

INSERT INTO [dbo].[ufd_ProductSubstitutions]
(
		 [IngredientKey]
		,[ProductId]
		,[HouseholdId]
		,[IsPrimary]
)
SELECT 
		 [IngredientKey]
		,[ProductId]
		,[GroupId]
		,[IsPrimary]
FROM [dbo].[ul_ProductSubstitutions];