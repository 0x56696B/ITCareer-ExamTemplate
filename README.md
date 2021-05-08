# ITCareer-ExamTemplate
'Cuz why not

## For Migrations:

I. ADD CONNECTION STRING AND EDIT DATABASE(`app.UseNpgsql()` to `app.UseMySql()` or whatever) IF NEEDED!!!  

II. Open CMD/Powershell

III. Navigate to the `Project.Data Folder` 
- `cd Data/Project.Data`

IV. Run migrations command(Use StartUpProject flag, because the project does not have a DbContextFactory!):
- `dotnet ef migrations add Intial -s ../../Project.Web` then `dotnet ef database update -s ../../Project.Web`

OR

- `Add-Migration InitialCreate -StartUpProjectName ../../Project.Web` then `Update-Database -StartUpProjectName ../../Project.Web`
