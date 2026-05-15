create unique nonclustered index [UserNameIndex]
    on [dbo].[AspNetUsers] ([NormalizedUserName])
    where [NormalizedUserName] is not null;