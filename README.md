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

---

### Clean Architecture

- Independente de Frameworks
- Testável
- Independente da Camada de Apresentação (UI)
- Independente do Banco de Dados
- Independente de fatores externos
