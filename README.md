```bash
dotnet new webapi -n NomeDoProjeto

dotnet run
```

```bash
dotnet ef migrations add 'nome'

dotnet ef migrations remove 'nome'

dotnet ef database update
```

```bash
# cria a solution do projeto
dotnet new sln -o Solucao

# cria o projeto
dotnet new Template -o NomeDoProjeto

# adiciona o projeto a solution criada
dotnet sln Solucao add NomeDoProjeto 

# adiciona uma referencia ao projeto
dotnet add NomeDoProjeto reference ProjetoReferencia
```
