create unique nonclustered index [RoleNameIndex]
    on [dbo].[AspNetRoles] ([NormalizedName])
    where [NormalizedName] is not null;